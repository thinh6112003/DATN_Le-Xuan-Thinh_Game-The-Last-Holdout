using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuidingSlot : MonoBehaviour
{
    public bool isBuilded;
    public GameObject modelSlot;
    public Transform tf1;
    public void Start()
    {
        isBuilded = false;
    }
}
