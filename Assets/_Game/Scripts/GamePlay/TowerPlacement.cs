using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacement : MonoBehaviour
{
    public BaseTower choseTower;
    public LayerMask slotLayer;
    public LayerMask towerLayer;
    public BaseTower towerCung;
    public BaseTower towerBoBinh;
    public BaseTower towerPhao;
    public BaseTower towerPhep;
    public bool isChoose;
    public bool isUpdate;
    public GameObject chooseCanvas;
    public GameObject updateCanvas;
    public BuidingSlot choosedBuildingSlot;
    public BaseTower chooseTower; 
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
                BuidingSlot buidingSlot = hit.transform.parent.GetComponent<BuidingSlot>();
                if (!buidingSlot.isBuilded)
                {
                    buidingSlot.isBuilded = true;
                    if (!isChoose)
                    {
                        isChoose = true;
                        chooseCanvas.SetActive(true);
                        transform.position = hit.transform.position;
                        choosedBuildingSlot = buidingSlot;
                    }
                    else
                    {

                    }
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
            Debug.Log("da qua check slot");
            if (Physics.Raycast(ray, out hit, 100f, towerLayer))
            {
                Debug.Log(hit.transform.gameObject.name);
                BaseTower baseTower = hit.transform.GetComponent<BaseTower>();
                Debug.Log("Trung Tower");
                if (!isUpdate)
                {
                    isUpdate = true;
                    updateCanvas.SetActive(true);
                    transform.position = hit.transform.position;
                    chooseTower = baseTower;
                }
                else
                {

                }
            }
            else
            {
                if (isChoose)
                {
                    updateCanvas.SetActive(false);
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
            case TowerType.Archer:
                newTower = towerCung;
                break;
            case TowerType.Barracks:
                newTower = towerBoBinh;
                break;
            case TowerType.Mage:
                newTower = towerPhep;
                break;
            case TowerType.Cannon:
                newTower = towerPhao;
                break;
        }
        Instantiate(newTower, transform.position, Quaternion.identity);
        newTower.myBuildingSlot = choosedBuildingSlot;
        chooseCanvas.SetActive(false);
    }
    public void ChooseSellUpdate(bool isUpdate)
    {
        if (isUpdate)
        {
            chooseTower.UpdateLevel();
        }
        else
        {
            SellTower();
        }
        updateCanvas.SetActive(false);
        this.isUpdate = false;
    }
    public void SellTower()
    {
        chooseTower.myBuildingSlot.isBuilded = false;
        Destroy(chooseTower.gameObject);
    }
}
public enum TowerType
{
    Archer, 
    Barracks,
    Mage, 
    Cannon
}