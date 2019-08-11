using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private PlayerUIManager[] playerUIManager;

    public void OnPlayerJoined(PlayerManager.PlayerArgs playerArgs)
    {
        //for (int i = 0; i < playerUIManager.Length; i++)
        //{
        //    PlayerUIManager selectedPlayerUIManager = playerUIManager[i];
        //    if (selectedPlayerUIManager.HasPlayer  selectedPlayerUIManager.BoundPlayerID == playerArgs.PlayerId)
        //    {
        //        return;
        //    }
        //}

        for (int i = 0; i < playerUIManager.Length; i++)
        {
            PlayerUIManager selectedPlayerUIManager = playerUIManager[i];
            if (!selectedPlayerUIManager.HasPlayer)
            {
                selectedPlayerUIManager.BindPlayer(playerArgs);
                return;
            }
        }
    }

    public void OnPlayerDismissed(PlayerManager.PlayerArgs playerArgs)
    {
        for (int i = 0; i < playerUIManager.Length; i++)
        {
            PlayerUIManager selectedPlayerUIManager = playerUIManager[i];
            if (selectedPlayerUIManager.HasPlayer && selectedPlayerUIManager.BoundPlayerID == playerArgs.PlayerId)
            {
                selectedPlayerUIManager.UnbindPlayer();
                return;
            }
        }
    }
}
