using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Star: MonoBehaviour {
    private int _posId;
    private int _color;
    private static Texture2D[] _textures;
    private Vector2 _initPos;
    private float _angleRadian;
    private bool _isTweenShowing;
    private bool _isToWeenPosing;
    private Vector2 _tweenPos;
    private Game _game;
    //private StarsShow _starsShow;
    private bool _isResumeY;
    private GameObject _particle;
    private bool _isDestroying;

    //下落或向左要移动的位置
    private float _moveX;
    private float _moveY;

    public bool isStopMove;

    public void Init(int color, int posId,StarsShow starsShow) {
        _color = color;
        _posId = posId;
        //_starsShow = starsShow;
        _moveX = 1e6f;
        _moveY = 1e6f;
        _initPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        _game = GameObject.Find("Main Camera").GetComponent<Game>();
        _particle = (GameObject)GameObject.Instantiate((GameObject)Resources.Load("Prefabs/StarParticle"), new Vector3(0,0,-1), new Quaternion());
        ChangeColor(_color);
    }

    public void ChangeColor(int color) {
        string[] names = new string[]{"r","g","b","v","y"};

        Texture2D starTexture = (Texture2D)Resources.Load("Sprites/"+names[color]);
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.sprite = Sprite.Create(starTexture, new Rect(0, 0, 48, 48), new Vector2(0.5f, 0.5f));
		renderer.sortingOrder=0;
        //
        
        _particle.GetComponent<Renderer>().material.mainTexture = (Texture2D)Resources.Load("Sprites/star");
        //改变粒子颜色
        Color[] colors = new Color[]{new Color(1f,0f,0f), new Color(0f,1f,0f), new Color(0f,1f,1f), new Color(0.8f,0f,0.8f), new Color(1f,1f,0f)};
        _particle.GetComponent<Renderer>().material.color = colors[color];
    }

    /**缓动出现*/
    public void TweenShow(Vector2 offset) {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        //包围星星矩阵的圆的半径
        float len = 3.4f;
        renderer.color = Color.clear;
        //星星除去初始偏移位置后的x,y
        float x0 = _initPos.x-offset.x-0.24f;
        float y0 = _initPos.y-offset.y-0.24f;
        //星星矩阵的中心x,y
        float centerX = 5f*0.48f;
        float centerY = 5f*0.48f;
        //星星与中心点的夹角
        float dx = x0-centerX;
        float dy = y0-centerY;
        _angleRadian = Mathf.Atan2(dy, dx);
        //从中心向外偏移
        float x1 = _initPos.x + len * Mathf.Cos(_angleRadian);
        float y1 = _initPos.y + len * Mathf.Sin(_angleRadian);
        gameObject.transform.position = new Vector3(x1, y1, 0);

        float reAngleRadian = _angleRadian+Mathf.PI;
        _tweenPos = new Vector2(_initPos.x+0.4f*Mathf.Cos(reAngleRadian),
                                _initPos.y+0.4f*Mathf.Sin(reAngleRadian));
        //计算延迟时间，离中心越远时间越长
        float d = Mathf.Sqrt(dx * dx + dy * dy);
        float rate = d/4; if(rate>1)rate=1;
        float time = 1f*rate;
        Invoke("SetTweenShowing",time);
    }

    private void SetTweenShowing(){
        _isTweenShowing = true;
        _isToWeenPosing = true;
    }

    private List<Star> _moveStars;
    private int _score;
    private bool _isRightNowShowWardScore;//是否立即显示要奖励的分数
    private int _effectTypeId=-1;
    public void DelayPop(float time, int score=0,List<Star> moveStars=null, bool isRightNowShowWardScore=false,int effectTypeId=-1){
        _isDestroying = true;
        _moveStars=moveStars;
        _score = score;
        _isRightNowShowWardScore = isRightNowShowWardScore;
        _effectTypeId = effectTypeId;
        Invoke("PopHandler",time);
    }
    private void PopHandler(){
        //设置允许星星移动
        if(_moveStars!=null){
            for(int i = 0; i<_moveStars.Count; i++){
                if(_moveStars[i]!=null) _moveStars[i].isStopMove=false;
            }
            _moveStars=null;
        }

        //隐藏显示
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
        //发射粒子
        _particle.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,-1);
		ParticleSystem particleSys= _particle.GetComponent<ParticleSystem>();
		particleSys.Play();
        Invoke("Dispose",particleSys.duration + particleSys.startLifetime);

        //建分数动画
        if(_score>0){
            GameObject scoreAnimPrefab = Resources.Load<GameObject>("Prefabs/ScoreAnim");
            GameObject scoreAnimObj = (GameObject)GameObject.Instantiate(scoreAnimPrefab);
            ScoreAnimScript scoreAnim = scoreAnimObj.GetComponentInChildren<ScoreAnimScript>();
            scoreAnim.Init(_score,gameObject.transform.position.x,gameObject.transform.position.y);
            _score = 0;
        }

        ///创建鼓励语
        if(_effectTypeId>-1)CreateEffect(_effectTypeId);

        if(_isRightNowShowWardScore){
            _game.gmUI.SetRewardScore(_game.gmUI.rewardScore,-1, true);
            _game.AddScore(_game.gmUI.rewardScore,true);
            //检测游戏过关、失败
            _game.CheckGameVictoryFailure();
        }

        //
        SoundMan.instance.Play("pop");
    }

    private void CreateEffect(int id){
        Debug.Log("CreateEffect");
        string[] names = new string[]{"effect_bucuo","effect_feiChangBang","effect_shuaiDaiLe"};
        Object effPrefab = Resources.Load("Prefabs/"+names[id]);
        GameObject eff=Instantiate(effPrefab) as GameObject;
		var renderers=eff.GetComponentsInChildren<SpriteRenderer>();
		for(int i=0;i<renderers.Length;i++){
			renderers[i].sortingOrder=2;
		}

        string[] soundNames = new string[]{"combo_1","combo_2","combo_3"};
        SoundMan.instance.PlayOneShot(soundNames[id]);
    }

    public void AddMoveDown(Vector2 offset) {
        //移动位置
        _posId += 1;
        //更新要移动的y
        int y_id = 9 - (_posId & 0xf);
        _moveY = 0.48f * y_id + (offset.y + 0.24f);
    }

    public void AddMoveLeft(int n, Vector2 offset) {
        //移动位置
        _posId -= (int)((1 << 4) * n);
        //计算需要左移的距离
        int x_id = _posId >> 4;
        _moveX = 0.48f * x_id + (offset.x + 0.24f);
    }

    void Update() {
        //星星缓动出现动画 
        if (_isTweenShowing) {
            float reAngleRadian = _angleRadian + Mathf.PI;
            float dx = Mathf.Abs(gameObject.transform.position.x - _tweenPos.x);
            float dy = Mathf.Abs(gameObject.transform.position.y - _tweenPos.y);
            float d = Mathf.Sqrt(dx * dx + dy * dy);

            float speed = 0.10f;
            Vector3 v;
            if (_isToWeenPosing) {//在去缓动的位置中
                SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
                if (d <= speed) {
                    gameObject.transform.position = _tweenPos;
                    renderer.color = Color.white;
                    _isToWeenPosing = false;
                } else {
                    float vx = speed * Mathf.Cos(reAngleRadian);
                    float vy = speed * Mathf.Sin(reAngleRadian);
                    v = new Vector3(vx,vy,0);
                    gameObject.transform.position += v;

                    float rate = d / 0.7f;
                    if (rate > 1)
                        rate = 1;
                    rate = 1 - rate;
                    Color cor = new Color(1,1,1,rate);
                    renderer.color = cor;
                }
            } else {
                dx = _initPos.x - gameObject.transform.position.x;
                dy = _initPos.y - gameObject.transform.position.y;
                d = Mathf.Sqrt(dx * dx + dy * dy);
                if (d < 0.01f) {//星星缓动动画完成
                    gameObject.transform.position = _initPos;
                    _isTweenShowing = false;
                    _game.SetStarTweenShowCount(_game.starTweenShowCount+1);
                } else {
                    v = new Vector3((_initPos.x - gameObject.transform.position.x) * 0.15f,
                                    (_initPos.y - gameObject.transform.position.y) * 0.15f,0);
                    gameObject.transform.position += v;
                }
            }
        }else if(!isStopMove){
            //下落移动
            if(_moveY!=1e6f){
                float dy = Mathf.Abs((_moveY-0.14f)-gameObject.transform.position.y);
                if(_isResumeY){
                    dy = Mathf.Abs(_moveY-gameObject.transform.position.y);
                    if(dy<0.01f){
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x,_moveY,0);
                        _isResumeY=false;
                        _moveY=1e6f;
                    }else{
                        Vector3 v = new Vector3(0, (_moveY - gameObject.transform.position.y) * 0.5f, 0);
                        gameObject.transform.position += v;
                    }
                }else if(dy<0.03f){
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, _moveY-0.14f,0);
                    _isResumeY=true;
                    //播放下落声音
                    //
                }else{
                    Vector3 v = new Vector3(0,(_moveY-0.14f - gameObject.transform.position.y) * 0.3f,0);
                    gameObject.transform.position += v;
                }
            }
            //左移
            if(_moveX!=1e6f){
                float dx=Mathf.Abs(_moveX - gameObject.transform.position.x);
                if(dx<0.01f){
                    gameObject.transform.position = new Vector3(_moveX,gameObject.transform.position.y,0);
                    _moveX=1e6f;
                }else{
                    Vector3 v = new Vector3((_moveX - gameObject.transform.position.x) * 0.5f,0,0);
                    gameObject.transform.position += v;
                }
            }
        }

        
    }

    public void Dispose() {
        CancelInvoke();
        GameObject.Destroy(gameObject);
        GameObject.Destroy(_particle);

    }

    void OnDestroy(){
        GameObject.Destroy(_particle);
    }

    public int posId { get { return _posId; } }
    public int color { get { return _color; } }
    public bool isDestroying{get{ return _isDestroying; }}
}
