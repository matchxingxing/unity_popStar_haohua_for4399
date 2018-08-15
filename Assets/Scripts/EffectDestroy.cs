using UnityEngine;
using System.Collections;

public class EffectDestroy : MonoBehaviour {

    public void Dispose() {
        Object.Destroy(transform.root.gameObject);
    }
}
