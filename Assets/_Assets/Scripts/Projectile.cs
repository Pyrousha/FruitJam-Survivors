using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;

    private float destroyTime = -1;

    [SerializeField] private LayerMask terrainLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Time.time >= destroyTime)
            Destroy(gameObject);
    }

    public void SetParameters(Vector2 _dir, float _moveSpeed, float _duration)
    {
        Vector3 rot = transform.eulerAngles;
        rot.z = Mathf.Atan2(_dir.y, _dir.x);
        transform.eulerAngles = rot * Mathf.Rad2Deg;

        rb.velocity = _dir * _moveSpeed;

        destroyTime = Time.time + _duration;
    }

    //Destroy projectile if it hits terrain
    void OnCollisionEnter2D(Collision2D _col)
    {
        if (terrainLayer == (terrainLayer | (1 << _col.gameObject.layer)))
            return;

        Destroy(gameObject);
    }
}