using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum WeaponType
    {
        Wand,
        Boomerang,
        Shotgun
    }

    private Rigidbody2D rb;

    private float destroyTime = -1;

    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private Hitbox hitbox;
    [SerializeField] private WeaponType weaponType;

    [Space(5)]
    [SerializeField] private float angleChangePerSec;
    private Transform target;
    private bool targeting;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        switch (weaponType)
        {
            case WeaponType.Wand:
                break;
            case WeaponType.Boomerang:
                Invoke("StartTargeting", 0.25f);
                break;
        }
    }

    void Update()
    {
        if (Time.time >= destroyTime)
            Destroy(gameObject);

        if (targeting)
        {
            Vector3 targetDir = (target.position - transform.position).normalized;
            if (Vector3.Cross(rb.velocity.normalized, targetDir).magnitude < 0.1f)
            {
                targetDir += new Vector3(0, 10, 0);
                targetDir.Normalize();
            }
            Vector3 newDir = Vector3.RotateTowards(rb.velocity.normalized, targetDir, angleChangePerSec * Time.deltaTime, 180).normalized;

            float velocity = rb.velocity.magnitude;
            rb.velocity = newDir * velocity;
        }
    }

    private void StartTargeting()
    {
        targeting = true;
    }

    public void SetParameters(Vector2 _dir, float _moveSpeed, float _duration, float _dmg, Transform _player)
    {
        hitbox.SetDamage(_dmg);

        Vector3 rot = transform.eulerAngles;
        rot.z = Mathf.Atan2(_dir.y, _dir.x);
        transform.eulerAngles = rot * Mathf.Rad2Deg;

        rb.velocity = _dir * _moveSpeed;

        destroyTime = Time.time + _duration;

        target = _player;
    }

    //Destroy projectile if it hits terrain
    void OnCollisionEnter2D(Collision2D _col)
    {
        if (Utils.IsInLayermask(_col.gameObject.layer, terrainLayer))
            Destroy(gameObject);
    }
}
