using UnityEngine;
using System.Collections;

public class SelectColorBarChild : MonoBehaviour {
    private RectTransform _rectTransform;

    void Awake(){
        _rectTransform = gameObject.GetComponent<RectTransform>();
    }

    void Start() {
        
    }

    private void Init(Star star){
       Vector3 screenPos = Camera.main.WorldToScreenPoint(star.gameObject.transform.position);
       if(gameObject.name == "container1"){//限制选色栏超出屏幕
            float guiScaleFactor = (float)Screen.width/GameData.stageW;
            Rect r = _rectTransform.rect;
            float minX = r.width*0.5f*guiScaleFactor;
            float maxX = Screen.width-r.width*0.5f*guiScaleFactor;
            if(screenPos.x>maxX) screenPos=new Vector3(maxX, screenPos.y,screenPos.z);
            else if(screenPos.x<minX)screenPos=new Vector3(minX, screenPos.y,screenPos.z);
        }
        _rectTransform.transform.position = screenPos;
    }

}
