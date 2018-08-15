using UnityEngine;
using System.Collections;

public class SelectColorBarButtonControl : UGUIButtonControl {
    public int colorId=-1;
    public GameObject thisRoot;
    private Star _star;
    protected override void OnClick() {
        Game.instance.ChangeStarColor(_star,colorId);
        //道具栏恢复可用 
        PropUIControl propUIControl= GameObject.Find("propUI").GetComponent<PropUIControl>();
        propUIControl.SetEnabled(true);
        //消毁选色面板
        Object.Destroy(thisRoot);
    }

    private void Init(Star star){
        _star = star;
    }

}
