using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : HpInterface
{
    [SerializeField] private int numXpSpawned;
    [SerializeField] private GameObject xpPickup;
    public override void Die()
    {
        for (int i = 0; i < numXpSpawned; i++)
        {
            Instantiate(xpPickup, transform.position, Quaternion.identity);
        }

        EnemySpawner.Instance.OnEnemyKilled(this);

        Destroy(gameObject);
    }
}
