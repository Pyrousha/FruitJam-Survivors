using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : HpInterface
{
    [SerializeField] private int numXpSpawned;
    [SerializeField] private GameObject xpPickup;
    bool dead = false;
    public override void Die()
    {
        if (dead)
            return;

        dead = true;

        for (int i = 0; i < numXpSpawned; i++)
        {
            Instantiate(xpPickup, transform.position, Quaternion.identity);
        }
        StyleManager.Instance.ChangeStyle(25);

        EnemySpawner.Instance.OnEnemyKilled(this);

        Destroy(gameObject);
    }
}
