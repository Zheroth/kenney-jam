using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Utils;
using Rewired;

public class GameManager : MonoSingleton<GameManager>
{
    public Player[] playerArr = new Player[4];

    public CastleShip castleShip;
    public CastleShip castleShip2;

    void Start()
    {
        castleShip.DamageableRef.OnDeath.AddListener(OnPlayerDeath);
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            ReInput.players.GetPlayers();
        }
    }

    public void SpawnPlayer(int playerId)
    {
        Debug.Log("Spawn player: "+playerId);

    }

    private void OnPlayerDeath()
    {
        Debug.Log("Player Died");
    }
}
