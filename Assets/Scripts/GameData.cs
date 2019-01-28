using UnityEngine;

public class GameData
{
	public const int stageW = 480;
	public const int stageH = 800;
	public const bool isClearData = false;
	public const string version = "cn";

	public static int getTargetScore (int level)
	{
		int targetScore;
        if(level>=19){
            targetScore = 47000+4000*(level-18);
        }else if(level>=7 && level<=18){
            targetScore = 11000+3000*(level-6);
        }else{
            targetScore = 1000+2000*(level-1);
        }
        return targetScore;
		/*int [] list = new int [36];
		list [0] = 1000;
		list [1] = 2500;
		list [2] = 4500;
		list [3] = 6500;
		list [4] = 8520;
		list [5] = 10560;
		list [6] = 12620;
		list [7] = 14700;
		list [8] = 16800;
		list [9] = 18920;
		list [10] = 21060;
		list [11] = 23220;
		list [12] = 25400;
		list [13] = 27600;
		list [14] = 29820;
		list [15] = 32060;
		list [16] = 34320;
		list [17] = 36600;
		list [18] = 38900;
		list [19] = 41220;
		list [20] = 43560;
		list [21] = 45920;
		list [22] = 48300;
		list [23] = 50700;
		list [24] = 53120;
		list [25] = 55560;
		list [26] = 58020;
		list [27] = 60500;
		list [28] = 63000;
		list [29] = 65520;
		list [30] = 68060;
		list [31] = 70620;
		list [32] = 73200;
		list [33] = 75800;
		list [34] = 78420;
		list [35] = 81060;
		return list [level - 1];*/
	}

	private static int[,] _shopData = 
    {
	//买的数量， 赠送数量， 需要的钱(元)
        {10,  7,  2},
        {20, 18,  4},
        {30, 28,  6},
        {50, 40,  8},
        {80, 56, 10}
    };

	public static int GetBuyTotal (int id)
	{
		return _shopData [id, 0];
	}

	public static int GetGiveTotal (int id)
	{
		return _shopData [id, 1];
	}

	public static int GetMoney (int id)
	{
		return _shopData [id, 2];
	}

	/**新手大礼包钻石数量*/
	public static int GetNoviceBagDiamondCount ()
	{
		return 5;
	}





	/**活动大礼包和领取大礼包 钻石数量*/
	public static int GetActivityBagDiamondCount ()
	{
		return 100 + 120;
	}
	/**活动大礼包和领取大礼包 需要充值的钱(元)*/
	public static int GetActivityBagMoney ()
	{
		return 12;
	}


    /**神秘大奖*/
    public static int GetMsteryPrizeMoney(){
        return 8;
    }
    public static int GetMsteryPrizeDiamondCount(){
        return 50+40;
    }



	/**我要继续界面 钻石数量*/
	public static int GetWantContinueDiamondCount ()
	{
		return 60 + 50;
	}
	/**我要继续界面 需要充值的钱(元)*/
	public static int GetWantContinueMoney ()
	{
		return 6;
	}

    public static string[] payCode = new string[]{
        "0001",//商城按钮0
        "0002",//商城按钮1
        "0003",//商城按钮2
        "0004",//商城按钮3
        "0005",//商城按钮4

        "0006",//大礼包
        "0007",//神秘大奖
        "0008"//游戏结束充值
    };

    


}