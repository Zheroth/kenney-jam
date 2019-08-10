using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaArrow : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rigidBody;
    [SerializeField]
    private int damage;

    public void Shoot(float strength)
    {
        this.rigidBody.AddRelativeForce(this.transform.forward*strength, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable;
        if(other.gameObject.TryGetComponent<Damageable>(out damageable))
        {
            damageable.TakeDamage(damage);
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
