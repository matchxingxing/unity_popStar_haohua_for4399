using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	private string payCode = "0004"; 
	private string result="";

    void Start(){
        byte n1 = 125;
        int n2 = 125;
        Debug.Log("test-------------");
        Debug.Log((n1>>2)&4);
        Debug.Log((n2>>2)&4);
    }


	// Update is called once per frame
	void Update ()
	{
		//当用户按下手机的返回键或home键退出游戏
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home) )
  		{
   			Application.Quit();
  		}
	}

	void OnGUI()
	{
		//绘制一个输入框接收用户输入 

		
		if(GUILayout.Button("wapy pay0001",GUILayout.Height(100)))
		{
			//注释1
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
      	 	AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        	jo.Call("Pay","0001","","","key:values;参数KEY:参数VALUES");
		}
		GUILayout.TextField ("wpay back resut:"+result, GUILayout.Width(500),GUILayout.Height(100)); 
	}
	void resultMessgae(string str)
     {
         result = str;
     }
}