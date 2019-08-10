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

    public void Shoot(float strength, Vector3 inheritedVelocity)
    {
        this.rigidBody.AddRelativeForce(this.transform.forward * (strength), ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable;
        if (other.gameObject.TryGetComponent<Damageable>(out damageable))
        {
            damageable.TakeDamage(damage);
            Remove();
        }
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
