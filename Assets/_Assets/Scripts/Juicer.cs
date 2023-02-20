using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juicer : MonoBehaviour
{
    InputHandler player;
    bool playerOverlapping = false;
    bool open = false;

    void OnTriggerEnter2D(Collider2D _col)
    {
        player = _col.gameObject.GetComponent<InputHandler>();
        playerOverlapping = true;
    }

    void OnTriggerExit2D(Collider2D _col)
    {
        if (_col.gameObject.GetComponent<InputHandler>() == player)
        {
            playerOverlapping = false;
        }
    }

    void Update()
    {
        if (player != null && playerOverlapping)
        {
            if (player.Interact.down)
            {
                if (open)
                {
                    CloseMenu();
                }
                else
                {
                    if (player.GetComponent<Inventory>().AllWeaponsMaxLevel())
                        return; //Don't open juicer if player is maxed

                    OpenMenu();
                }
            }
        }
    }

    public void CloseMenu()
    {
        if (open == false)
            return;

        //close menu
        player.GetComponent<Inventory>().InventoryUI.SetTrigger("CloseMenu");
        player.GetComponent<Inventory>().DisableBuyMenu();

        Time.timeScale = 1;

        open = false;

        Destroy(gameObject);
    }

    private void OpenMenu()
    {
        if (open)
            return;

        //open menu
        GetComponent<Collider2D>().enabled = false;

        player.GetComponent<Inventory>().InventoryUI.SetTrigger("OpenMenu");
        player.GetComponent<Inventory>().SetJuicer(this);

        Time.timeScale = 0;

        open = true;
    }
}
