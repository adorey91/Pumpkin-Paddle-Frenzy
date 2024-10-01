using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    private float offset;
    private float damper = 0.004f;

    void LateUpdate()
    {
        offset += GameManager.instance.moveSpeed * damper * Time.deltaTime * (GameManager.instance.moveSpeed * 75);
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
}
