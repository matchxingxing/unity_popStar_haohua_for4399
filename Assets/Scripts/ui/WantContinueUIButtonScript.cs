using UnityEngine;
using System.Collections;
/**我要继续界面的领取按钮*/
public class WantContinueUIButtonScript : UGUIButtonControl {

    protected override void OnClick() {
        if (gameObject.name == "wantContinueUI_lingqu") {
           // GGameCaller.instance.Pay(GameData.GetWantContinueMoney()*100,null,null,PayResultHandler);
            PayCaller.instance.Pay(GameData.payCode[7], PayResultHandler);
        }
    }

    private void PayResultHandler(int result) {
        switch (result) {
            case 1://成功
                Game.instance.SetDiamondCount(Game.instance.diamondCount + GameData.GetWantContinueDiamondCount());
                
                //充值成功，关闭我要继续界面，重玩当前失败的关卡
                Object.Destroy(gameObject.transform.parent.gameObject);
                Game.instance.ContinueGame();
                break;
            case -1://失败

                break;
        }
    }
}
