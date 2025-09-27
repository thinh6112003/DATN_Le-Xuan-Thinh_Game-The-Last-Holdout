using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("ua ua ua"); 
            DataManager.Instance.gamePlayData.heart -= 1;
            GamePlayUI.Instance.UpdateUIInGame();
            if(DataManager.Instance.gamePlayData.heart== 0)
            {
                /// lose game handle action
            }
        }
    }
}
