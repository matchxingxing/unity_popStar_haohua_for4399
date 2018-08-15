using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/**拥有的钻石数量*/
public class HasDiamondText : MonoBehaviour {
    private Text _txt;
    void Start() {
        _txt = gameObject.GetComponent<Text>();
    }

    void Update() {
        
    }

    void OnGUI(){
        if (Game.instance.diamondCount >= 1e4)
            _txt.text = (Game.instance.diamondCount * 0.0001f).ToString("0.00") + "万";
        else
            _txt.text = Game.instance.diamondCount.ToString();
        //_txt.text = Game.instance.diamondCount.ToString();
    }
}
