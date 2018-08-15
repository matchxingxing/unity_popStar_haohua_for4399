using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/**商城要赠送钻石的数量 文本  */
public class GiveTotalText : MonoBehaviour {
    public int id =-1;

    void Start() {
        Text txt = gameObject.GetComponent<Text>();
        txt.text = "x"+GameData.GetGiveTotal(id).ToString();
    }

    // Update is called once per frame
    void Update() {

    }
}
