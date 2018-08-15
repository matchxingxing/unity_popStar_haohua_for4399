using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PropUIButtonControl : UGUIButtonControl {

    protected override void OnClick() {
        int needDiamond = 5;
        if(Game.instance.diamondCount<needDiamond){
            Game.instance.CreateReceiveBag();
        }else{
            switch(gameObject.name){
                case "Props_SelectColor"://选色
                    SendMessageUpwards("SetEnabled",false);
                    Game.instance.PropSelectColor();
                    Game.instance.SetDiamondCount(Game.instance.diamondCount-needDiamond);//扣除钻石
                    Game.instance.CreateScreenText("点击需要改变颜色的水果");
                    PlaySound();
                    break;
                case "Props_Random"://随机
                    Game.instance.PropRandom();
                    Game.instance.SetDiamondCount(Game.instance.diamondCount-needDiamond);//扣除钻石
                    Game.instance.CreateScreenText("重新排列");
                    PlaySound();
                    break;
                case "Props_CoinAdd"://分数加倍
                    SendMessageUpwards("SetEnabled",false);
                    Game.instance.CoinAdd();
                    Game.instance.SetDiamondCount(Game.instance.diamondCount-needDiamond);//扣除钻石
                    Game.instance.CreateScreenText("下一次获得分数加倍");
                    PlaySound();
                    break;
                case "Props_PopOne"://消除一个
                    SendMessageUpwards("SetEnabled",false);
                    Game.instance.PropPopOne();
                    Game.instance.SetDiamondCount(Game.instance.diamondCount-needDiamond);//扣除钻石
                    Game.instance.CreateScreenText("点击消除一个水果");
                    PlaySound();
                    break;
            }
        }
    }

    private void PlaySound(){
        SoundMan.instance.PlayOneShot("coin",0.7f);
    }

    private void ChangeEnabled(bool value){
        _btn.enabled = value;
        Image image = gameObject.GetComponent<Image>();
        image.color = new Color(1,1,1,value ? 1.0f : 0.5f);
    }
}
