using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleLine : MonoBehaviour
{
    [SerializeField] private Transform player;
    private LineRenderer lineRend;

    void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRend.SetPosition(0, transform.position);
        lineRend.SetPosition(1, player.position);
    }
}
