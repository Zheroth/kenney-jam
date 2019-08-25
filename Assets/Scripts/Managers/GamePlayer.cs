using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    [SerializeField]
    ConfigurableJoint anchoredJoint;

    [SerializeField]
    private bool ai = false;
    public bool isAI
    {
        get
        {
            return ai;
        }
    }

    [SerializeField]
    BattleManager battleManagerRef;
    Player playerRef;

    [SerializeField]
    Transform spawnPosition;

    public int Victories
    {
        get;
        private set;
    }

    public bool IsDead { get { return playerState == PlayerState.Dead; }}

    private bool useLives = false;
    private int maxLives = 3;
    private int lives = 3;
    public int Lives { get { return lives; } }
    public delegate void OnLivesChanged(int lives);
    public OnLivesChanged onLivesChanged;
    public delegate void OnUseLivesChanged(bool useLives);
    public OnUseLivesChanged onUseLivesChanged;

    private int gold = 0;
    public int Gold { get { return gold; } }
    public delegate void OnGoldChanged(int gold);
    public OnGoldChanged onGoldChanged;

    public delegate void OnShipChanged(CastleShip newShip);
    public OnShipChanged onShipChanged;

    private int kills;
    public int Kills { get { return kills; } }
    public delegate void OnKillsChanged(int killCount);
    public OnKillsChanged onKillsChanged;

    public delegate void OnAddKill(GamePlayer gamePlayer);
    public OnAddKill onAddKill;

    public Color PlayerColour
    {
        get
        {
            return battleManagerRef.GetColour(colorId);
        }
    }
    private int colorId = 0;
    private int ColorID
    {
        get
        {
            return colorId;
        }
        set
        {
            colorId = value;
            onColourChanged?.Invoke(PlayerColour);
        }
    }

    public delegate void OnColourChanged(Color color);
    public OnColourChanged onColourChanged;

    public Action<bool> OnKingStatusChanged;

    [SerializeField]
    private PlayerUIManager playerUIManager;

    enum PlayerState { Unassigned, Assigned, Waiting, SelectingShip, Playing, EndOfBattle, Dead }
    PlayerState playerState = PlayerState.Unassigned;

    private CastleShip.CastleShipType currentlySelectedShip = CastleShip.CastleShipType.Assaulter;

    [SerializeField]
    private CastleShip castleShip;
    public CastleShip CastleShip { get { return castleShip; } }

    private void Start()
    {
        playerUIManager.ConnectToGamePlayer(this);
        ChangeKingStatus(false);
        ChangeToUnassigned();
    }

    public int BoundPlayerID
    {
        get;
        private set;
    }

    public string PlayerName
    {
        get { return string.Format("PLAYER{0}", (BoundPlayerID+1).ToString("0")); }
    }

    public bool HasPlayer
    {
        get;
        private set;
    }

    public void SetColour(int colour)
    {
        this.ColorID = colour;
    }

    public void BindPlayer(PlayerManager.PlayerArgs playerArgs)
    {
        this.BoundPlayerID = playerArgs.PlayerId;
        this.HasPlayer = true;
        playerRef = ReInput.players.GetPlayer(BoundPlayerID);
        ChangeToAssigned(playerArgs);
    }

    public void UnBindPlayer()
    {
        this.BoundPlayerID = 0;
        this.HasPlayer = false;
        ChangeToUnassigned();
    }

    public void ChangeToAssigned(PlayerManager.PlayerArgs playerArgs)
    {
        this.playerState = PlayerState.Assigned;
        playerUIManager.ChangeToAssigned(playerArgs);
        ColorID = battleManagerRef.GetFirstAvailableColour();
    }

    public void ChangeToEndOfBattle()
    {
        this.CancelInvoke("ChangeToPlaying");
        this.playerState = PlayerState.EndOfBattle;
        this.RemoveShip();
        playerUIManager.ChangeToEndOfBattle();
    }

    public void ChangeToUnassigned()
    {
        this.playerState = PlayerState.Unassigned;

        if (this.HasPlayer)
        {
            this.ResetValues();
            if (this.isAI)
            {
                this.UnBindAI();
            }
            else
            {
                this.UnBindPlayer();
            }
            this.onAddKill -= battleManagerRef.OnShipKilled;
        }

        playerUIManager.ChangeToUnassigned();
    }

    public void ChangeToWaiting()
    {
        this.playerState = PlayerState.Waiting;
        playerUIManager.ChangeToWaiting();
    }

    public void ChangeToShipSelection()
    {
        this.playerState = PlayerState.SelectingShip;
        playerUIManager.ChangeToShipSelection();

        if (ai)
        {
            //TODO Add Ship Selection Coroutine
            Invoke("ChangeToPlaying", UnityEngine.Random.Range(4, 10));
        }
    }

    private void ChangeToPlaying()
    {
        AddShip(currentlySelectedShip, ai);
        this.playerState = PlayerState.Playing;
        playerUIManager.ChangeToPlaying();
    }

    private void ChangeToDead()
    {
        this.playerState = PlayerState.Dead;
        playerUIManager.ChangeToDead();
    }

    public void AddGold(int gold)
    {
        this.gold += gold;
        onGoldChanged?.Invoke(this.gold);
    }

    private void Update()
    {
        switch (playerState)
        {
            case PlayerState.Unassigned:
                break;
            case PlayerState.Assigned:
                AssignedUpdate();
                break;
            case PlayerState.Waiting:
                break;
            case PlayerState.SelectingShip:
                SelectingShipUpdate();
                break;
            case PlayerState.Playing:
                break;
            default:
                break;
        }
    }

    private void SelectingShipUpdate()
    {
        if(isAI)
        {

        }
        else
        {
            if (playerRef.GetButtonDown("Left"))
            {
                currentlySelectedShip--;
                if ((int)currentlySelectedShip < 0)
                {
                    currentlySelectedShip = (CastleShip.CastleShipType)0;
                }
                onShipChanged(battleManagerRef.GetShip(currentlySelectedShip));
            }
            else if (playerRef.GetButtonDown("Right"))
            {
                currentlySelectedShip++;
                if ((int)currentlySelectedShip >= 1)
                {
                    currentlySelectedShip = 0;
                }
                onShipChanged(battleManagerRef.GetShip(currentlySelectedShip));
            }
            else if (playerRef.GetButtonDown("Accept"))
            {
                ChangeToPlaying();
            }
        }
    }

    private void AssignedUpdate()
    {
        if (ai)
        {

        }
        else
        {
            if (playerRef.GetButtonDown("ColourCycleLeft"))
            {
                this.ColorID = battleManagerRef.GetNextColor(this.ColorID, -1);
            }
            if (playerRef.GetButtonDown("ColourCycleRight"))
            {
                this.ColorID = battleManagerRef.GetNextColor(this.ColorID, 1);
            }

            if(battleManagerRef.MatchInProgress)
            {
                if (playerRef.GetButtonDown("Accept"))
                {
                    this.ChangeToShipSelection();
                }
            }
        }
    }

    public void SetUsingLives(bool useLives)
    {
        this.useLives = useLives;
        onUseLivesChanged?.Invoke(this.useLives);
    }

    public void SetMaxLives(int maxLives)
    {
        this.maxLives = maxLives;
    }

    public void SetLivesToMax()
    {
        this.lives = this.maxLives;
        onLivesChanged?.Invoke(Lives);
    }

    private void AddKill(int newKills)
    {
        this.kills += newKills;
        onKillsChanged?.Invoke(kills);
        onAddKill?.Invoke(this);
    }

    private void AddLife(int livesToAdd)
    {
        lives += livesToAdd;
        onLivesChanged?.Invoke(Lives);
    }

    private void RemoveLife(int livesToRemove)
    {
        lives -= livesToRemove;
        onLivesChanged?.Invoke(Lives);
    }

    private void AddShip(CastleShip.CastleShipType castleShipType, bool ai = false)
    {
        if (CastleShip!=null)
        {
            RemoveShip();
        }
        castleShip = battleManagerRef.SpawnShip(castleShipType, this.BoundPlayerID, this.spawnPosition, ai);
        CastleShip.OnGoldChanged += AddGold;
        CastleShip.OnKill += AddKill;
        CastleShip.SetColourMaterial(this.PlayerColour);
        playerUIManager.ConnectToCastleShip(CastleShip);
        CastleShip.DamageableRef.OnDeath.AddListener(OnShipDie);
        anchoredJoint.connectedBody = CastleShip.RigidbodyRef;
    }
    private void RemoveShip()
    {
        if(CastleShip!=null)
        {
            GameObject.Destroy(CastleShip.gameObject);
        }
    }

    public void ChangeKingStatus(bool value)
    {
        OnKingStatusChanged?.Invoke(value);
    }

    private void OnShipDie()
    {
        this.StartCoroutine(ShipDeathSequence());
    }

    private IEnumerator ShipDeathSequence()
    {
        playerUIManager.DisconnectCastleShip(CastleShip);

        Vector3 scale = this.transform.localScale;
        float timer = 0;

        Time.timeScale = 0.5f;
        while(timer < 1)
        {
            timer += Time.unscaledDeltaTime;

            //float completion = timer / 3;

            //this.transform.localScale = Vector3.Lerp(scale, Vector3.zero, completion);

            yield return null;
        }
        Time.timeScale = 1f;

        RemoveShip();

        RemoveLife(1);
        if (useLives && Lives < 0)
        {
            this.ChangeToDead();
        }
        else
        {
            ChangeToShipSelection();
        }

        battleManagerRef.OnShipDeath(this);
    }

    public void ResetValues()
    {
        this.kills = 0;
        this.onKillsChanged(0);
        this.gold = 0;
        this.onGoldChanged(0);
        SetLivesToMax();
    }

    public void BindAI()
    {
        this.ai = true;
        this.HasPlayer = true;
    }
    public void UnBindAI()
    {
        this.ai = false;
        this.HasPlayer = false;
        this.RemoveShip();
    }
}
