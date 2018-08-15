using UnityEngine;
using System.Collections;

public class PauseUIScript : MonoBehaviour {
    private bool _recordMute;

    void Awake(){
        Debug.Log("Awake");
        Time.timeScale = 0;
        _recordMute = Game.instance.mute;
        Game.instance.SetMute(true);

    }

    void OnDestroy(){
        Time.timeScale = 1;
        Game.instance.SetMute(_recordMute);
    }
}
