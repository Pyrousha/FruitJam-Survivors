using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpInterface : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] protected float maxHp;
    protected float currHp;
    public GameObject gameOverObj;

    void Awake()
    {
        currHp = maxHp;
        UpdateHPBar();
    }

    public virtual void TakeDamage(float _dmg)
    {
        currHp = Mathf.Max(0, currHp - _dmg);
        UpdateHPBar();

        if (currHp >= 0)
            VFXManager.Instance.CreateTextFlyout(_dmg.ToString(), transform.position);

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

    protected void UpdateHPBar()
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
        gameOverObj.SetActive(true);
        Time.timeScale = 0;
    }

    public override void TakeDamage(float _dmg)
    {
        base.TakeDamage(_dmg);
        StyleManager.Instance.ChangeStyle(-100);
    }

    void FixedUpdate()
    {
        if (currHp < maxHp)
        {
            currHp += StyleManager.Instance.GetGrade() * 0.02f;

            if (currHp > maxHp)
                currHp = maxHp;
            this.UpdateHPBar();
        }
    }
}
