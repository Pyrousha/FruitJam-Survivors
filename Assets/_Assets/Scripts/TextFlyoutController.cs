using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFlyoutController : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [Header("Stats")]
    [SerializeField] private float lifetime;
    [SerializeField] private AnimationCurve sizeOverLifetime;
    [SerializeField] private AnimationCurve alphaOverLifetime;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<DestroyAfterDelay>().SetLifetime(lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        Color color = text.color;
        color.a = alphaOverLifetime.Evaluate(timer/lifetime);
        transform.localScale = sizeOverLifetime.Evaluate(timer/lifetime) * Vector3.one;
        text.color = color;
        timer += Time.deltaTime;

        transform.position += Time.deltaTime *  0.5f * Vector3.up;
    }

    public void Init(string text) {
        this.text.text = text;
    }
}
