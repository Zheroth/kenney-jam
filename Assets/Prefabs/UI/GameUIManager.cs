using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private GamePlayer[] humanPlayers;

    public void OnPlayerJoined(PlayerManager.PlayerArgs playerArgs)
    {
        for (int i = 0; i < humanPlayers.Length; i++)
        {
            GamePlayer selHumanPlayer = humanPlayers[i];
            if (!selHumanPlayer.HasPlayer)
            {
                selHumanPlayer.BindPlayer(playerArgs);
                return;
            }
        }
    }

    public void OnPlayerDismissed(PlayerManager.PlayerArgs playerArgs)
    {
        for (int i = 0; i < humanPlayers.Length; i++)
        {
            GamePlayer selHumanPlayer = humanPlayers[i];
            if (selHumanPlayer.HasPlayer && selHumanPlayer.BoundPlayerID == playerArgs.PlayerId)
            {
                selHumanPlayer.UnBindPlayer();
                return;
            }
        }
    }
}
