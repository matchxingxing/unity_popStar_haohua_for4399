using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class TextAnimScript : MonoBehaviour {
    private Text _txtLevel;
    private Text _txtTargetScore;
    

    void Awake(){
        gameObject.name = "textAnimObj";
        _txtLevel = GameObject.Find("textAnimObj/txtLevel").GetComponent<Text>();
        _txtTargetScore = GameObject.Find("textAnimObj/txtTargetScore").GetComponent<Text>();

    }

    void Start() {
        SetText(Game.instance.level.ToString(),Game.instance.targetScore.ToString());

        Rect r = _txtLevel.rectTransform.rect;
        _txtLevel.rectTransform.anchoredPosition = new Vector2((GameData.stageW>>1)+r.width*0.5f,32);

        r = _txtTargetScore.rectTransform.rect;
        _txtTargetScore.rectTransform.anchoredPosition = new Vector2((GameData.stageW>>1)+r.width*0.5f,-32);

        _txtLevel.rectTransform.DOAnchorPos(new Vector2(0,32),0.25f).OnComplete(Complete1);
    }

    private void Complete1(){
        _txtTargetScore.rectTransform.DOAnchorPos(new Vector2(0,-32),0.25f).OnComplete(Complete2);
    }
    private void Complete2(){
        Invoke("OutHandler",0.75f);
    }

    private void OutHandler(){
        Rect r = _txtLevel.rectTransform.rect;
        _txtLevel.rectTransform.DOAnchorPos(new Vector2(-((GameData.stageW>>1)+r.width*0.5f),32),0.25f);

        r = _txtTargetScore.rectTransform.rect;
        Tweener tweener = _txtTargetScore.rectTransform.DOAnchorPos(new Vector2(-((GameData.stageW>>1)+r.width*0.5f),-32),0.25f);
        tweener.OnComplete(OutComplete);
    }

    private void OutComplete(){
        Destroy(gameObject);
    }

    public void SetText(string level,string targetScore) {
        _txtLevel.text = (GameData.version == "cn" ? "关卡 " : "level ") + level;
        _txtTargetScore.text = (GameData.version == "cn" ? "目标分数 " : "target score ") + targetScore;
    }

    void OnDestroy() {
        Game.instance.NewLevelB();
    }

    
}
