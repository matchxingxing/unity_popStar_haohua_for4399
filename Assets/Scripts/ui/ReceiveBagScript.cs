using UnityEngine;
using System.Collections;

public class ReceiveBagScript : MonoBehaviour {
    private bool _destroyIsContinueGame=false;

    void Start() {
        Game.instance.SetIsBanClickStar(true);
    }

    public void SetDestroyIsContinueGame(bool value){
        _destroyIsContinueGame=value;
    }

    void OnDestroy(){
        if(_destroyIsContinueGame){
            if(Game.instance.level>=36){
                Game.instance.ClearGameContent();
                Game.instance.ToTitle();
            }else{
                Game.instance.ContinueGame();
            }
        }
        Game.instance.SetIsBanClickStar(false);
    }

}
