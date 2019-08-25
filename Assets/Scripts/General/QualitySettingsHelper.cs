using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wilberforce.FinalCameraEffectsPro;

public class QualitySettingsHelper : MonoBehaviour
{
    [SerializeField]
    private AmplifyColorEffect amplifyColor;
    [SerializeField]
    private AmplifyOcclusionEffect amplifyOcclusion;
    [SerializeField]
    private AmplifyBloom.AmplifyBloomEffect amplifyBloom;
    [SerializeField]
    FinalCameraEffectsPro finalCameraEffectsPro;
    [SerializeField]
    FocusDistanceAdjuster focusDistanceAdjuster;

    // Start is called before the first frame update
    void Awake()
    {
        UpdateBasedOnQualityLevel(QualitySettings.GetQualityLevel());
    }

    void UpdateBasedOnQualityLevel(int level)
    {
        //Basic
        if (level == 0)
        {
            UpdateBasic();
        }
        //Medium
        if (level == 1)
        {
            UpdateMedium();
        }
        //High
        if (level == 2)
        {
            UpdateHigh();
        }
        //Ultra
        if (level == 3)
        {
            UpdateUltra();
        }
        //High Alt
        if (level == 4)
        {
            UpdateHighAlt();
        }
        //High Alt 2
        if (level == 5)
        {
            UpdateHighAlt2();
        }
        //High Alt 3
        if (level == 6)
        {
            UpdateHighAlt3();
        }
    }

    void UpdateBasic()
    {
        amplifyColor.enabled = false;
        amplifyOcclusion.enabled = false;
        amplifyBloom.enabled = false;
        finalCameraEffectsPro.enabled = false;
        focusDistanceAdjuster.enabled = false;
    }
    void UpdateMedium()
    {
        amplifyColor.enabled = false;
        amplifyOcclusion.enabled = false;
        amplifyBloom.enabled = false;
        finalCameraEffectsPro.enabled = true;
        focusDistanceAdjuster.enabled = true;
    }
    void UpdateHigh()
    {
        amplifyColor.enabled = true;
        amplifyOcclusion.enabled = true;
        amplifyBloom.enabled = true;
        finalCameraEffectsPro.enabled = true;
        focusDistanceAdjuster.enabled = true;
    }
    void UpdateUltra()
    {
        amplifyColor.enabled = true;
        amplifyOcclusion.enabled = true;
        amplifyBloom.enabled = true;
        finalCameraEffectsPro.enabled = true;
        focusDistanceAdjuster.enabled = true;
    }
    void UpdateHighAlt()
    {
        amplifyColor.enabled = true;
        amplifyOcclusion.enabled = true;
        amplifyBloom.enabled = true;
        finalCameraEffectsPro.enabled = true;
        focusDistanceAdjuster.enabled = true;
    }
    void UpdateHighAlt2()
    {
        amplifyColor.enabled = true;
        amplifyOcclusion.enabled = false;
        amplifyBloom.enabled = true;
        finalCameraEffectsPro.enabled = true;
        focusDistanceAdjuster.enabled = true;
    }
    void UpdateHighAlt3()
    {
        amplifyColor.enabled = true;
        amplifyOcclusion.enabled = true;
        amplifyBloom.enabled = true;
        finalCameraEffectsPro.enabled = false;
        focusDistanceAdjuster.enabled = false;
    }
}
