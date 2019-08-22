using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public enum GameMode
    {
        Deathmatch,
        KillCount
    }
    private GameMode gameMode;
    public Action<GameMode> OnGameModeChanged;
    private int lives = 3;
    public Action<int> OnLivesChanged;
    private int killCount = 5;
    public Action<int> OnKillCountChanged;
    private bool bots = true;
    public Action<bool> OnBotsChanged;
    private int botsCount = 4;
    public Action<int> OnBotCountChanged;

    [SerializeField] private Cinemachine.CinemachineTargetGroup targetGroup;
    [SerializeField] private List<CastleShip> availableCastleShips;
    [SerializeField] private List<CastleShip> availableAICastleShips;

    private int roundNumber;
    private List<GamePlayer> humanPlayers = new List<GamePlayer>();
    private GamePlayer currentKing = null;

    private Dictionary<CastleShip.CastleShipType, GameObject> shipDict;

    private Dictionary<CastleShip.CastleShipType, GameObject> ShipDict
    {
        get
        {
            if (shipDict == null)
            {
                shipDict = new Dictionary<CastleShip.CastleShipType, GameObject>();

                for(int i=0; i<availableCastleShips.Count; i++)
                {
                    if (!shipDict.ContainsKey(availableCastleShips[i].shipType))
                    {
                        shipDict.Add(availableCastleShips[i].shipType, availableCastleShips[i].gameObject);
                    }
                }
            }

            return shipDict;
        }
    }

    private Dictionary<CastleShip.CastleShipType, GameObject> aiShipDict;

    private Dictionary<CastleShip.CastleShipType, GameObject> AIShipDict
    {
        get
        {
            if (aiShipDict == null)
            {
                aiShipDict = new Dictionary<CastleShip.CastleShipType, GameObject>();

                for (int i = 0; i < availableAICastleShips.Count; i++)
                {
                    if (!aiShipDict.ContainsKey(availableAICastleShips[i].shipType))
                    {
                        aiShipDict.Add(availableAICastleShips[i].shipType, availableAICastleShips[i].gameObject);
                    }
                }
            }

            return aiShipDict;
        }
    }


    public CastleShip SpawnShip(CastleShip.CastleShipType shipType, int playerId, Transform spawnTransform, bool ai = false)
    {
        GameObject newShip = ai ? Instantiate(AIShipDict[shipType], spawnTransform) : Instantiate(ShipDict[shipType], spawnTransform);
        newShip.transform.position = spawnTransform.position;
        newShip.transform.rotation = spawnTransform.rotation;
        if (!ai) { newShip.GetComponent<PlayerControlled>().AssignPlayer(playerId); }
        targetGroup.AddMember(newShip.transform,1.0f,3.0f);
        return newShip.GetComponent<CastleShip>();
    }

    public CastleShip GetShip(CastleShip.CastleShipType shipType)
    {
        return ShipDict[shipType].GetComponent<CastleShip>();
    }

    public void IncreaseGameMode()
    {
        gameMode = gameMode.Next();
        OnGameModeChanged?.Invoke(gameMode);
    }
    public void DecreaseGameMode()
    {
        gameMode = gameMode.Previous();
        OnGameModeChanged?.Invoke(gameMode);
    }

    public void IncreaseLives()
    {
        lives++;
        OnLivesChanged?.Invoke(lives);
    }
    public void DecreaseLives()
    {
        lives--;
        OnLivesChanged?.Invoke(lives);
    }


    public void IncreaseKillCount()
    {
        killCount++;
        OnKillCountChanged?.Invoke(killCount);
    }
    public void DecreaseKillCount()
    {
        killCount--;
        OnKillCountChanged?.Invoke(killCount);
    }

    public void ToggleBots()
    {
        bots = !bots;
        OnBotsChanged?.Invoke(bots);
    }

    public void IncreaseBotCount()
    {
        botsCount++;
        OnBotCountChanged?.Invoke(botsCount);
    }
    public void DecreaseBotCount()
    {
        botsCount--;
        OnBotCountChanged?.Invoke(botsCount);
    }

    public void RunAllCallbacks()
    {
        OnGameModeChanged?.Invoke(gameMode);
        OnKillCountChanged?.Invoke(killCount);
        OnLivesChanged?.Invoke(lives);
        OnBotsChanged?.Invoke(bots);
        OnBotCountChanged?.Invoke(botsCount);
    }
}
