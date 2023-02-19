using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int numMaxWeapons;
    [SerializeField] private List<Image> weaponSlots;
    [SerializeField] private TextMeshProUGUI juiceText;

    [SerializeField] private float counterChangePerSec;
    [SerializeField] private float countDownSpeedMult;

    private float displayedJuice;
    [SerializeField] private int juice;

    private List<Weapon> weapons = new List<Weapon>();

    public bool PickupWeapon(Weapon _weapon)
    {
        if (weapons.Count < numMaxWeapons)
        {
            weapons.Add(_weapon);

            weaponSlots[weapons.Count - 1].sprite = _weapon.GetComponent<SpriteRenderer>().sprite;
            weaponSlots[weapons.Count - 1].color = Color.white;

            return true;
        }

        return false;
    }

    public void AddJuice(int _value)
    {
        juice += _value;
    }

    void Update()
    {
        if (displayedJuice != juice)
        {
            float amountToChange = juice - displayedJuice;
            if (amountToChange > 0)
                amountToChange = Mathf.Min(amountToChange, counterChangePerSec * Time.deltaTime * Mathf.Max(1, Mathf.Sqrt(juice - displayedJuice)));
            else
                amountToChange = Mathf.Max(amountToChange, -counterChangePerSec * countDownSpeedMult * Time.deltaTime * Mathf.Max(1, Mathf.Sqrt(displayedJuice - juice)));

            displayedJuice += amountToChange;
            juiceText.text = $"{(int)displayedJuice}";
        }
    }
}
