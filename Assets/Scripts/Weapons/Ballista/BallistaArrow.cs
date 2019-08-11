using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaArrow : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rigidBody;
    [SerializeField]
    private int damage;

    [SerializeField]
    CastleShip castleShip;

    void FixedUpdate()
    {
        if (rigidBody.velocity != Vector3.zero)
            rigidBody.rotation = Quaternion.LookRotation(rigidBody.velocity);
    }

    public void Shoot(float strength, CastleShip castleShip)
    {
        this.rigidBody.AddRelativeForce(this.transform.forward*strength, ForceMode.Impulse);
        this.castleShip = castleShip;
    }

    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable;
        if(other.gameObject.TryGetComponent<Damageable>(out damageable))
        {
            if(other.gameObject == castleShip.gameObject)
            {
                return;
            }
            damageable.TakeDamage(damage, () => castleShip.AddKills(1));
            Remove();
        }
    }

    private void Update()
    {
        if(this.transform.position.y < 30)
        {
            Remove();
        }
    }

    private void Remove()
    {
        GameObject.Destroy(this.gameObject);
    }
}
