using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableKeyValue
{
    public GameObject PlayerObject;
    public int id;
    public int state;
    public int price;

    public SerializableKeyValue(GameObject k, int v1, int v2,int v3)
    {
        PlayerObject = k;
        id = v1;
        state = v2;
        price = v3;
    }
}