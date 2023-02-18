using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpInterface : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private float maxHp;
    private float currHp;

    void Awake()
    {
        currHp = maxHp;
        UpdateHPBar();
    }

    public void TakeDamage(float _dmg)
    {
        currHp = Mathf.Max(0, currHp - _dmg);
        UpdateHPBar();

        if (currHp <= 0)
        {
            Die();
        }
    }

    public void Heal(float _healing)
    {
        currHp = Mathf.Min(maxHp, currHp + _healing);
        UpdateHPBar();
    }

    private void UpdateHPBar()
    {
        if (hpBar == null)
            return;
        hpBar.value = currHp / maxHp;
    }

    public virtual void Die() { }
}

public class PlayerHp : HpInterface
{
    public override void Die()
    {
        Debug.Log("GAME OVER");
    }
}
