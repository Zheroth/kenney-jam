using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrebuchetTargetedArea : MonoBehaviour
{
    [SerializeField]
    private CurvedLinePoint[] curvedLinePoints;

    public void UpdatePosition(Vector3 newPosition, Vector3 casterPosition)
    {
        this.transform.position = newPosition;
        curvedLinePoints[0].transform.position = casterPosition;
        curvedLinePoints[2].transform.position = newPosition;

        curvedLinePoints[1].transform.position = Vector3.Lerp(newPosition, casterPosition, 0.5f);
        curvedLinePoints[1].transform.position += Vector3.up * 5;
    }
}
