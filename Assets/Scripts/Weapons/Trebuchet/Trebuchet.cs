using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trebuchet : MonoBehaviour
{
    [SerializeField]
    private TrebuchetTargetedArea trebuchetTargetedArea;
    [SerializeField]
    private Transform originPoint;
    [SerializeField]
    private Transform startPoint;


    private bool inUse = false;

    public void TryFire(CastleShip castleShip, string actionKey)
    {
        if(inUse)
        {
            return;
        }
        this.StartCoroutine(TargetManagement_Coroutine(castleShip, actionKey));
    }

    IEnumerator TargetManagement_Coroutine(CastleShip castleShip, string actionKey)
    {
        inUse = true;
        Player playerRef = ReInput.players.GetPlayer(castleShip.GetComponent<PlayerControlled>().PlayerID);
        TrebuchetTargetedArea trebuchetTargetedArea = Instantiate(this.trebuchetTargetedArea);
        float distance = 0;
        while (playerRef.GetButton(actionKey))
        {
            distance += Time.deltaTime*3;
            trebuchetTargetedArea.UpdatePosition(startPoint.position + castleShip.transform.forward * distance, originPoint.transform.position);
            yield return null;
        }
        Destroy(trebuchetTargetedArea.gameObject);
        inUse = false;
    }
}
