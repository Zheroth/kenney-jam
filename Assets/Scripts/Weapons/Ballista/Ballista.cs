using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Ballista fires at a DPS while the button is held down
/// </summary>
public class Ballista : MonoBehaviour
{
    [SerializeField]
    private float shotsPerSecond = 2;
    [SerializeField]
    private BallistaArrow ballistaArrow;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private float shotStrength = 10;
    [SerializeField]
    float angleSpread = 20;

    private float cooldownTimer = 0;
    
    // Update is called once per frame
    void Update()
    {
        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            TryFire();
        }
    }

    void TryFire()
    {
        if(cooldownTimer <= 0)
        {
            Fire();
            cooldownTimer = 1 / shotsPerSecond;
        }
    }

    void Fire()
    {
        BallistaArrow missile = GameObject.Instantiate(ballistaArrow);
        missile.transform.position = this.shootPoint.position;
        missile.transform.rotation = this.shootPoint.rotation;
        missile.transform.Rotate(0, Random.Range(-angleSpread, angleSpread), 0, Space.Self);
        missile.Shoot(shotStrength);
    }
}
