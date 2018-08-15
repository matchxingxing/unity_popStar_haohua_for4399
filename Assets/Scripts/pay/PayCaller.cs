using UnityEngine;
using UnityEngine.UI;
public delegate void PayCallback(int result);
public class PayCaller: MonoBehaviour{

    private string payCode = "0001";//费用项代码
    private string result = "nothing";

    /**单例*/
    private static PayCaller _instance;
	public static PayCaller instance{
        get {
            if(_instance == null) {
				GameObject gameObj = new GameObject("PayCaller");
				gameObj.AddComponent<PayCaller>();
			}
			return _instance;
        }
    }

    void Awake(){
        if(_instance) return;
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private PayCallback _payResultHandler;
    public void Pay(string payCode, PayCallback payResultHandler) {
        if (Application.platform == RuntimePlatform.Android) {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("Pay", payCode);
            _payResultHandler = payResultHandler;
        }
    }


    public void PayCallbackHandler(string str) {
        result = str;
        int rt=str.Contains("success")? 1 : -1; //-1 充值失败， 1 充值成功
        if (_payResultHandler != null) {
            _payResultHandler(rt);
            _payResultHandler = null;
        }
    } 

    void OnGUI(){
        showText(result);
    }

    private void showText(string text){
        GameObject gObj = GameObject.Find("GText");
        Text txt = gObj.GetComponent<Text>();
        txt.text = text;
    }

    
}