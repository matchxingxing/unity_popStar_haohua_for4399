using UnityEngine;
using System.Collections;
/**活动、领取，大礼包,神秘大奖，的领取按钮脚本 */
public class BagButtonScript : UGUIButtonControl {

    public override void Start() {
        base.Start();
    }

    protected override void OnClick() {
        if(gameObject.name=="activityBag_lingqu"||gameObject.name=="receiveBag_lingqu"){
            //活动礼包、领取礼包
           // GGameCaller.instance.Pay(GameData.GetActivityBagMoney()*100,null,null,PayResultHandler);
            PayCaller.instance.Pay(GameData.payCode[5], PayResultHandler);
        }else if(gameObject.name=="mysteryPrize_lingqu"){
            //神秘大奖
           // GGameCaller.instance.Pay(GameData.GetMsteryPrizeMoney()*100,null,null,PayResultMsteryPrize);
            PayCaller.instance.Pay(GameData.payCode[6], PayResultHandler);
        }
    }

    private void PayResultHandler(int result){
        switch (result) {
            case 1://成功
                Game.instance.SetDiamondCount(Game.instance.diamondCount+GameData.GetActivityBagDiamondCount());
                break;
            case -1://失败
                
                break;
        }
    }

    /**神秘大奖*/
    private void PayResultMsteryPrize(int result) {
        switch (result) {
            case 0://成功
                Game.instance.SetDiamondCount(Game.instance.diamondCount + GameData.GetMsteryPrizeDiamondCount());
                break;
            case -1://失败

                break;
        }
    }
 
}
