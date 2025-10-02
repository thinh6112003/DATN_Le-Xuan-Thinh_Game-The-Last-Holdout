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
            
            // Thông báo enemy ð? ði qua gate (c?ng tính là bi?n m?t)
            GamePlayUI.Instance.EnemyDestroyed();
            
            if(DataManager.Instance.gamePlayData.heart== 0)
            {
                
                GameStateManager.Instance.ChangeStateAfter(1.5f,GameState.Lose);
            }
        }
    }
}
