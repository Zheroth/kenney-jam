using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    [SerializeField]
    private float shotsPerSecond = 2;
    [SerializeField]
    private CatapultBall catapultBall;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private float shotStrength = 10;
    [SerializeField]
    float angleSpread = 20;

    private float cooldownTimer = 0;

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public void TryFire(CastleShip castleShip, string actionID)
    {
        if (cooldownTimer <= 0)
        {
            Fire(castleShip, actionID);
            cooldownTimer = 1 / shotsPerSecond;
        }
    }

    void Fire(CastleShip castleShip, string actionID)
    {
        CatapultBall catapultBall = GameObject.Instantiate(this.catapultBall);
        catapultBall.transform.position = this.shootPoint.position;
        catapultBall.transform.rotation = this.shootPoint.rotation;
        //catapultBall.transform.position += this.controllable.RigidbodyRef.velocity * 0.2f;
        catapultBall.transform.Rotate(Random.Range(-angleSpread, angleSpread), Random.Range(-angleSpread, angleSpread), 0, Space.Self);
        catapultBall.Shoot(shotStrength, castleShip.RigidbodyRef);
    }
}
