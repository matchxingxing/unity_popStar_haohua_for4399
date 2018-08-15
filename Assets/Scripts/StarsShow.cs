using UnityEngine;
using System.Collections.Generic;
public class StarsShow {
    private List<GameObject>[] _stars;     //全部星星
    private Vector2 _offset;
    private int _selectTotal = 0;
    private StarsPosition _starsPosition;
    private int[] _mvs = new int[102];
    private Game _game;

    public void Init(Game game, Vector2 offset) {
        _game = game;
        _offset = offset;
        _stars = new List<GameObject>[10];
        for(int i = 0; i < 10; i++) _stars[i] = new List<GameObject>(10);
    }

    public void Dispose(){
        _starsPosition=null;
        _mvs=null;
        for(int i=0;i<_stars.Length;i++){
            for(int j=0;j<_stars[i].Count;j++){
                Object.Destroy(_stars[i][j].gameObject);
            }
        }
    }

    public void NewStarsPosition(int[] cnts) {
        //清理
        _selectTotal = 0;
        int i; int j;
        for(i = 0; i < 10; i++) {
            for(j = 0; j < _stars[i].Count; j++) _stars[i][j].GetComponent<Star>().Dispose();
            _stars[i].Clear();
            for(j = 0; j < 10; j++) _stars[i].Add(null);
        }

        //新建
        int id;
        StarsBoard[] starsBoards = new StarsBoard[] { new StarsBoard(), new StarsBoard(), new StarsBoard(), new StarsBoard(), new StarsBoard() };
        for(i = 0; i < 5; i++) {
            starsBoards[i].SetAll(false);
            for(j = 0; j < cnts[i]; j++) {
                id = RandomArr.GetNextRandomElement();
                starsBoards[i].SetTrue(id);
                PushStar(i, id, true);
            }
            //Debug.Log( starsBoards[i].toString());
        }
        _starsPosition = new StarsPosition(starsBoards);
    }

    private void PushStar(int color, int idx, bool isTweenShow=false) {
        GameObject prefabStar = (GameObject)Resources.Load("Prefabs/Star");
        int x_id = idx >> 4;//0-9
        int y_id = 9 - (idx & 0xf);//0-9
        GameObject star;
        Vector3 starPos = new Vector3(0.48f * x_id + _offset.x + 0.24f, 0.48f * y_id + _offset.y + 0.24f, 0f);
        star = GameObject.Instantiate(prefabStar, starPos, new Quaternion()) as GameObject;
        star.GetComponent<Star>().Init(color, idx,this);
        if(isTweenShow){
            star.GetComponent<Star>().TweenShow(_offset);
        }
        _stars[x_id][y_id] = star;
    }

    /**测试左移*/
    private void CheckMoveLeft(List<Star> moveStars){
        int[] tmp = _starsPosition.DoMoveToLeft();
        Star star;
        for (var i = 0; i < tmp[10]; i++) {
            if (tmp[i] != i) {
                //将现在应该存储的存储到LIST
                _stars[i].AddRange(_stars[tmp[i]]);
                //依次移动
                for (var j = 0; j < _stars[i].Count; j++) {
                    star = _stars[i][j].GetComponent<Star>();
                    star.AddMoveLeft(tmp[i] - i, _offset);
                    star.isStopMove=true;//设true禁止Update移动，等待逐个删除完成
                    moveStars.Add(star);
                }
                _stars[tmp[i]].Clear();
            }
        }
    }
    /**检测游戏结束*/
    private void CheckGameOver(){
        int[][] retval = new int[50][];
        if (_starsPosition.NextGenerateMove(ref retval) == 0) {
            DestroyRemainStars();//消除剩下的星星，奖励得分
            _game.SetIsGameOver(true);
        }
    }

    /**消除游戏结束余下的星星*/
    private void DestroyRemainStars(){
        //遍历找到余下星星的 posid
        List<Star> stars = new List<Star>();
        Star star;
        int i,j;
        for(i=0; i<10; i++){
            for(j=0; j<_stars[i].Count; j++){
                star = _stars[i][j].GetComponent<Star>();
                stars.Add(star);
            }
            _stars[i].Clear();
        }
        //
        _game.gmUI.ShowOrHideReward(false);
        _game.gmUI.SetRewardScore(2000,2000, true);
        _game.gmUI.SetRemainStars(stars.Count);
        //
        //计算要奖励的分数
        int rewardScore = 0;
        if (stars.Count < 10) rewardScore = 2000 - stars.Count * stars.Count * 20;
        _game.gmUI.SetRewardScore(rewardScore,2000,false);
        //
        DeleteOneByOne(stars,null,false,1);
    }

    public void Pop(int id) {
        _starsPosition.GetBlackByPoint(id, ref _mvs);
       for(int i=0;i<102;i++){
        //Debug.Log(_mvs[i]);
       }

        if(_mvs[101] >= 2) {//相同颜色的方块>=2
            _selectTotal = (int)_mvs[101];
            //从局面中弹出
            _starsPosition.DoPop(_mvs);
            //消除星星
            int x, y; 
            Star star; 
            List<Star> popStars=new List<Star>();
            List<Star> moveStars=new List<Star>();
            for(int i = 0; i < _selectTotal; i++) {
                x = _mvs[i] >> 4;
                y = 9 - (_mvs[i] & 0xf);
                star = _stars[x][y].GetComponent<Star>();
                popStars.Add(star);//添加到要删除星星列表进行逐个删除
                _stars[x].RemoveAt(y);

                for(int j = y; j < _stars[x].Count; j++){
                    star = _stars[x][j].GetComponent<Star>();
                    star.AddMoveDown(_offset);
                    star.isStopMove=true;//设true禁止Update移动，等待逐个删除完成
                    moveStars.Add(star);
                }
            }
            //测试左移
            CheckMoveLeft(moveStars);
            int scoreMul = _game.scoreMul;
            if(scoreMul>1)_game.SetScoreMul(1);
            //一个个删除
            DeleteOneByOne(popStars, moveStars,true, scoreMul);
            //若盘面达到终结，计算奖励分值，结束当前
            CheckGameOver();
        }
    }

