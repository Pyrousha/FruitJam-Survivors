using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float degreesPerSecond;

    // Update is called once per frame
    void Update()
    {
        Vector3 newRotation = transform.eulerAngles;
        newRotation.z += degreesPerSecond * Time.deltaTime;

        transform.eulerAngles = newRotation;
    }
}
