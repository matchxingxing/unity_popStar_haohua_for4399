using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/**商城要买的钻石的数量 文本  */
public class BuyTotalText : MonoBehaviour {
    public int id =-1;

    void Start() {
        Text txt = gameObject.GetComponent<Text>();
        txt.text = "x"+GameData.GetBuyTotal(id).ToString();
    }

    // Update is called once per frame
    void Update() {

    }
}