    private void DeleteOneByOne(List<Star> popStars,List<Star> moveStars=null, bool isAddScore=true, int scoreMul=1) {
        int total=popStars.Count; float interval=0.07f; float time=0;
        //int totalScore = total*total*5;//消除分数计算公式
        //算出每颗的加的分值
        int[] scoreArr = new int[total];
        int tempScore=0;
        int i;
        for (i = 0; i < total; i++) {
            tempScore=(i*10+5)*scoreMul;
            scoreArr[i]=tempScore;
        }
        /*  0 不错呦：6、7
            1 非常棒：8、9
            2 帅呆啦：10~
        */
        int effectTypeId=-1;
        if(isAddScore){
            if(total>10)effectTypeId=2;
            else if(total>=8&&total<=9)effectTypeId=1;
            else if(total>=6&&total<=7)effectTypeId=0;
        }
        //
        int addScore; 
        for(i=0; i<total; i++){
            time+=Mathf.Min(0.2f,interval*i);
            if(!isAddScore){
                if(i>=10){//在游戏结束时，个数>10的一次性消除
                    for (int j = i; j < total; j++){
                        if(j==total-1){//最后一个时，立即显示要奖励的分值
                            popStars[j].DelayPop(time,0,null,true,-1);
                        }else{
                            popStars[j].DelayPop(time,0,null,false,-1);
                        }
                    }
                    break; 
                }
            }
            addScore = isAddScore?scoreArr[i]:0;
            if(i==total-1) popStars[i].DelayPop(time, addScore, moveStars,!isAddScore,effectTypeId);//消除的最后一个允许星星向下移动,创建鼓励语
            else popStars[i].DelayPop(time, addScore,null,false,-1);
        }
    }

    //消除一个
    public void PopOne(int id) {
        int scoreMul = _game.scoreMul;
        if(_game.scoreMul>1)_game.SetScoreMul(1);
        _starsPosition.DoPopOne(id);
        int x, y;
        Star star; 
        List<Star> popStars = new List<Star>();
        List<Star> moveStars = new List<Star>();
        x = id >> 4;
        y = 9 - (id & 0xf);
        star = _stars[x][y].GetComponent<Star>();
        popStars.Add(star);//添加到要删除星星列表进行逐个删除
        _stars[x].RemoveAt(y);

        for (int j = y; j < _stars[x].Count; j++) {
            star = _stars[x][j].GetComponent<Star>();
            star.AddMoveDown(_offset);
            star.isStopMove = true;//设true禁止Update移动，等待逐个删除完成
            moveStars.Add(star);
        }
        //测试左移
        CheckMoveLeft(moveStars);
        DeleteOneByOne(popStars, moveStars,true, scoreMul);
        //若盘面达到终结，计算奖励分值，结束当前
        CheckGameOver();
    }

    /**打乱*/
    public void Upset() {
        //获取星星颜色，位置id，准备打乱
        List<int> cors = new List<int>();
        List<int> posIds = new List<int>();
        Star star;
        int i, j;
        for(i = 0; i < 10; i++) {
            for(j = 0; j < _stars[i].Count; j++) {
                star = _stars[i][j].GetComponent<Star>();
                cors.Add(star.color);
                posIds.Add(star.posId);
                star.Dispose();
            }
            _stars[i].Clear();
            for(j = 0; j < 10; j++) _stars[i].Add(null);
        }
        int[] corArr = cors.ToArray();
        int[] posIdArr = posIds.ToArray();
        RandomArr.Randomize(corArr,corArr.Length);
        //重建
        StarsBoard[] starsBoards = new StarsBoard[] { new StarsBoard(), new StarsBoard(), new StarsBoard(), new StarsBoard(), new StarsBoard() };
        for(i = 0;i < 5;i++) starsBoards[i].SetAll(false);
        int cor; int posId;
        for(i=0; i<corArr.Length; i++){
            cor = corArr[i];
            posId = posIdArr[i];
            starsBoards[cor].SetTrue(posId);
            PushStar(cor,posId);
        }
        _starsPosition = new StarsPosition(starsBoards);
        //移除空的元素
        i = _stars.Length;
        while(--i>=0){
            j=_stars[i].Count;
            while(--j>=0){  if(!_stars[i][j]) _stars[i].RemoveAt(j); }
        }
    }

    public void SelectColor(int curCor, int targetCor, int id){
        //////////1.改变starsBoards
        StarsBoard[] starsBoards = _starsPosition.starsBoard;
        starsBoards[curCor].SetFalse(id);//当前颜色列表中移出
        starsBoards[targetCor].SetTrue(id);//加到新的列表

        //////////2.改变stars
        int x = id >> 4;
        int y = 9 - (id & 0xf);
        _stars[x][y].GetComponent<Star>().ChangeColor(targetCor);
    }

    public Vector2 offset { get { return _offset; } }
}

class StarObject {
    public int color;
    public int posId;
    public StarObject(int color, int posId) {
        this.color = color;
        this.posId = posId;
    }
}