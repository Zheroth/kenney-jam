using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wilberforce.FinalCameraEffectsPro;

public class FocusDistanceAdjuster : MonoBehaviour
{
    [SerializeField]
    private Cinemachine.CinemachineTargetGroup targetGroup;
    [SerializeField]
    private FinalCameraEffectsPro finalCameraEffectsPro;

    [SerializeField]
    private GameObject sphere;

    public void Update()
    {
        Vector3 point = Vector3.zero;
        int totalCount = 0;
        for (int i = 0; i < targetGroup.m_Targets.Length; i++)
        {
            if(targetGroup.m_Targets[i].target != null && targetGroup.m_Targets[i].weight == 1)
            {
                point += targetGroup.m_Targets[i].target.position;
                totalCount++;
            }
        }

        point = point / totalCount;

        sphere.transform.position = point;

        finalCameraEffectsPro.FocalPlaneDistance = Vector3.Distance(finalCameraEffectsPro.transform.position, point);
    }
}
