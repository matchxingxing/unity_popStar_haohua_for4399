using UnityEngine;
using System.Collections;

public class PropUIControl : MonoBehaviour {

    void Awake(){
        gameObject.name = "propUI";
    }

    public void SetEnabled(bool value){
        BroadcastMessage("ChangeEnabled",value);
    }
}
