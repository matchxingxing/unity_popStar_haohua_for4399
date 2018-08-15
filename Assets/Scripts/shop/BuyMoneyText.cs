using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/**商城购买钻石需要的金钱 文本  */
public class BuyMoneyText : MonoBehaviour {
    public int id =-1;

    void Start() {
        Text txt = gameObject.GetComponent<Text>();
        txt.text = GameData.GetMoney(id)+".00";
    }

    // Update is called once per frame
    void Update() {

    }
}
