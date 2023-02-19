using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float startY;

    private bool active = false;

    private PlayerController player;

    [System.Serializable]
    enum TargetEnum
    {
        ClosestEnemy,
        RandomDirection,
        PlayerVelocity
    }

    private float nextFireTime = -1;
    [SerializeField] private float cooldown;
    [SerializeField] private TargetEnum targetType;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Parameters")]
    [SerializeField] private float projectileMoveSpeed;
    [SerializeField] private float projectileLifespan;

    void Awake()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            Vector3 newPos = transform.position;
            newPos.y = Mathf.Sin(Time.time * 4) * 0.25f + startY;

            transform.position = newPos;
            return;
        }

        if (Time.time >= nextFireTime)
            Fire();
    }

    //Spawns the corresponding projectile for this weapon
    private void Fire()
    {
        Vector3 dir = new Vector3();
        switch (targetType)
        {
            case TargetEnum.ClosestEnemy:
                dir = EnemySpawner.Instance.FindDirectionToNearestEnemy(transform.position);
                break;

            case TargetEnum.RandomDirection:
                float angle = Random.Range(0, Mathf.PI * 2);
                dir = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
                break;

            case TargetEnum.PlayerVelocity:
                dir = player.DirFacing;
                break;
        }

        Projectile newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
        newProjectile.SetParameters(dir.normalized, projectileMoveSpeed, projectileLifespan);

        nextFireTime = Time.time + cooldown;
    }

    void OnTriggerEnter2D(Collider2D _player)
    {
        if (_player.GetComponent<Inventory>().PickupWeapon(this) == false)
            return;

        active = true;

        GetComponent<SpriteRenderer>().enabled = false;

        transform.parent = _player.transform;
        transform.localPosition = Vector3.zero;

        player = _player.GetComponent<PlayerController>();

        Fire();
    }
}
