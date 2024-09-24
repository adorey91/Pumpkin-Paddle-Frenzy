using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    private float offset;
    private float damper = 0.004f;
    public float coef = 1;

    void LateUpdate()
    {
        offset += GameManager.instance.moveSpeed * damper * Time.deltaTime * (coef * 75);
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
}
