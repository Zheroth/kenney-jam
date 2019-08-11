using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public UnityEvent<int> OnPlayerJoined;
    public UnityEvent<int> OnPlayerDismissed;

    private HashSet<int> joinedPlayerIds = new HashSet<int>();

    private int[] playerIdArr;
    private int[] PlayerIdArr
    {
        get
        {
            if (playerIdArr == null)
            {
                playerIdArr = ReInput.players.GetPlayerIds();
            }

            return playerIdArr;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (joinedPlayerIds.Count < 3)
        {
            for (int i = 0; i < PlayerIdArr.Length; i++)
            {
                if (joinedPlayerIds.Contains(i))
                {
                    continue;
                }

                if (ReInput.players.GetPlayer(PlayerIdArr[i]).GetButtonDown("Start"))
                {
                    JoinPlayer(i);
                }
            }
        }
    }

    public void JoinPlayer(int playerId)
    {
        Debug.Log("Player "+playerId+" joined.");
        if (!joinedPlayerIds.Contains(playerId))
        {
            joinedPlayerIds.Add(playerId);
            OnPlayerJoined?.Invoke(playerId);
        }
    }

    public void DismissPlayer(int playerId)
    {
        Debug.Log("Player "+playerId+" dismissed.");
        if (joinedPlayerIds.Contains(playerId))
        {
            joinedPlayerIds.Remove(playerId);
            OnPlayerDismissed?.Invoke(playerId);
        }
    }
}
