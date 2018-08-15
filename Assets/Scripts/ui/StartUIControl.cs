using UnityEngine;
using System.Collections;

public class StartUIControl : MonoBehaviour {
    
    void Awake(){
        gameObject.name = "startUI";
    }

    public void HideNoviceBagButton(){
        BroadcastMessage("DestroyNoviceBagButton");
    }
    
}
