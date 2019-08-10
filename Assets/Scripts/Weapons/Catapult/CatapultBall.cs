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

    void FixedUpdate()
    {
        if (rigidBody.velocity != Vector3.zero)
            rigidBody.rotation = Quaternion.LookRotation(rigidBody.velocity);
    }

    public void Shoot(float strength, Rigidbody shipRigidbody)
    {
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
            damageable.TakeDamage(damage);
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
