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
    [SerializeField]
    CastleShip castleShip;

    private float cooldownTimer = 0;

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            TryFire();
        }
    }

    void TryFire()
    {
        if (cooldownTimer <= 0)
        {
            Fire();
            cooldownTimer = 1 / shotsPerSecond;
        }
    }

    void Fire()
    {
        CatapultBall catapultBall = GameObject.Instantiate(this.catapultBall);
        catapultBall.transform.position = this.shootPoint.position;
        catapultBall.transform.rotation = this.shootPoint.rotation;
        //catapultBall.transform.position += this.controllable.RigidbodyRef.velocity * 0.2f;
        //catapultBall.transform.Rotate(0, Random.Range(-angleSpread, angleSpread), 0, Space.Self);
        catapultBall.Shoot(shotStrength, this.castleShip.RigidbodyRef);
    }
}
