using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera Camera;
    public Vector3 eulerAngleToCamera;
    private void Start()
    {
        Camera = Camera.main;
        eulerAngleToCamera = Camera.transform.rotation.eulerAngles;
    }
    private void Update()
    {
        transform.eulerAngles = -eulerAngleToCamera;
    }
}
