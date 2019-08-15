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
    [SerializeField]
    CatapultBall catapultBall;
    [SerializeField]
    int catapultBallsToSpawn = 10;

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
        Vector3 endPosition = Vector3.zero;
        float timeHeld = 0;
        while (playerRef.GetButton(actionKey))
        {
            timeHeld += Time.deltaTime;
            distance += Time.deltaTime*3;
            distance = Mathf.Clamp(distance, 0, 10);
            endPosition = startPoint.position + castleShip.transform.forward * distance;
            trebuchetTargetedArea.UpdatePosition(endPosition, originPoint.transform.position);
            yield return null;
        }
        Destroy(trebuchetTargetedArea.gameObject);

        inUse = false;
        if (timeHeld <= 0.3f)
        {
            yield break;
        }

        for (int i = 0; i < catapultBallsToSpawn; i++)
        {
            CatapultBall spawnedBall = GameObject.Instantiate(catapultBall);
            spawnedBall.castleShipRef = castleShip;
            Vector3 randomXZ = Random.insideUnitCircle * 1.5f;
            float randomY = Random.Range(0f, 3f);

            spawnedBall.transform.position = 
                new Vector3(endPosition.x + randomXZ.x,
                endPosition.y + 10 + randomY, endPosition.z + randomXZ.y);
        }
    }
}
