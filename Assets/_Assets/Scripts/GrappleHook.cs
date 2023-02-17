using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    enum GrappleState_Enum
    {
        Inactive,
        Firing,
        Attached,
        Retracting
    }

    private GrappleState_Enum grappleState;

    private Rigidbody2D rb;
    private Collider2D col;
    [SerializeField] private PlayerController player;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float fireDuration;

    [SerializeField] private GameObject visuals;
    [SerializeField] private LineRenderer lineRend;

    private float timer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void Fire(Vector2 _dir)
    {
        if (grappleState != GrappleState_Enum.Inactive)
            return;

        grappleState = GrappleState_Enum.Firing;

        transform.position = player.transform.position;

        col.enabled = true;

        timer = fireDuration;
        rb.velocity = _dir.normalized * moveSpeed;
        visuals.SetActive(true);
    }

    void Update()
    {
        switch (grappleState)
        {
            case GrappleState_Enum.Inactive:
                {
                    break;
                }
            case GrappleState_Enum.Firing:
                {
                    UpdateLineRend();

                    if (timer > 0)
                    {
                        timer -= Time.deltaTime;

                        if (timer <= 0)
                            Retract();
                    }

                    break;
                }
            case GrappleState_Enum.Attached:
                {
                    UpdateLineRend();

                    break;
                }
            case GrappleState_Enum.Retracting:
                {
                    UpdateLineRend();

                    Vector3 toPlayer = player.transform.position - transform.position;

                    if (toPlayer.magnitude < 5f)
                    {
                        //Close enough to player
                        grappleState = GrappleState_Enum.Inactive;

                        visuals.SetActive(false);
                    }
                    else
                    {
                        rb.velocity = moveSpeed * toPlayer.normalized * 2;
                    }

                    break;
                }
        }
    }

    public void Retract()
    {
        if (grappleState != GrappleState_Enum.Firing && grappleState != GrappleState_Enum.Attached)
            return;

        col.enabled = false;

        grappleState = GrappleState_Enum.Retracting;
        player.SetIsGrappling(false);
    }

    /// <summary>
    /// Called when the grappling hook hits terrain while firing out
    /// </summary>
    /// <param name="_col"></param>
    void OnCollisionEnter2D(Collision2D _col)
    {
        if (grappleState != GrappleState_Enum.Firing)
            return;

        grappleState = GrappleState_Enum.Attached;
        rb.velocity = Vector2.zero;

        transform.position = _col.otherCollider.ClosestPoint(transform.position);

        player.SetIsGrappling(true);

    }


    void UpdateLineRend()
    {
        lineRend.SetPosition(0, transform.position);
        lineRend.SetPosition(1, player.transform.position);
    }
}
