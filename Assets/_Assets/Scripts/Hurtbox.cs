using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private HpInterface hpInterface;

    public void OnTriggerEnter2D(Collider2D _other)
    {
        Hitbox otherHitbox = _other.GetComponent<Hitbox>();

        if (hpInterface.TakeDamage(otherHitbox.Damage))
        {
            VFXManager.Instance.SpawnParticleSystem(ParticleSystemType.Hit, transform.position, otherHitbox.transform.rotation);
            if (otherHitbox.DestroyOnHit)
                Destroy(otherHitbox.gameObject);
        }
    }
}
