using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private HpInterface hpInterface;

    public void OnTriggerEnter2D(Collider2D _other)
    {
        Hitbox otherHitbox = _other.GetComponent<Hitbox>();

        hpInterface.TakeDamage(otherHitbox.Damage);
        if (otherHitbox.DestroyOnHit)
            Destroy(otherHitbox.gameObject);
    }
}
