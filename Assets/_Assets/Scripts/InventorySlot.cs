using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image weaponSlotImage;
    [SerializeField] private Slider xpSlider;

    private Weapon weapon;
    public Weapon Weapon => weapon;

    public void SetWeapon(Weapon _weapon)
    {
        weapon = _weapon;

        weaponSlotImage.sprite = _weapon.GetComponent<SpriteRenderer>().sprite;
        weaponSlotImage.color = Color.white;

        weapon.SetSlot(this);

        xpSlider.gameObject.SetActive(true);
    }

    public void UpdateSlider(float _xpPercent)
    {
        xpSlider.value = _xpPercent;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (weapon != null)
            inventory.StartFill(weapon);
    }

    private Inventory inventory;
    public void SetInventory(Inventory _inventory)
    {
        inventory = _inventory;
    }
}
