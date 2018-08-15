using UnityEngine;
using System.Collections;

public class SoundLoopMan : MonoBehaviour {
    
    private static SoundLoopMan _instance;
    public static SoundLoopMan instance{
        get{
            if(_instance==null){
                GameObject gObj= new GameObject("SoundLoopMan");
                gObj.AddComponent<AudioSource>();
                gObj.AddComponent<SoundLoopMan>();
            }
            return _instance;
        }
    }

    void Awake(){
        if(_instance)return;
        _instance = this;
        Object.DontDestroyOnLoad(gameObject);
        //
        gameObject.transform.position = new Vector3(0,0,-10);
    }

    public void Play(string name,float volume=1.0f,float time=0f){
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/"+name);
        GetComponent<AudioSource>().loop=true;
        GetComponent<AudioSource>().volume = volume;
        GetComponent<AudioSource>().time = time;
        GetComponent<AudioSource>().Play();
    }

    public void PlayOneShot(string name,float volumeScale=1.0f){
        GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/"+name),volumeScale);
    }
}
