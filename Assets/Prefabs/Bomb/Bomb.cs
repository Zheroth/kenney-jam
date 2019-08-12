using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private Collider explosionHitBox;
    [SerializeField]
    private ParticleSystem explosionParticle;
    [SerializeField]
    int damage;
    [SerializeField]
    Cinemachine.CinemachineImpulseSource explosionImpulse;

    public void Explode()
    {
        this.StartCoroutine(Explode_Coroutine());
    }

    IEnumerator Explode_Coroutine()
    {
        explosionImpulse.GenerateImpulse(Vector3.one*5);
        explosionParticle.Play(true);
        this.explosionHitBox.enabled = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        this.explosionHitBox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable;
        if (other.TryGetComponent<Damageable>(out damageable))
        {
            damageable.TakeDamage(damage);
        }
    }
}
