using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    [Header("Tower Selection")]
    public BaseTower choseTower;
    public BaseTower towerCung;
    public BaseTower towerBoBinh;
    public BaseTower towerPhao;
    public BaseTower towerPhep;
    
    [Header("Raycast Layers")]
    public LayerMask slotLayer;
    public LayerMask towerLayer;
    
    [Header("State Management")]
    public bool isChoose;
    public bool isUpdate;
    public BuidingSlot choosedBuildingSlot;
    public BaseTower chooseTower;
    
    [Header("UI Canvas")]
    public GameObject chooseCanvas;
    public GameObject updateCanvas;
    
    [Header("Tower Prices")]
    public int priceOfCung;
    public int priceOfBoBinh;
    public int priceOfPhao;
    public int priceOfPhep;
    
    [Header("UI Colors")]
    public Color nomalPriceColor;
    
    [Header("Active Tower Images")]
    public GameObject activeCungimg;
    public GameObject activeBoBinhimg;
    public GameObject activePhaoimg;
    public GameObject activePhepimg;
    
    [Header("Inactive Tower Images")]
    public GameObject deActiveCungimg;
    public GameObject deActiveBoBinhimg;
    public GameObject deActivePhaoimg;
    public GameObject deActivePhepimg;
    
    [Header("Price Text UI")]
    public TextMeshProUGUI textPriceCung;
    public TextMeshProUGUI textPriceBoBinh;
    public TextMeshProUGUI textPricePhao;
    public TextMeshProUGUI textPricePhep;
    
    [Header("Active Checkmark Images")]
    public GameObject tichXanhCungImg;
    public GameObject tichXanhBoBinhimg;
    public GameObject tichXanhPhaoimg;
    public GameObject tichXanhPhepimg;
    
    [Header("Inactive Checkmark Images")]
    public GameObject TichXanhGreyCungImg;
    public GameObject TichXanhGreyBoBinhimg;
    public GameObject TichXanhGreyPhaoimg;
    public GameObject TichXanhGreyPhepimg;


    [Header("Button Status")]
    public bool statusOfCung;
    public bool statusOfBoBinh;
    public bool statusOfPhao;
    public bool statusOfPhep;

    [Header("Choose Status For New Tower")]
    public bool isChooseCung= false;
    public bool isChooseBoBinh= false;
    public bool isChoosePhao = false;
    public bool isChoosePhep = false;

    [Header("Tich Xanh Tower")]

    public GameObject tichXanhCung;
    public GameObject tichXanhBoBinh;
    public GameObject tichXanhPhao;
    public GameObject tichXanhPhep;


    [Header("Element Of Update")]
    public TextMeshProUGUI textPriceUpdate;
    public GameObject activeUpdateImg;
    public GameObject deActiveUpdateImg;
    public GameObject tichXanhUpdateImg;
    public GameObject tichXanhGrayUpdateImg;
    public GameObject tichXanhUpdate;
    public GameObject tichXanhSell;
    public bool statusofUpdate;
    public bool isChooseUpdate;
    public bool isChooseSell;

    private void Start()
    {
        isChoose = false;
        UpdateStatusOfButton();
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
                    if (!isChoose)
                    {
                        isChoose = true;
                        chooseCanvas.SetActive(true);
                        InitChooseCanvas();
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
                    InitChooseCanvas();
                    UpdateStatusOfButton();
                }
                else
                {

                }
            }
            else
            {
                if (isUpdate)
                {
                    updateCanvas.SetActive(false);
                    isUpdate = false;
                }
                else
                {

                }
            }

        }
    }
    public void InitChooseCanvas()
    {
        textPriceCung.text = priceOfCung.ToString();
        textPriceBoBinh.text = priceOfBoBinh.ToString();
        textPricePhao.text = priceOfPhao.ToString();
        textPricePhep.text = priceOfPhep.ToString();
        textPriceUpdate.text = chooseTower != null ? chooseTower.GetPriceUpdate().ToString() : "0";
        isChooseCung = false;
        isChooseBoBinh = false;
        isChoosePhao = false;
        isChoosePhep = false;
        isChooseUpdate = false;
        isChooseSell = false;
        tichXanhCung.SetActive(false);
        tichXanhBoBinh.SetActive(false);
        tichXanhPhao.SetActive(false);
        tichXanhPhep.SetActive(false);
        tichXanhUpdate.SetActive(false);
        tichXanhSell.SetActive(false);
    }
    public void UpdateStatusOfButton()
    {
        GamePlayData gamePlayData = DataManager.Instance.gamePlayData;
        if (gamePlayData.coin >= priceOfCung)
            SetStatusOfButton(TowerType.Archer, true);
        else 
            SetStatusOfButton(TowerType.Archer, false);
        
        if (gamePlayData.coin >= priceOfBoBinh)
            SetStatusOfButton(TowerType.Barracks, true);
        else
            SetStatusOfButton(TowerType.Barracks, false);
        
        if (gamePlayData.coin >= priceOfPhao)
            SetStatusOfButton(TowerType.Cannon, true);
        else
            SetStatusOfButton(TowerType.Cannon, false);
        
        if (gamePlayData.coin >= priceOfPhep)
            SetStatusOfButton(TowerType.Mage, true);
        else
            SetStatusOfButton(TowerType.Mage, false);
        if(chooseTower != null)
        {
            if(gamePlayData.coin >= chooseTower.GetPriceUpdate())
            {
                SetStatusOfUpdate(true);
            }
            else
            {
                SetStatusOfUpdate(false);
            }
        }
    }
    public void SetStatusOfUpdate(bool isActive)
    {
        activeUpdateImg.SetActive(isActive);
        deActiveUpdateImg.SetActive(!isActive);
        SetGrayText(textPriceUpdate, !isActive);
        tichXanhUpdateImg.SetActive(isActive);
        tichXanhGrayUpdateImg.SetActive(!isActive);
        statusofUpdate = isActive;
    }
    public void SetStatusOfButton(TowerType towerType,bool isActive)
    {
        switch (towerType)
        {
            case TowerType.Archer:
                activeCungimg.SetActive(isActive);
                deActiveCungimg.SetActive(!isActive);
                SetGrayText(textPriceCung, !isActive);
                tichXanhCungImg.SetActive(isActive);
                TichXanhGreyCungImg.SetActive(!isActive);
                statusOfCung = isActive;
                break;
            case TowerType.Barracks:
                activeBoBinhimg.SetActive(isActive);
                deActiveBoBinhimg.SetActive(!isActive);
                SetGrayText(textPriceBoBinh, !isActive);
                tichXanhBoBinhimg.SetActive(isActive);
                TichXanhGreyBoBinhimg.SetActive(!isActive);
                statusOfBoBinh = isActive;
                break;
            case TowerType.Cannon:
                activePhaoimg.SetActive(isActive);
                deActivePhaoimg.SetActive(!isActive);
                SetGrayText(textPricePhao, !isActive);
                tichXanhPhaoimg.SetActive(isActive);
                TichXanhGreyPhaoimg.SetActive(!isActive);
                statusOfPhao = isActive;
                break;
            case TowerType.Mage:
                activePhepimg.SetActive(isActive);
                deActivePhepimg.SetActive(!isActive);
                SetGrayText(textPricePhep, !isActive);
                tichXanhPhepimg.SetActive(isActive);
                TichXanhGreyPhepimg.SetActive(!isActive);
                statusOfPhep = isActive;
                break;
        }
    }
    
    public void SetGrayText(TextMeshProUGUI text, bool isGrey)
    {
        if (isGrey)
        {
            text.color = Color.gray;
        }
        else
        {
            text.color = nomalPriceColor;
        }
    }
    
    public void ChooseTower(int towerType)
    {
        BaseTower newTower = null;
        switch ((TowerType) towerType)
        {
            case TowerType.Archer:
                newTower = towerCung;
                if (!isChooseCung)
                {
                    isChooseCung = true;
                    tichXanhCung.SetActive(true);
                    tichXanhBoBinh.SetActive(false);
                    tichXanhPhao.SetActive(false);
                    tichXanhPhep.SetActive(false);
                    isChooseBoBinh = false;
                    isChoosePhao = false;
                    isChoosePhep = false;
                    return;
                }
                if (!statusOfCung) return;
                DataManager.Instance.gamePlayData.coin -= priceOfCung;
                break;
            case TowerType.Barracks:
                newTower = towerBoBinh;
                if (!isChooseBoBinh)
                {
                    isChooseBoBinh = true;
                    tichXanhBoBinh.SetActive(true);
                    tichXanhCung.SetActive(false);
                    tichXanhPhao.SetActive(false);
                    tichXanhPhep.SetActive(false);
                    isChooseCung = false;
                    isChoosePhao = false;
                    isChoosePhep = false;
                    return;
                }
                if (!statusOfBoBinh) return;
                DataManager.Instance.gamePlayData.coin -= priceOfBoBinh;
                break;
            case TowerType.Mage:
                newTower = towerPhep;
                if (!isChoosePhep)
                {
                    isChoosePhep = true;
                    tichXanhPhep.SetActive(true);
                    tichXanhCung.SetActive(false);
                    tichXanhBoBinh.SetActive(false);
                    tichXanhPhao.SetActive(false);
                    isChooseCung = false;
                    isChooseBoBinh = false;
                    isChoosePhao = false;
                    return;
                }
                if (!statusOfPhep) return;
                DataManager.Instance.gamePlayData.coin -= priceOfPhep;
                break;
            case TowerType.Cannon:
                newTower = towerPhao;
                if (!isChoosePhao)
                {
                    isChoosePhao = true;
                    tichXanhPhao.SetActive(true);
                    tichXanhCung.SetActive(false);
                    tichXanhBoBinh.SetActive(false);
                    tichXanhPhep.SetActive(false);
                    isChooseCung = false;
                    isChooseBoBinh = false;
                    isChoosePhep = false;

                    return;
                }
                if(!statusOfPhao) return;
                DataManager.Instance.gamePlayData.coin -= priceOfPhao;
                break;
        }
        isChoose = false;
        chooseCanvas.SetActive(false);
        choosedBuildingSlot.isBuilded = true;
        choseTower =  Instantiate(newTower, transform.position, Quaternion.identity);
        choseTower.myBuildingSlot = choosedBuildingSlot;
        UpdateStatusOfButton();
        GamePlayUI.Instance.UpdateUIInGame();
    }
    
    public void ChooseSellUpdate(bool isUpdate)
    {
        if (isUpdate)
        {
            if (!isChooseUpdate)
            {
                isChooseUpdate = true;
                tichXanhUpdate.SetActive(true);
                tichXanhSell.SetActive(false);
                isChooseSell = false;
                return;
            }
            if (!statusofUpdate) return;
            DataManager.Instance.gamePlayData.coin -= chooseTower.GetPriceUpdate();
            GamePlayUI.Instance.UpdateUIInGame() ;
            chooseTower.UpdateLevel();
            UpdateStatusOfButton();
        }
        else
        {
            if (!isChooseSell)
            {
                isChooseSell = true;
                tichXanhSell.SetActive(true);
                tichXanhUpdate.SetActive(false);
                isChooseUpdate = false;
                return;
            }
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