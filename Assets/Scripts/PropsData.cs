using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PropsData
{
    public string scene;
    public List<bool> doorList;

    public PropsData()
    {
        doorList = new List<bool>();
    }
}
