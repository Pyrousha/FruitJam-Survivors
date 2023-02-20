using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponSelection : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image weaponSlotImage;

    private Weapon weapon;
    public Weapon Weapon => weapon;

    private Inventory inventory;

    public void SetWeapon(Weapon _weapon, Inventory _inventory = null)
    {
        if (_inventory != null)
            inventory = _inventory;

        weapon = _weapon;

        if (weapon == null)
        {
            weaponSlotImage.sprite = null;
            weaponSlotImage.color = Color.clear;
        }
        else
        {
            weaponSlotImage.sprite = _weapon.GetComponent<SpriteRenderer>().sprite;
            weaponSlotImage.color = Color.white;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("click");
        if (weapon != null)
            inventory.BoughtWeapon(weapon);
    }
}
