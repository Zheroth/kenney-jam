using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField]
    private CastleShip castleShip;
    [SerializeField]
    private UICombinedFilledImage healthBarImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI healthPercentage;
    [SerializeField]
    private TMPro.TextMeshProUGUI goldText;

    void Start()
    {
        castleShip.DamageableRef.onHpChanged += OnHPChanged;
        castleShip.onGoldChanged += OnGoldChanged;

        SetHealthPer(castleShip.DamageableRef.HealthPercentage);
        OnGoldChanged(castleShip.Gold);
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
}
