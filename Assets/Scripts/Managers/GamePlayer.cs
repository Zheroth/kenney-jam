using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    [SerializeField]
    private bool ai = false;

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

    private int kills;
    public int Kills { get { return kills; } }
    private int gold = 0;
    public int Gold { get { return gold; } }
    public delegate void OnGoldChanged(int gold);
    public OnGoldChanged onGoldChanged;

    public delegate void OnShipChanged(CastleShip newShip);
    public OnShipChanged onShipChanged;

    public delegate void OnKillsChanged(int killCount);
    public OnKillsChanged onKillsChanged;

    [SerializeField]
    private Color playerColour;

    [SerializeField]
    private PlayerUIManager playerUIManager;

    enum PlayerState { Unassigned, Waiting, SelectingShip, Playing }
    PlayerState playerState = PlayerState.Unassigned;

    private CastleShip.CastleShipType currentlySelectedShip = CastleShip.CastleShipType.Assaulter;

    [SerializeField]
    private CastleShip castleShip;
    public CastleShip CastleShip { get { return castleShip; } }

    private void Start()
    {
        playerUIManager.ConnectToGamePlayer(this);

        if(ai)
        {
            this.HasPlayer = true;
            ChangeToPlaying();
        }
        else
        {
            ChangeToUnassigned();
        }
    }

    public int BoundPlayerID
    {
        get;
        private set;
    }

    public bool HasPlayer
    {
        get;
        private set;
    }

    public void BindPlayer(PlayerManager.PlayerArgs playerArgs)
    {
        this.BoundPlayerID = playerArgs.PlayerId;
        this.HasPlayer = true;
        playerRef = ReInput.players.GetPlayer(BoundPlayerID);
        ChangeToShipSelection();
    }

    public void UnBindPlayer()
    {
        this.BoundPlayerID = 0;
        this.HasPlayer = false;
        ChangeToUnassigned();
    }

    private void ChangeToUnassigned()
    {
        this.playerState = PlayerState.Unassigned;
        playerUIManager.ChangeToUnassigned();
    }

    private void ChangeToWaiting()
    {
        this.playerState = PlayerState.Waiting;
        playerUIManager.ChangeToWaiting();
    }

    private void ChangeToShipSelection()
    {
        this.playerState = PlayerState.SelectingShip;
        playerUIManager.ChangeToShipSelection();
    }

    private void ChangeToPlaying()
    {
        AddShip(currentlySelectedShip, ai);
        this.playerState = PlayerState.Playing;
        playerUIManager.ChangeToPlaying();
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
        if(playerRef.GetButtonDown("Left"))
        {
            currentlySelectedShip--;
            if ((int)currentlySelectedShip < 0)
            {
                currentlySelectedShip = (CastleShip.CastleShipType)2;
            }
            onShipChanged(battleManagerRef.GetShip(currentlySelectedShip));
        }
        else if (playerRef.GetButtonDown("Right"))
        {
            currentlySelectedShip++;
            if ((int)currentlySelectedShip >= 3)
            {
                currentlySelectedShip = 0;
            }
            onShipChanged(battleManagerRef.GetShip(currentlySelectedShip));
        }else if(playerRef.GetButtonDown("Accept"))
        {
            ChangeToPlaying();
        }
    }

    private void AddKill(int newKills)
    {
        this.kills += newKills;
        onKillsChanged?.Invoke(kills);
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
        CastleShip.SetColourMaterial(this.playerColour);
        playerUIManager.ConnectToCastleShip(CastleShip);
        CastleShip.DamageableRef.OnDeath.AddListener(OnShipDie);
    }
    private void RemoveShip()
    {
        GameObject.Destroy(CastleShip.gameObject);
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

        Time.timeScale = 0.3f;
        while(timer < 2)
        {
            timer += Time.unscaledDeltaTime;

            //float completion = timer / 3;

            //this.transform.localScale = Vector3.Lerp(scale, Vector3.zero, completion);

            yield return null;
        }
        Time.timeScale = 1f;

        RemoveShip();
        if(ai)
        {
            ChangeToPlaying();
        }
        else
        {
            ChangeToShipSelection();
        }
    }
}
