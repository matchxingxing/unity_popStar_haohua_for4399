using UnityEngine;

public class Game : MonoBehaviour {
    public GameObject startUI;
    public GameObject propUI;
    public GameObject gameMessageUI;
    public GameObject gameBg;
    public GameObject textAnimObj;
    public GameObject shopUI_Prefab;
    public GameObject noviceBag_Prefab;
    public GameObject helpUI_Prefab;
    public GameObject pauseUI_Prefab;
    public GameObject selectColorBar_Prefab;
    public GameObject activityBag_Prefab;
    public GameObject receiveBag_Prefab;
    public GameObject failureUI_Prefab;
    public GameObject wantContinueUI_Prefab;
    public GameObject screenText_Prefab;
    public GameObject mysteryPrize_Prefab;


    private StarsShow _starsShow;
    private int _level;
    private int _targetScore;
    private int _score;
    private int _maxScore;
    private int _diamondCount;//拥有的钻石数量
    private int _scoreMul;//分数倍数
    private bool _mute;
    private bool _isSelectColoring;
    private bool _isBanClickStar;
    private bool _isPopOneing;
    private bool _isGameOver=true;
    private bool _isReceiveNoviceBag;//是否领取了新手礼包
    private bool _pause;
    private bool _isToTargetScored;

    private int _starTweenShowCount;//星星缓动动画完成个数
    private static Game _instance;
    public static Game instance{ get{return _instance;} }

    void Awake(){
        _instance=this;
        _isReceiveNoviceBag = PlayerPrefs.GetInt("isReceiveNoviceBag",0)==1;
        if(PlayerPrefs.GetInt("isGiveDiamonded",0)==0){
            SetDiamondCount(20);//第一次游戏 送钻石
            PlayerPrefs.SetInt("isGiveDiamonded",1);
        }
        _diamondCount = PlayerPrefs.GetInt("diamondCount",0);
        if(GameData.isClearData)  PlayerPrefs.DeleteAll();
    }

    void Start() {
        Screen.SetResolution(480,800,false,60);

        ToTitle();
        if (!_isReceiveNoviceBag)
            CreateNoviceBag();
    }

    public void ToTitle(){
        GameObject.Instantiate(startUI);
        SoundMan.instance.GetComponent<AudioSource>().Stop();
        SoundLoopMan.instance.GetComponent<AudioSource>().Stop();
        SoundLoopMan.instance.Play("背景",0.3f);
    }

    public void ToShop(){
        GameObject.Instantiate(shopUI_Prefab);
    }

    public void CreatePauseUI(){
        _pause = true;
        Object.Instantiate(pauseUI_Prefab);
    }
    
    /**添加分数*/
    public void AddScore(int value, bool isRightNow){
        SetScore(_score +value,isRightNow);
    }

    /**新游戏*/
    public void NewGame(){
        PlayerPrefs.SetInt("level",1);
        PlayerPrefs.SetInt("score", 0);
        NewLevelA();
    }

    /**继续游戏*/
    public void ContinueGame(){
        NewLevelA();
    }

    private void NewLevelA(){//开始游戏、继续游戏都执行
        _isGameOver = false;
        _pause=false;
        _isToTargetScored=false;
        _level = PlayerPrefs.GetInt("level",1);

        if(GameObject.Find("gameBg")==null){
            GameObject gameBgObj = (GameObject)GameObject.Instantiate(gameBg);
            gameBgObj.name = "gameBg";
        }
        if(GameObject.Find("gameMessageUI")==null){
            GameObject gameMessageUIObj = (GameObject)GameObject.Instantiate(gameMessageUI);
            gameMessageUIObj.name = "gameMessageUI";
        }
        if(GameObject.Find("propUI")==null){
            GameObject propUIObj = (GameObject)GameObject.Instantiate(propUI);
            propUIObj.name ="propUI";
            PropUIControl propUIControl = propUIObj.GetComponent<PropUIControl>();
            propUIControl.SetEnabled(false);//星星出现动画播放完再激活
        }

        SetScore(PlayerPrefs.GetInt("score",0),true);
        _maxScore = PlayerPrefs.GetInt("maxScore",0);
        _targetScore = GameData.getTargetScore(_level);
        _starTweenShowCount=0;
        _scoreMul = 1;
        GameObject.Instantiate(textAnimObj);
        gmUI.Init(_level,_maxScore,_targetScore);
        GameObject.Destroy(GameObject.Find("failureUI"));
    }
    /**播放完文字动画后执行*/
    public void NewLevelB(){
        _starsShow = new StarsShow();
        _starsShow.Init(this,new Vector2(-2.4f,-3.6f));

        RandomArr.Instance();
        int[] cnts = RandomArr.RandElement(100,5,12,28);//得到长度为5的数组,各个元素的和为100,元素最小12最大28如:[16, 14, 26, 24, 20 ]
         /*  for(int i=0; i<5; i++){
                Debug.Log(cnts[i]);
           }*/
        _starsShow.NewStarsPosition(cnts);
        //
        SoundMan.instance.PlayOneShot("星星出现动画");
    }

