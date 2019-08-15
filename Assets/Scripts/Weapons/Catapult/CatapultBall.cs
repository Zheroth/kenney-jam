using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultBall : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Rigidbody rigidBody;
    [SerializeField]
    private int damage;
    [SerializeField]
    TrailRenderer trail;
    [SerializeField]
    GameObject ball;

    private bool removed = false;

    //PUBLIC FOR HACKS
    public CastleShip castleShipRef;

    void FixedUpdate()
    {
        if (rigidBody.velocity != Vector3.zero)
            rigidBody.rotation = Quaternion.LookRotation(rigidBody.velocity);
    }

    public void Shoot(float strength, CastleShip castleShip)
    {
        this.castleShipRef = castleShip;
        Rigidbody shipRigidbody = castleShip.RigidbodyRef;
        Vector2 forcePos = this.transform.position;
        forcePos.y = shipRigidbody.centerOfMass.y;
        shipRigidbody.AddForce(-shipRigidbody.transform.forward * strength/2, ForceMode.Impulse);
        this.rigidBody.AddForce(this.transform.forward * strength, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Damageable damageable;
        if (collision.gameObject.TryGetComponent<Damageable>(out damageable))
        {
            damageable.TakeDamage(damage, () => castleShipRef.AddKills(1));
        }
        Remove();
    }

    private void Update()
    {
        if(!removed)
        {
            if (this.transform.position.y < 30)
            {
                Remove();
            }
        }
    }

    private void Remove()
    {
        this.removed = true;
        Destroy(this.gameObject, trail.time);
        this.ball.SetActive(false);
        this.rigidBody.isKinematic = true;
    }
}
