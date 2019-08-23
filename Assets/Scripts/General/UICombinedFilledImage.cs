using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICombinedFilledImage : MonoBehaviour
{
    [SerializeField]
    private Image[] images;
    [SerializeField]
    [Range(0,1)]
    private float fillPercentage = 1;
    [SerializeField]
    private Color barColor;

    private void OnValidate()
    {
        SetFillAmount(fillPercentage);
        SetColor(barColor);
    }

    public void SetColor(Color color)
    {
        for (int i = 0; i < images.Length; i++)
        {
            Image image = images[i];
            image.color = color;
        }
    }

    public void SetFillAmount(float value)
    {
        fillPercentage = value;

        float division = 1.0f / images.Length;

        for (int i = 0; i < images.Length; i++)
        {
            Image image = images[i];
            float section = (i * division);
            float fillLevel = MapValue(section, section + division, 0, 1, fillPercentage);
            image.fillAmount = fillLevel;
        }
    }

    private float MapValue(float a0, float a1, float b0, float b1, float a)
    {
        return b0 + (b1 - b0) * ((a - a0) / (a1 - a0));
    }

}
