using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    enum UISTate { Waiting, SelectingShip, Playing }
    UISTate uiState = UISTate.Waiting;

    /// WAITING
    [SerializeField]
    private GameObject waitingUIGroup;

    /// PLAYING
    [SerializeField]
    private GameObject playingUIGroup;
    [SerializeField]
    private CastleShip castleShip;
    [SerializeField]
    private UICombinedFilledImage healthBarImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI healthPercentage;
    [SerializeField]
    private TMPro.TextMeshProUGUI goldText;

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

    private void Start()
    {
        UnbindPlayer();
    }

    public void BindPlayer(PlayerManager.PlayerArgs playerArgs)
    {
        this.BoundPlayerID = playerArgs.PlayerId;
        this.HasPlayer = true;

        castleShip.DamageableRef.onHpChanged += OnHPChanged;
        castleShip.onGoldChanged += OnGoldChanged;

        SetHealthPer(castleShip.DamageableRef.HealthPercentage);
        OnGoldChanged(castleShip.Gold);

        ChangeToShipSelection();
    }

    public void UnbindPlayer()
    {
        this.BoundPlayerID = -1;
        this.HasPlayer = false;

        castleShip.DamageableRef.onHpChanged -= OnHPChanged;
        castleShip.onGoldChanged -= OnGoldChanged;

        SetHealthPer(1);
        OnGoldChanged(0);

        ChangeToWaiting();
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

    private void ChangeToWaiting()
    {
        this.uiState = UISTate.Waiting;
        this.waitingUIGroup.SetActive(true);
        this.playingUIGroup.SetActive(false);
    }

    private void ChangeToShipSelection()
    {
        this.uiState = UISTate.SelectingShip;
        this.waitingUIGroup.SetActive(false);
        this.playingUIGroup.SetActive(true);
    }
}