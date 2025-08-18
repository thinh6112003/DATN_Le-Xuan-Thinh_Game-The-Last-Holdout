using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridUnit : MonoBehaviour
{
    public GameObject squareWall0;
    public GameObject squareWall1;
    public GameObject squareWall2;
    public GameObject squareWall4;
    public GameObject cell;

    public void Init()
    {
        squareWall0.SetActive(false);
        squareWall1.SetActive(false);
        squareWall2.SetActive(false);
        squareWall4.SetActive(false);
        cell.SetActive(true);
    }
    public void Init(int corner, direction direction = direction.none)
    {
        switch (corner)
        {
            case 0:
                squareWall0.SetActive(true);
                break;
            case 1:
                squareWall1.SetActive(true);
                break;
            case 2:
                squareWall2.SetActive(true);
                break;
            case 4:
                squareWall4.SetActive(true);
                break;
            default:
                Debug.LogError("Invalid corner value: " + corner);
                break;
        }
    }
}
public enum direction
{
    none = 0,
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4
}