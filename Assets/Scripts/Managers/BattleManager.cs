using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private MainMenuUIManager setupMenu;
    [SerializeField]
    private EndMenuUIManager endMenu;

    [SerializeField]
    private Color[] playerUsableColours;
    private List<PlayerColor> colours;

    public enum GameState
    {
        Setup,
        Playing,
        Finished
    }
    private GameState gameState = GameState.Setup;

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

    public CastleBG castleBG;

    [SerializeField] private Cinemachine.CinemachineTargetGroup targetGroup;
    [SerializeField] private List<CastleShip> availableCastleShips;
    [SerializeField] private List<CastleShip> availableAICastleShips;

    private int roundNumber;
    [SerializeField]
    private List<GamePlayer> gamePlayers = new List<GamePlayer>();
    private GamePlayer currentKing = null;
    public GamePlayer CurrentKing
    {
        get
        {
            return currentKing;
        }
        set
        {
            if (currentKing != null)
            {
                currentKing.ChangeKingStatus(false);
            }
            currentKing = value;
            value.ChangeKingStatus(true);
            castleBG.SetColour(value.PlayerColour);
            if(gameMode == GameMode.KillCount && value.Kills >= killCount)
            {
                this.ChangeToEndMatch();
            }
        }
    }

    public bool MatchInProgress
    {
        get
        {
            if(this.gameState == GameState.Playing)
            {
                return true;
            }
            return false;
        }
    }

    public void OnPlayerJoined(PlayerManager.PlayerArgs playerArgs)
    {
        for (int i = 0; i < gamePlayers.Count; i++)
        {
            GamePlayer selGamePlayer = gamePlayers[i];
            if (!selGamePlayer.HasPlayer)
            {
                selGamePlayer.BindPlayer(playerArgs);
                return;
            }
            else if(selGamePlayer.isAI)
            {
                selGamePlayer.UnBindAI();
                selGamePlayer.BindPlayer(playerArgs);
                return;
            }
        }
    }

    public void OnPlayerDismissed(PlayerManager.PlayerArgs playerArgs)
    {
        for (int i = 0; i < gamePlayers.Count; i++)
        {
            GamePlayer selGamePlayer = gamePlayers[i];
            if (selGamePlayer.HasPlayer && selGamePlayer.BoundPlayerID == playerArgs.PlayerId)
            {
                selGamePlayer.UnBindPlayer();
                return;
            }
        }
    }

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

    private void Awake()
    {
        this.colours = new List<PlayerColor>();
        for (int i = 0; i < playerUsableColours.Length; i++)
        {
            colours.Add(new PlayerColor(playerUsableColours[i], true));
        }
    }

    private void Start()
    {
        ChangeToSetup();
    }

    public CastleShip SpawnShip(CastleShip.CastleShipType shipType, int playerId, Transform spawnTransform, bool ai = false)
    {
        GameObject newShip = ai ? Instantiate(AIShipDict[shipType], spawnTransform) : Instantiate(ShipDict[shipType], spawnTransform);
        newShip.transform.position = spawnTransform.position;
        newShip.transform.rotation = spawnTransform.rotation;
        if (!ai) { newShip.GetComponent<PlayerControlled>().AssignPlayer(playerId); }
        targetGroup.AddMember(newShip.transform,1.0f,3.0f);
        CastleShip castleShip = newShip.GetComponent<CastleShip>();
        return castleShip;
    }

    public CastleShip GetShip(CastleShip.CastleShipType shipType)
    {
        return ShipDict[shipType].GetComponent<CastleShip>();
    }

    public void OnShipKilled(GamePlayer killer)
    {
        if(currentKing == null)
        {
            CurrentKing = killer;
        }
        else
        {
            if(killer.Kills >= currentKing.Kills)
            {
                CurrentKing = killer;
            }
        }
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
        if (lives > 20)
        {
            lives = 1;
        }
        OnLivesChanged?.Invoke(lives);
    }
    public void DecreaseLives()
    {
        lives--;
        if(lives < 1)
        {
            lives = 20;
        }
        OnLivesChanged?.Invoke(lives);
    }


    public void IncreaseKillCount()
    {
        killCount++;
        if(killCount > 20)
        {
            killCount = 1;
        }
        OnKillCountChanged?.Invoke(killCount);
    }
    public void DecreaseKillCount()
    {
        killCount--;
        if (killCount < 0)
        {
            killCount = 20;
        }
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
        if(botsCount > 4)
        {
            botsCount = 1;
        }
        OnBotCountChanged?.Invoke(botsCount);
    }
    public void DecreaseBotCount()
    {
        botsCount--;
        if (botsCount < 1)
        {
            botsCount = 4;
        }
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

    public void ChangeToSetup()
    {
        endMenu.Close();
        setupMenu.Open();

        for (int i = 0; i < colours.Count; i++)
        {
            this.colours[i].available = true;
        }
        for (int i = 0; i < gamePlayers.Count; i++)
        {
            GamePlayer selGamePlayer = gamePlayers[i];
            selGamePlayer.ChangeToUnassigned();
        }

        PlayerManager.Instance.ResetPlayerManager();
    }

    public void ChangeToStartMatch()
    {
        endMenu.Close();
        setupMenu.Close();
        if (bots)
        {
            int filledWithBots = 0;
            for (int i = 0; i < gamePlayers.Count && filledWithBots < botsCount; i++)
            {
                GamePlayer selGamePlayer = gamePlayers[i];
                if(!selGamePlayer.HasPlayer)
                {
                    selGamePlayer.BindAI();
                    filledWithBots++;
                }
            }
        }

        for (int i = 0; i < gamePlayers.Count; i++)
        {
            GamePlayer selGamePlayer = gamePlayers[i];
            if(selGamePlayer.HasPlayer)
            {
                if (selGamePlayer.isAI)
                {
                    selGamePlayer.SetColour(this.GetFirstAvailableColour());
                }
                selGamePlayer.ChangeToShipSelection();
                selGamePlayer.onAddKill += this.OnShipKilled;
            }
        }
        this.gameState = GameState.Playing;
    }

    public void ChangeToEndMatch()
    {
        setupMenu.Close();
        endMenu.Open();
        for (int i = 0; i < gamePlayers.Count; i++)
        {
            gamePlayers[i].ChangeToEndOfBattle();
        }

        endMenu.Populate(CurrentKing, gamePlayers);
        this.gameState = GameState.Finished;
    }

    public int GetNextColor(int index, int direction)
    {
        colours[index].available = true;
        int offset = index;
        for (int i = 0; i < colours.Count; i++)
        {
            offset += direction;
            if (offset >= colours.Count)
            {
                offset = 0;
            }
            if (offset < 0)
            {
                offset = colours.Count - 1;
            }
            if (colours[offset].available)
            {
                colours[offset].available = false;
                return offset;
            }
        }
        colours[index].available = false;
        return index;
    }

    public int GetFirstAvailableColour()
    {
        for (int i = 0; i < colours.Count; i++)
        {
            if(colours[i].available)
            {
                colours[i].available = false;
                return i;
            }
        }
        return -1;
    }

    public Color GetColour(int index)
    {
        return colours[index].color;
    }
}

[Serializable]
class PlayerColor
{
    public Color color = Color.white;
    public bool available = true;

    public PlayerColor(Color color, bool available)
    {
        this.color = color;
        this.available = available;
    }
}