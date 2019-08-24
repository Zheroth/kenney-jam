using Rewired;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    private Player playerRef;
    private Player PlayerRef
    {
        get
        {
            if (playerRef == null)
            {
                playerRef = ReInput.players.GetPlayer(1);
            }
            return playerRef;
        }
    }

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
    private MenuSelectable currentSelected;

    MenuSelectable[] selectables;
    MenuSelectable[] Selectables
    {
        get
        {
            if(selectables == null)
            {
                selectables = this.GetComponentsInChildren<MenuSelectable>(true);
            }
            return selectables;
        }
    }

    [SerializeField]
    private MenuSelectable gameModeSelect;
    [SerializeField]
    private MenuSelectable livesSelect;
    [SerializeField]
    private MenuSelectable killCountSelect;
    [SerializeField]
    private MenuSelectable botsSelect;
    [SerializeField]
    private MenuSelectable botsCountSelect;

    private void Start()
    {
        battleManager.OnGameModeChanged += OnGameModeChanged;
        battleManager.OnLivesChanged += OnLivesChanged;
        battleManager.OnKillCountChanged += OnKillCountChanged;
        battleManager.OnBotsChanged += OnBotsChanged;
        battleManager.OnBotCountChanged += OnBotCountChanged;

        battleManager.RunAllCallbacks();

        Select(gameModeSelect);
    }

    #region Callbacks
    private void OnGameModeChanged(BattleManager.GameMode gameMode)
    {
        this.gameModeText.text = gameMode.ToString();
        if (gameMode == BattleManager.GameMode.Deathmatch)
        {
            lives.SetActive(true);
            killCountSelect.gameObject.SetActive(false);
        }
        else if (gameMode == BattleManager.GameMode.KillCount)
        {
            lives.SetActive(false);
            killCountSelect.gameObject.SetActive(true);
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

        this.botsCountSelect.gameObject.SetActive(botsEnabled);
    }

    private void OnBotCountChanged(int botCount)
    {
        this.botsCountText.text = botCount.ToString();
    }
    #endregion

    #region Value Adjustements
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
        battleManager.DecreaseKillCount();
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
    #endregion

    #region Select Methods
    public void Select(MenuSelectable selectable)
    {
        if(selectable!=null)
        {
            currentSelected?.Deselect();
            currentSelected = selectable;
            currentSelected?.Select();
        }
    }

    private void Update()
    {
        if (PlayerRef == null || currentSelected == null) return;

        if (PlayerRef.GetButtonDown("Up"))
        {
            MoveToSelectable(-1);
        }

        if (PlayerRef.GetButtonDown("Down"))
        {
            MoveToSelectable(1);
        }

        currentSelected?.OnUpdate(PlayerRef);
    }

    /// <summary>
    /// 1 Down
    /// -1 up
    /// </summary>
    /// <param name="direction"></param>
    private void MoveToSelectable(int direction)
    {
        float closest = Mathf.Infinity;
        MenuSelectable closestMenuSelectable = null;
        for (int i = 0; i < Selectables.Length; i++)
        {
            if (Selectables[i] == currentSelected || !Selectables[i].gameObject.activeInHierarchy) { continue; }

            RectTransform currRectTransform = (RectTransform)currentSelected.transform;
            RectTransform rectTransform = (RectTransform)Selectables[i].transform;

            float difference = rectTransform.position.y - currRectTransform.position.y;
            if (Mathf.Sign(difference) == direction)
            {
                continue;
            }

            float distance = Mathf.Abs(difference);
            if (distance < closest)
            {
                closest = distance;
                closestMenuSelectable = Selectables[i];
            }
        }

        if (closestMenuSelectable != null) { Select(closestMenuSelectable); }
    }

    #endregion

    public void StartMatch()
    {
        this.battleManager.ChangeToStartMatch();
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}