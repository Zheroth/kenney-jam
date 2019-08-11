using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    // UNASSIGNED
    [SerializeField]
    private GameObject unassignedUIGroup;

    /// WAITING
    [SerializeField]
    private GameObject waitingUIGroup;

    /// PLAYING
    [SerializeField]
    private GameObject playingUIGroup;
    [SerializeField]
    private UICombinedFilledImage healthBarImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI healthPercentage;
    [SerializeField]
    private TMPro.TextMeshProUGUI goldText;

    // SELECTING SHIP
    [SerializeField]
    private GameObject selectingShipUIGroup;
    [SerializeField]
    private TMPro.TextMeshProUGUI shipName;
    [SerializeField]
    private UnityEngine.UI.Image shipImage;

    private void Start()
    {
        SetHealthPer(1);
        OnGoldChanged(0);
    }

    public void ConnectToHumanPlayer(HumanPlayer humanPlayer)
    {
        humanPlayer.onGoldChanged += OnGoldChanged;
        humanPlayer.onShipChanged += OnShipSelectionChanged;
    }

    public void ConnectToCastleShip(CastleShip castleShip)
    {
        castleShip.DamageableRef.onHpChanged += OnHPChanged;
    }
    public void DisconnectCastleShip(CastleShip castleShip)
    {
        castleShip.DamageableRef.onHpChanged -= OnHPChanged;
    }

    private void SetHealthPer(float healthPer)
    {
        healthBarImage.SetFillAmount(healthPer);
        healthPercentage.text = ((int)(healthPer*100)).ToString();
    }

    private void OnHPChanged(int currentHealth, int maxHealth, float hpPercentage)
    {
        SetHealthPer(hpPercentage);
    }

    private void OnGoldChanged(int gold)
    {
        goldText.text = gold.ToString();
    }

    private void OnShipSelectionChanged(CastleShip ship)
    {
        this.shipImage.sprite = ship.Image;
        this.shipName.text = ship.ShipName;
    }

    public void ChangeToUnassigned()
    {
        this.waitingUIGroup.SetActive(false);
        this.playingUIGroup.SetActive(false);
        this.selectingShipUIGroup.SetActive(false);
        this.unassignedUIGroup.SetActive(true);
    }

    public void ChangeToWaiting()
    {
        this.waitingUIGroup.SetActive(true);
        this.playingUIGroup.SetActive(false);
        this.selectingShipUIGroup.SetActive(false);
        this.unassignedUIGroup.SetActive(false);
    }

    public void ChangeToPlaying()
    {
        this.waitingUIGroup.SetActive(false);
        this.playingUIGroup.SetActive(true);
        this.selectingShipUIGroup.SetActive(false);
        this.unassignedUIGroup.SetActive(false);
    }

    public void ChangeToShipSelection()
    {
        this.waitingUIGroup.SetActive(false);
        this.playingUIGroup.SetActive(false);
        this.selectingShipUIGroup.SetActive(true);
        this.unassignedUIGroup.SetActive(false);
    }
}