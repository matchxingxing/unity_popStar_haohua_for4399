using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UGUIButtonControl : MonoBehaviour {
    protected Button _btn;
    void Awake(){
        _btn = gameObject.GetComponent<Button>();
    }
    public virtual void Start() {
        _btn.onClick.AddListener(OnClick);
        if(gameObject.name == "btnNoviceBag"){
            //已经领取了新手礼包，则删除新手礼包按钮
            if(Game.instance.isReceiveNoviceBag)  DestroyNoviceBagButton();
        }
    }

    private void DestroyNoviceBagButton(){
        if (gameObject.name == "btnNoviceBag")
            Object.Destroy(gameObject);
    }

    protected virtual void OnClick(){
        SoundMan.instance.Play("按钮",1.0f,0.47f);
        
        bool isDestroyParent = true;
        
        Game game = GameObject.Find("Main Camera").GetComponent<Game>();
       //Debug.Log(gameObject.name);
        switch(gameObject.name){
            case "toTitle":
                game.ToTitle();
                break;
            case "start":
                game.NewGame();
                break;
            case "continue":
                game.ContinueGame();
                break;
            case "shop":
                game.ToShop();
                break;
			case "rank":
				isDestroyParent = false;
				game.ToRank();
				break;
            case "btnNoviceBag"://首页新手礼包按钮
                game.CreateNoviceBag();
                isDestroyParent=false;
                break;
            case "noviceBag_lingqu"://新手礼包领取
                Game.instance.AddDiamondCount(GameData.GetNoviceBagDiamondCount());
                //马上删除开始界面上的新手礼包按钮
                GameObject startUIObj = GameObject.Find("startUI");
                if(startUIObj!=null){
                    StartUIControl startUIControl= startUIObj.GetComponent<StartUIControl>();
                    startUIControl.HideNoviceBagButton();
                }
                //设置已经领取过新手礼包
                Game.instance.SetIsReceiveNoviceBag(true);
                //弹出活动大礼包
                //Game.instance.CreateActivityBag();
                break;
            case "help":
                game.ToHelp();
                isDestroyParent = false;
                break;
            case "helpUI_close":
                //
                break;
            case "mute":
                isDestroyParent = false;
                game.SetMute(!game.mute);
                Image image  = gameObject.GetComponent<Image>();
                string str = game.mute?"2":"1";
                Texture2D texture2d = (Texture2D)Resources.Load("Sprites/mute000"+str);
                image.sprite = Sprite.Create(texture2d,new Rect(0,0,51f,53f),new Vector2(0.5f,0.5f));
                break;
            case "btn_createReceiveBag"://创建领取大礼包按钮
                isDestroyParent = false;
                game.CreateReceiveBag();
                break;
			case "btn_browseAdGetDiamond"://弹看广告加钻石界面
				isDestroyParent = false;
				game.CreateBrowseAdGetDiamondUI();
				break;
            case "wantContinueUI_cancel"://我要继续界面的 "x"
                game.FailureBackToTitle();
                break;
            case "btn_failureUIcancel"://失败界面的 "x"
                game.FailureBackToTitle();
                break;
            case "failureUI_continue"://失败界面的 "继续游戏"
                game.CreateWantContinueUI();//创建我要继续界面
                break;
            case "pauseUI_back":
                game.SetPause(false);
                game.ClearGameContent();
                game.ToTitle();
                break;
            case "pauseUI_continue"://暂停界面的继续
                game.SetPause(false);
                break;
            case "msgUI_pause":
                isDestroyParent = false;
                game.CreatePauseUI();
                break;
        }
        if(isDestroyParent)Object.Destroy(gameObject.transform.parent.gameObject);
    }

    void OnDestroy(){
        _btn.onClick.RemoveListener(OnClick);
    }

}
