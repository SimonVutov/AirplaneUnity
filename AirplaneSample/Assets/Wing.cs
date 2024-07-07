using UnityEngine;

[System.Serializable]
public class Wing
{
    public Vector3 degreesOfMotionRollPitchYaw;
    public GameObject Object;
    [HideInInspector]
    public Vector3 def; // defaultlocalEulerAngles
}