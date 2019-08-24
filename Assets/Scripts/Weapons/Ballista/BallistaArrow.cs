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
    TrailRenderer trail;
    [SerializeField]
    GameObject missile;

    private bool removed = false;

    private CastleShip castleShip;

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
            if(castleShip != null && other.gameObject == castleShip.gameObject)
            {
                return;
            }
            damageable.TakeDamage(damage, () => castleShip?.AddKills(1));
            Remove();
        }
    }

    private void Update()
    {
        if (!removed)
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
        trail.time = 0.5f;
        Destroy(this.gameObject, trail.time);
        this.missile.SetActive(false);
        this.rigidBody.isKinematic = true;
    }
}
