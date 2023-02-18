using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int numMaxWeapons;
    [SerializeField] private List<Image> weaponSlots;

    private List<Weapon> weapons = new List<Weapon>();

    public bool PickupItem(Weapon _weapon)
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
}
