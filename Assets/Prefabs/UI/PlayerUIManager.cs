using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField]
    private Damageable damageable;
    [SerializeField]
    private UICombinedFilledImage healthBarImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI healthPercentage;

    void Start()
    {
        damageable.onHpChanged += OnHPChanged;
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
}
