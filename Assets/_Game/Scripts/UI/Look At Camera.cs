using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera Camera;
    public Vector3 eulerAngleToCamera;
    public bool isNguoc = false;
    private void Start()
    {
        if (Camera == null)
        {
            Camera = Camera.main;
        }
        eulerAngleToCamera = Camera.transform.rotation.eulerAngles;
    }
    private void OnEnable()
    {
        if (Camera == null)
        {
            Camera = Camera.main;
        }
        eulerAngleToCamera = Camera.transform.rotation.eulerAngles;
    }
    private void Update()
    {
        if(isNguoc)
            transform.eulerAngles = eulerAngleToCamera;
        else
            transform.eulerAngles = -eulerAngleToCamera;
    }
}