    public void CheckGameVictoryFailure(){
        if(_score>=_targetScore)Victory();
        else Failure();
    }

    private void Victory(){
        //保存最高记录分数
        if(_score>_maxScore) PlayerPrefs.SetInt("maxScore", _score);
        //保存当前得分
        PlayerPrefs.SetInt("score",_score);
        //保存关卡
        PlayerPrefs.SetInt("level", _level+1);
        //
        //////////////////////////////////////////////////////////////////////////
        //创建领取礼包界面,true表示关闭界面后继续游戏
        CreateReceiveBag(true);
    }
    private void Failure(){
        gmUI.ShowOrHideReward(true);//隐藏奖励文本
        //创建失败界面
        GameObject failureUIObj = (GameObject)GameObject.Instantiate(failureUI_Prefab);
        failureUIObj.name = "failureUI";
    }


    public void FailureBackToTitle(){
        //清除保存的关卡、分数
        PlayerPrefs.SetInt("level",1);
        PlayerPrefs.SetInt("score",0);
        ClearGameContent();
        //返回到标题
        ToTitle();
    }

    public void ClearGameContent(){
        _starsShow.Dispose();
        _starsShow = null;
        _isGameOver = true;
        //清除界面
        Object.Destroy(GameObject.Find("gameBg"));
        Object.Destroy(GameObject.Find("gameMessageUI"));
        Object.Destroy(GameObject.Find("propUI"));
    }

    /**创建新手礼包*/
    public void CreateNoviceBag(){
        Object.Instantiate(noviceBag_Prefab);
    }

    /*创建活动大礼包*/
    public void CreateActivityBag(){
        Object.Instantiate(activityBag_Prefab);
    }

    /**创建领取大礼包*/
    public void CreateReceiveBag(bool destroyIsContinueGame=false){
        GameObject gObj= (GameObject)Object.Instantiate(receiveBag_Prefab);
        ReceiveBagScript rBag = gObj.GetComponent<ReceiveBagScript>();
        rBag.SetDestroyIsContinueGame(destroyIsContinueGame);
    }

    /*神秘大奖*/
    public void CreateMysteryPrize(){
       GameObject gObj = (GameObject)Object.Instantiate(mysteryPrize_Prefab);
       gObj.name = "mysteryPrize";
    }

    public void CreateWantContinueUI(){
        Object.Instantiate(wantContinueUI_Prefab);
    }

    
    public void CreateScreenText(string text){
        GameObject gObj=(GameObject)Object.Instantiate(screenText_Prefab);
        ScreenTextScript script=gObj.GetComponentInChildren<ScreenTextScript>();
        script.Init(text);
    }

    /**帮助面板*/
    public void ToHelp(){
        Object.Instantiate(helpUI_Prefab);
    }

