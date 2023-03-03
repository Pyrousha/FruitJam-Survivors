using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public virtual bool TakeDamage(float _dmg)
    {
        if (currHp <= 0)
            return false;

        currHp = Mathf.Max(0, currHp - _dmg);
        UpdateHPBar();

        if (currHp >= 0)
            VFXManager.Instance.CreateTextFlyout(_dmg.ToString(), transform.position);

        if (currHp <= 0)
        {
            Die();
        }

        return true;
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
    bool dead = false;
    [SerializeField] private InputHandler inputHandler;

    public override void Die()
    {
        dead = true;
        gameOverObj.SetActive(true);
        Time.timeScale = 0;
    }

    public override bool TakeDamage(float _dmg)
    {
        StyleManager.Instance.ChangeStyle(-100);

        return base.TakeDamage(_dmg);
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

    void Update()
    {
        if (dead && inputHandler.Interact.holding)
            SceneManager.LoadScene(0);
    }
}
