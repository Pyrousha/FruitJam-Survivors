using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juicer : MonoBehaviour
{
    InputHandler player;
    bool playerOverlapping = false;

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
                GetComponent<Collider2D>().enabled = false;

                player.GetComponent<Inventory>().InventoryUI.SetTrigger("OpenMenu");

                Time.timeScale = 0;
            }
            else
            {
                if (player.Menu.down)
                {
                    player.GetComponent<Inventory>().InventoryUI.SetTrigger("CloseMenu");
                    Destroy(gameObject);

                    Time.timeScale = 1;
                }
            }
        }
    }
}
