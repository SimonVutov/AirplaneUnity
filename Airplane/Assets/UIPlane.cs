using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlane : MonoBehaviour
{
    public Text txt;
    void Update()
    {
        txt.text = (GetComponent<Rigidbody>().velocity.magnitude * 3.6f).ToString("0") + " km/h";
    }
}
