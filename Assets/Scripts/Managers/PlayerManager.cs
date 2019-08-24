using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Utils;
using Rewired;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public PlayerEvent OnPlayerJoined;
    public PlayerEvent OnPlayerDismissed;

    public Player MainPlayer
    {
        get
        {
            if(joinedPlayerIds.Count > 0)
            {
                return ReInput.players.GetPlayer(joinedPlayerIds[0]);
            }
            return null;
        }
    }

    private List<int> joinedPlayerIds = new List<int>();

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
                    return;
                }
            }
        }
    }

    public void JoinPlayer(int playerId)
    {
        Debug.Log("Player "+playerId+" joined.");
        if (!joinedPlayerIds.Contains(playerId))
        {
            Debug.Log("Player " + playerId + " joined for the first time.");
            joinedPlayerIds.Add(playerId);
            OnPlayerJoined?.Invoke(new PlayerArgs(playerId));
        }
    }

    public void DismissPlayer(int playerId)
    {
        Debug.Log("Player "+playerId+" dismissed.");
        if (joinedPlayerIds.Contains(playerId))
        {
            joinedPlayerIds.Remove(playerId);
            OnPlayerDismissed?.Invoke(new PlayerArgs(playerId));
        }
    }

    [Serializable]
    public class PlayerArgs : EventArgs
    {
        public readonly int PlayerId;

        public PlayerArgs(int newPlayerId)
        {
            PlayerId = newPlayerId;
        }
    }

    [Serializable]
    public class PlayerEvent : UnityEvent<PlayerArgs> {}
}
