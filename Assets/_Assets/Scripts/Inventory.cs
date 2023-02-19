using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int numMaxWeapons;
    [SerializeField] private List<InventorySlot> weaponSlots;
    [SerializeField] private TextMeshProUGUI juiceText;
    [SerializeField] private Animator inventoryUI;
    public Animator InventoryUI => inventoryUI;

    [Space(5)]
    [SerializeField] private float counterChangePerSec;
    [SerializeField] private float countDownSpeedMult;

    [Space(5)]
    [SerializeField] private float xpAddedPerSec;

    private float displayedJuice;
    private float juice;

    private Camera mainCamera;

    private bool filling;
    private Weapon selectedWeapon;

    private InputHandler inputHandler;

    public bool AllWeaponsMaxLevel()
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.IsMaxLevel == false)
                return false;
        }

        return true;
    }

    void Awake()
    {
        mainCamera = Camera.main;
        inputHandler = GetComponent<InputHandler>();
    }

    private List<Weapon> weapons = new List<Weapon>();

    public bool PickupWeapon(Weapon _weapon)
    {
        if (weapons.Count < numMaxWeapons)
        {
            weapons.Add(_weapon);

            weaponSlots[weapons.Count - 1].SetWeapon(_weapon);
            weaponSlots[weapons.Count - 1].SetInventory(this);

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
        if (inputHandler.Grapple.up)
            StopFill();

        if (filling)
        {
            float amountToFill = Mathf.Min(xpAddedPerSec * Time.unscaledDeltaTime, juice, selectedWeapon.XpToNextLevel);
            juice -= amountToFill;

            if (amountToFill == 0)
                StopFill();
            else
                selectedWeapon.AddXp(amountToFill);
        }

        //Roll fruits counter
        if (displayedJuice != juice)
        {
            float amountToChange = juice - displayedJuice;
            if (amountToChange > 0)
                amountToChange = Mathf.Min(amountToChange, counterChangePerSec * Time.unscaledDeltaTime * Mathf.Max(1, Mathf.Sqrt(juice - displayedJuice)));
            else
                amountToChange = Mathf.Max(amountToChange, -counterChangePerSec * countDownSpeedMult * Time.unscaledDeltaTime * Mathf.Max(1, Mathf.Sqrt(displayedJuice - juice)));

            displayedJuice += amountToChange;
            juiceText.text = $"{(int)displayedJuice}";
        }
    }

    public void StartFill(Weapon _weapon)
    {
        selectedWeapon = _weapon;
        filling = true;
    }

    void StopFill()
    {
        filling = false;
        selectedWeapon = null;

        if (juice == 0 || AllWeaponsMaxLevel())
            juicer.CloseMenu();
    }

    private Juicer juicer;
    internal void SetJuicer(Juicer _juicer)
    {
        juicer = _juicer;
    }
}
