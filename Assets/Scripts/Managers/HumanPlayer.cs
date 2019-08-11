using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : MonoBehaviour
{
    public int Victories
    {
        get;
        private set;
    }

    private int gold = 0;
    public int Gold { get { return gold; } }
    public delegate void OnGoldChanged(int gold);
    public OnGoldChanged onGoldChanged;

    [SerializeField]
    private Color playerColour;

    [SerializeField]
    private PlayerUIManager playerUIManager;

    enum PlayerState { Unassigned, Waiting, SelectingShip, Playing }
    PlayerState playerState = PlayerState.Unassigned;

    private CastleShip currentlySelectedShip;

    [SerializeField]
    private CastleShip castleShip;
    public CastleShip CastleShip { get { return castleShip; } }

    private void Start()
    {
        playerUIManager.ConnectToHumanPlayer(this);
        ChangeToUnassigned();
        //AddShip();
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
        //If directional buttons are pushed, change the ship
    }

    private void AddShip()
    {
        if(CastleShip!=null)
        {
            RemoveShip();
        }
        //CastleShip = BattleManager.GetNewShip();
        CastleShip.OnGoldChanged += AddGold;
        CastleShip.SetColourMaterial(this.playerColour);
        CastleShip.GetComponent<PlayerControlled>().AssignPlayer(this.BoundPlayerID);
        playerUIManager.ConnectToCastleShip(CastleShip);
    }
    private void RemoveShip()
    {
        playerUIManager.DisconnectCastleShip(CastleShip);
    }
}
