using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuicePickup : MonoBehaviour
{
    [SerializeField] private int value;
    private float startingVelocity = 5;

    void Start()
    {
        float angle = Random.Range(Mathf.PI / 4, 3 * Mathf.PI / 4);
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * startingVelocity;
    }

    void OnTriggerEnter2D(Collider2D _col)
    {
        _col.GetComponent<Inventory>().AddJuice(value);
        Destroy(transform.parent.gameObject);
    }
}