    void Update(){
        if(!_isGameOver && !_isToTargetScored && _starTweenShowCount>=100){
            //到达目标分 
            if (_score >= _targetScore) {
                Object.Instantiate(Resources.Load("Prefabs/effect_clear"));
                SoundMan.instance.PlayOneShot("达到目标分");
                _isToTargetScored=true;
            }
        }
        //if(_starTweenShowCount<100)return;//星星出现动画没完成则返回
        
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape)) {
            if (!_isGameOver && !_pause){
                //游戏中，按返回键
                CreatePauseUI();
            }else if(GameObject.Find("startUI")!=null){
                if(GameObject.Find("mysteryPrize")==null){
                    CreateMysteryPrize();
                }else{
                    Application.Quit();
                }
            }
        }
    }

    void OnGUI(){
        if(_starTweenShowCount<100)return;//星星出现动画没完成则返回
        if(Event.current.isMouse && Event.current.type == EventType.MouseDown){
            if(!_isBanClickStar&&!_pause){
                if(_starsShow!=null && _isPopOneing) PopOneHandler();
                else if(_starsShow!=null && _isSelectColoring) CreateSelectColorBar();
                else MouseDownStarHandler();
            }
        }
    }

    private GameObject GetObjByMousePos(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if(Physics.Raycast(ray,out raycastHit))
            return raycastHit.transform.gameObject;
        return null;
    }

    private void MouseDownStarHandler(){
        GameObject obj = GetObjByMousePos();
        if(obj) {
            Star star = obj.GetComponent<Star>();
            if(star&&star.isStopMove==false){
                 if(!star.isDestroying) _starsShow.Pop(star.posId);
            }
        }
    }

    private void PopOneHandler(){
        GameObject obj = GetObjByMousePos();
        if(obj) {
            Star star = obj.GetComponent<Star>();
            if(star&&star.isStopMove==false) {
                if(!star.isDestroying){
                    _starsShow.PopOne(star.posId);
                    _isPopOneing=false;
                    //恢复道具栏可用
                    SetPropUIEnabled(true);
                }
            }
        }
    }

    private void SetPropUIEnabled(bool value){
        GameObject propUIObj = GameObject.Find("propUI");
        if (propUIObj) {
            PropUIControl propUIControl = propUIObj.GetComponent<PropUIControl>();
            propUIControl.SetEnabled(value);
        }
    }

    /**创建选择颜色面板*/
    private void CreateSelectColorBar(){
        GameObject obj = GetObjByMousePos();
        if (obj) {
            Star star = obj.GetComponent<Star>();
            if (star) {
                if(!star.isDestroying){
                    GameObject gObj = (GameObject)Object.Instantiate(selectColorBar_Prefab);
                    gObj.BroadcastMessage("Init",star);
                    _isBanClickStar = true;
                 }
            }
        }
    }
    public void ChangeStarColor(Star star, int targetCor){
        _starsShow.SelectColor(star.color, targetCor, star.posId);
        _isSelectColoring = false;
        _isBanClickStar=false;
        SoundMan.instance.PlayOneShot("Props_changeColor");
    }

    /**道具随机*/
    public void PropRandom(){
        _starsShow.Upset();
        SoundMan.instance.PlayOneShot("Props_random");
    }
    /**道具选色*/
    public void PropSelectColor(){
        _isSelectColoring = true;
    }
    /**道具消除一个*/
    public void PropPopOne(){
        _isPopOneing = true;
    }
    /**道具分数加倍*/
    public void CoinAdd(){
        _scoreMul=2;
    }

    public void SetScoreMul(int mul){
        _scoreMul = mul;
        if(mul==1){
            //恢复道具栏可用
            SetPropUIEnabled(true);
        }
    }

    private void SetScore(int value,bool isRightNow){
        _score = value;
        GameObject gmUIObj = GameObject.Find("gameMessageUI");
        if(gmUIObj){
            GameMessageUIScript gmUI = gmUIObj.GetComponent<GameMessageUIScript>();
            if(gmUI)gmUI.SetScore(_score,isRightNow);
        }
    }

    public void SetDiamondCount(int value){
        _diamondCount = value;
        PlayerPrefs.SetInt("diamondCount",_diamondCount);
        //Debug.Log("钻石数量："+_diamondCount);
    }

    public void AddDiamondCount(int value){
        SetDiamondCount(_diamondCount+value);
    }

    public void SetMute(bool value){
        _mute = value;
        if(_mute){
            //静音
            AudioListener.volume = 0f;
        }else{
            //恢复声音
            AudioListener.volume = 1;
        }
    }

    public void SetStarTweenShowCount(int value){
        _starTweenShowCount = value;
        //星星动画播放完成
        if(_starTweenShowCount>=100){
            //恢复道具栏可用
            SetPropUIEnabled(true);
        }
    }
    public void SetIsGameOver(bool value){
        _isGameOver = value;
        if(_isGameOver)SetPropUIEnabled(false);
    }
    public void SetIsReceiveNoviceBag(bool value){
        if(value==false)return;//只能设true
        _isReceiveNoviceBag = value;
         PlayerPrefs.SetInt("isReceiveNoviceBag",1);
    }
    public void SetIsBanClickStar(bool value){
        _isBanClickStar = value;
    }
    public void SetPause(bool value){
        _pause = value;
    }
    /**接收支付结果*/
    void resultMessgae(string str){
         GameObject payCaller = GameObject.Find("PayCaller");
        if(payCaller){
            PayCaller pScript = payCaller.GetComponent<PayCaller>();
            pScript.PayCallbackHandler(str);
        }
     }
    public int starTweenShowCount{get{return _starTweenShowCount;}}
    public int level{get{return _level;}}
    public int targetScore{get{return _targetScore;}}
    public int score{get{return _score;}}
    public int maxScore{get{return _maxScore;}}
    public int diamondCount{get{return _diamondCount;}}
    public int scoreMul{get{return _scoreMul;}}
    public bool mute{get{return _mute;}}
    public bool isGameOver{get{return _isGameOver;}}
    public bool isReceiveNoviceBag{get{return _isReceiveNoviceBag;}}
    public bool pause{get{return _pause;}}
    public GameMessageUIScript gmUI{
        get{
            GameObject gmUIObj = GameObject.Find("gameMessageUI");
            GameMessageUIScript gm = gmUIObj.GetComponent<GameMessageUIScript>();
            return gm;
        }
    }

}