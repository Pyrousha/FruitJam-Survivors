using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [System.Serializable]
    public struct WeaponStats
    {
        public TargetEnum targetType;

        public float xpNeededToLevelUp;

        public float damage;

        public float cooldownBetweenShots;
        public int projectilesFired;

        public float projectileMoveSpeed;
        public float projectileLifespan;
    }

    private float startY;

    private bool active = false;

    private PlayerController player;

    private int level = 0;
    private float xp;
    private float maxXp;
    public float XpToNextLevel => maxXp - xp;

    private InventorySlot equippedSlot;

    [System.Serializable]
    public enum TargetEnum
    {
        ClosestEnemy,
        RandomDirection,
        PlayerVelocity
    }

    private float nextFireTime = -1;

    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private List<WeaponStats> statsAsWeaponLevelsUp;

    private WeaponStats currStats;


    void Awake()
    {
        startY = transform.position.y;

        OnLevelUp();
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
        switch (currStats.targetType)
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
        newProjectile.SetParameters(dir.normalized, currStats.projectileMoveSpeed, currStats.projectileLifespan, currStats.damage);

        nextFireTime = Time.time + currStats.cooldownBetweenShots;
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

    public void AddXp(float _xpToAdd)
    {
        xp += _xpToAdd;
        if (xp >= maxXp)
            OnLevelUp();

        equippedSlot.UpdateSlider(xp / maxXp);
    }

    private void OnLevelUp()
    {
        if (level == statsAsWeaponLevelsUp.Count)
            return;

        xp -= maxXp;
        level++;

        if (level == statsAsWeaponLevelsUp.Count)
        {
            //max level reached
            xp = maxXp;
            return;
        }

        currStats = statsAsWeaponLevelsUp[level - 1];

        maxXp = currStats.xpNeededToLevelUp;
    }

    public void SetSlot(InventorySlot _slot)
    {
        equippedSlot = _slot;
    }
}
