using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultBall : MonoBehaviour
{
    // Start is called before the first frame update
    private CastleShip castleRef;
    [SerializeField]
    private int damage;

    void FixedUpdate()
    {
        if (castleRef.RigidbodyRef.velocity != Vector3.zero)
            castleRef.RigidbodyRef.rotation = Quaternion.LookRotation(castleRef.RigidbodyRef.velocity);
    }

    public void Shoot(float strength, CastleShip castleShip)
    {
        this.castleRef = castleShip;
        Vector2 forcePos = this.transform.position;
        forcePos.y = castleShip.RigidbodyRef.centerOfMass.y;
        castleShip.RigidbodyRef.AddForce(-castleShip.transform.forward * strength/2, ForceMode.Impulse);
        this.castleRef.RigidbodyRef.AddForce(this.transform.forward * strength, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Damageable damageable;
        if (collision.gameObject.TryGetComponent<Damageable>(out damageable))
        {
            damageable.TakeDamage(damage, () => castleRef.AddKills(1));
        }
        Remove();
    }

    private void Update()
    {
        if (this.transform.position.y < 30)
        {
            Remove();
        }
    }

    private void Remove()
    {
        GameObject.Destroy(this.gameObject);
    }
}
