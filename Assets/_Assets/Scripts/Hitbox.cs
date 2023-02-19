using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private bool destroyOnHit;
    public bool DestroyOnHit => destroyOnHit;
    [SerializeField] private float damage;
    public float Damage => damage;
    public void SetDamage(float _dmg)
    {
        damage = _dmg;
    }
}
