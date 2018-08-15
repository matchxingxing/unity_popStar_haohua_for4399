using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ScreenTextScript : MonoBehaviour {
    private RectTransform _rectTransform;
    private Text _txt;
    private Vector2 _pos;
    void Awake(){
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _txt = gameObject.GetComponent<Text>();
        _pos = _rectTransform.anchoredPosition;
        _rectTransform.anchoredPosition -= new Vector2(0f,50f);
        _txt.color = Color.clear;
    }

    void Start() {
        Tweener tweener = _rectTransform.DOAnchorPos(new Vector2(_pos.x,_pos.y),0.5f);
        tweener.SetUpdate(true);
        tweener.OnComplete(Complete1);

        tweener = _txt.DOColor(new Color(1f,1f,0f),0.5f);
        tweener.SetUpdate(true);
    }

    public void Init(string text){
        _txt.text = text;
    }

    private void Complete1(){
        Tweener tweener = _rectTransform.DOAnchorPos(new Vector2(_pos.x,_pos.y+50f),0.5f);
        tweener.SetUpdate(true);
        tweener.SetDelay(2);
        tweener.OnComplete(Complete2);

        tweener = _txt.DOColor(Color.clear,0.5f);
        tweener.SetUpdate(true);
        tweener.SetDelay(2);
    }


    private void Complete2(){
        Object.Destroy(gameObject.transform.parent.gameObject);
    }
}
