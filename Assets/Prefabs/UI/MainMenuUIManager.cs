using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField]
    private BattleManager battleManager;
    [SerializeField]
    private TMPro.TextMeshProUGUI gameModeText;
    [SerializeField]
    private TMPro.TextMeshProUGUI livesText;
    [SerializeField]
    private TMPro.TextMeshProUGUI killCountText;
    [SerializeField]
    private TMPro.TextMeshProUGUI botsText;
    [SerializeField]
    private TMPro.TextMeshProUGUI botsCountText;

    [SerializeField]
    private GameObject lives;
    [SerializeField]
    private GameObject killCount;

    private void Start()
    {
        battleManager.OnGameModeChanged += OnGameModeChanged;
        battleManager.OnLivesChanged += OnLivesChanged;
        battleManager.OnKillCountChanged += OnKillCountChanged;
        battleManager.OnBotsChanged += OnBotsChanged;
        battleManager.OnBotCountChanged += OnBotCountChanged;

        battleManager.RunAllCallbacks();
    }

    #region Callbacks
    private void OnGameModeChanged(BattleManager.GameMode gameMode)
    {
        this.gameModeText.text = gameMode.ToString();
        if(gameMode == BattleManager.GameMode.Deathmatch)
        {
            lives.SetActive(true);
            killCount.SetActive(false);
        }
        else if(gameMode == BattleManager.GameMode.KillCount)
        {
            lives.SetActive(false);
            killCount.SetActive(true);
        }
    }

    private void OnLivesChanged(int lives)
    {
        this.livesText.text = lives.ToString();
    }

    private void OnKillCountChanged(int killCount)
    {
        this.killCountText.text = killCount.ToString();
    }

    private void OnBotsChanged(bool botsEnabled)
    {
        this.botsText.text = botsEnabled.ToString();
    }

    private void OnBotCountChanged(int botCount)
    {
        this.botsCountText.text = botCount.ToString();
    }
    #endregion

    
    public void IncreaseGameMode()
    {
        battleManager.IncreaseGameMode();
    }
    public void DecreaseGameMode()
    {
        battleManager.DecreaseGameMode();
    }

    public void IncreaseLives()
    {
        battleManager.IncreaseLives();
    }
    public void DecreaseLives()
    {
        battleManager.DecreaseLives();
    }


    public void IncreaseKillCount()
    {
        battleManager.IncreaseKillCount();
    }
    public void DecreaseKillCount()
    {
        battleManager.DecreaseGameMode();
    }

    public void ToggleBots()
    {
        battleManager.ToggleBots();
    }

    public void IncreaseBotCount()
    {
        battleManager.IncreaseBotCount();
    }
    public void DecreaseBotCount()
    {
        battleManager.DecreaseBotCount();
    }
}