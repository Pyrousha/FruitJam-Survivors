using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerController targetPlayer;

    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;

    void Awake()
    {
        targetPlayer = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //predictive
        //rb.velocity = (targetPlayer.transform.position + new Vector3(targetPlayer.RB.velocity.x, targetPlayer.RB.velocity.y, 0) - transform.position).normalized * moveSpeed;

        //standard
        rb.velocity = (targetPlayer.transform.position - transform.position).normalized * moveSpeed;

    }
}
