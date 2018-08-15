using UnityEngine;
using System.Collections;

public class BuyButtonControl :UGUIButtonControl {
    public int id=-1;
    private int _diamondTotal;//钻石数量
    private int _money;//花费的钱(元)

    override public void Start(){
        _diamondTotal = GameData.GetBuyTotal(id)+GameData.GetGiveTotal(id);//钻石总数=买的数量+赠送的数量
        _money = GameData.GetMoney(id);
        base.Start();
    }

    override protected void OnClick(){
       // GGameCaller.instance.Pay(_money*100,null,null,PayResultHandler);
        PayCaller.instance.Pay(GameData.payCode[id], PayResultHandler);
        //Debug.Log("点购买钻石: 钻石总数"+_diamondTotal+" 金钱"+_money);
        
    }

    private void PayResultHandler(int result){
        switch(result){
            case 1://成功
                Game.instance.SetDiamondCount(Game.instance.diamondCount+_diamondTotal);
                break;
            case -1://失败
                
                break;
        }
    }
}
