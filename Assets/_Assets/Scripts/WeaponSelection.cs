using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponSelection : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image weaponSlotImage;
    [SerializeField] private Sprite emptySprite;

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
            weaponSlotImage.sprite = emptySprite;
            weaponSlotImage.color = new Color(1, 0, 0, 0);
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
