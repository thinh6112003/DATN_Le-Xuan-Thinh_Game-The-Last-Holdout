using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacement : MonoBehaviour
{
    public BaseTower choseTower;
    public LayerMask slotLayer;
    public BaseTower towerCung;
    public BaseTower towerBoBinh;
    public BaseTower towerPhao;
    public BaseTower towerPhep;
    public bool isChoose;
    public GameObject chooseCanvas;
    // Update is called once per frame
    private void Start()
    {
        isChoose = false;
    }
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("btn dowawn ");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f, slotLayer))
            {
                Debug.Log("TRUNG TARGET");
                if (!isChoose)
                {
                    isChoose=true;
                    chooseCanvas.SetActive(true);
                    transform.position = hit.transform.position;
                }
                else
                {

                }
            }
            else
            {
                if (isChoose)
                {
                    chooseCanvas.SetActive(false);
                    isChoose = false;
                }
                else
                {

                }
            }
        }
    }
    public void ChooseTower(int towerType)
    {
        BaseTower newTower = null;
        switch((TowerType) towerType)
        {
            case TowerType.cung:
                newTower = towerCung;
                break;
            case TowerType.boBinh:
                newTower = towerBoBinh;
                break;
            case TowerType.phep:
                newTower = towerPhep;
                break;
            case TowerType.phao:
                newTower = towerPhao;
                break;
        }
        Instantiate(newTower, transform.position, Quaternion.identity);
        isChoose = false;
        chooseCanvas.SetActive(false);
    }
}
public enum TowerType
{
    cung, 
    boBinh,
    phep, 
    phao
}