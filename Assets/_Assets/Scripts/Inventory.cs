using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    private List<Weapon> weapons = new List<Weapon>();

    [SerializeField] private int numMaxWeapons;
    [SerializeField] private List<InventorySlot> weaponSlots;
    [SerializeField] private List<WeaponSelection> weaponSelections;
    [SerializeField] private TextMeshProUGUI juiceText;
    [SerializeField] private Animator inventoryUI;
    public Animator InventoryUI => inventoryUI;

    [Space(5)]
    [SerializeField] private float counterChangePerSec;
    [SerializeField] private float countDownSpeedMult;

    [Space(5)]
    [SerializeField] private float xpPercentAddedPerSecond;
    [Space(5)]
    [SerializeField] private List<Weapon> weaponsOnMap;

    [Space(5)]
    [SerializeField] private TextMeshProUGUI selectNewWeaponText;
    [SerializeField] private List<Image> weaponSelectionImages;

    private float displayedJuice;
    private float juice;

    private Camera mainCamera;

    private bool filling;
    private Weapon selectedWeapon;

    private InputHandler inputHandler;

    public bool AllWeaponsMaxLevel()
    {
        if (weapons.Count != numMaxWeapons)
            return false;

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

    void Start()
    {
        DisableBuyMenu();
    }

    public List<Weapon> GetNotPickedUpWeapons(int _numMaxWeapons)
    {
        if (weapons.Count == numMaxWeapons)
        {
            Debug.Log("already full!");
            return new List<Weapon>();

        }
        //Do the shuuuuuffle

        Debug.Log(weaponsOnMap.Count);

        // Loops through array
        for (int i = weaponsOnMap.Count - 1; i > 0; i--)
        {
            // Randomize a number between 0 and i (so that the range decreases each time)
            int rnd = Random.Range(0, i);

            // Save the value of the current i, otherwise it'll overright when we swap the values
            (weaponsOnMap[rnd], weaponsOnMap[i]) = (weaponsOnMap[i], weaponsOnMap[rnd]);
        }

        Debug.Log(weaponsOnMap.Count);

        List<Weapon> toReturn = new List<Weapon>();
        int wepsToGet = Mathf.Min(_numMaxWeapons, weaponsOnMap.Count);
        for (int i = 0; i < wepsToGet; i++)
            toReturn.Add(weaponsOnMap[i]);

        Debug.Log("arr: " + toReturn.Count);

        return toReturn;
    }

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
        if (filling)
        {
            float amountToFill = Mathf.Min(xpPercentAddedPerSecond * selectedWeapon.MaxXp * Time.unscaledDeltaTime, juice, selectedWeapon.XpToNextLevel);
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

    private List<Weapon> amongulungus;

    private Juicer juicer;
    internal void SetJuicer(Juicer _juicer)
    {
        juicer = _juicer;

        foreach (WeaponSelection steven in weaponSelections)
        {
            steven.SetWeapon(null);
        }

        List<Weapon> weaponsToPurchace = GetNotPickedUpWeapons(numMaxWeapons);
        amongulungus = weaponsToPurchace;
        if (weaponsToPurchace.Count > 0)
        {
            //show weapons to buy
            selectNewWeaponText.color = Color.white;
            foreach (Image img in weaponSelectionImages)
            {
                img.color = Color.white;
            }

            for (int i = 0; i < weaponsToPurchace.Count; i++)
            {
                Debug.Log(i);
                Weapon currWeapon = weaponsToPurchace[i];
                weaponSelections[i].SetWeapon(currWeapon, this);
            }
        }
        else
        {
            DisableBuyMenu();
        }
    }

    public void BoughtWeapon(Weapon _weapon)
    {
        weaponsOnMap.Remove(_weapon);

        _weapon.transform.position = transform.position + new Vector3(0, -1, 0);
        _weapon.gameObject.SetActive(true);
        juicer.CloseMenu();
    }

    public void DisableBuyMenu()
    {
        //hide buying ui

        foreach (WeaponSelection steven in weaponSelections)
        {
            steven.SetWeapon(null);
        }

        selectNewWeaponText.color = Color.clear;
        foreach (Image img in weaponSelectionImages)
        {
            img.color = Color.clear;
        }
    }
}
