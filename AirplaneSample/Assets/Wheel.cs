using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Wheel
{
    public GameObject Object;
    public GameObject LocationObject;
    public float height = 2;
    public float wheelSize = 1;
    [HideInInspector]
    public float lastDistance;
}
