using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreAnimScript : MonoBehaviour {
    private int _score;
    private Vector2 _endPos;
    private Text _txt;
    private RectTransform _rectTransform;

    void Awake(){
        _txt = gameObject.GetComponent<Text>();
        _rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void Init(int score, float startPosX, float startPosY){
        _score = score;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(startPosX,startPosY,0));
        _rectTransform.position = screenPos;

        _txt.text = _score.ToString();
        
        _endPos = (GameObject.Find("gameMessageUI").GetComponent<GameMessageUIScript>()).GetScoretextPos();

        Tweener tweener = _rectTransform.DOAnchorPos(_endPos,0.8f);
        tweener.OnComplete(Complete);
    }

    private void Complete(){
        Game.instance.AddScore(_score,false);
        GameObject.Destroy(gameObject.transform.parent.gameObject);
    }
}
