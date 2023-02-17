using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public struct ButtonState
    {
        private bool firstFrame;
        public bool holding { get; private set; }
        public bool down
        {
            get
            {
                return holding && firstFrame;
            }
        }
        public bool up
        {
            get
            {
                return !holding && firstFrame;
            }
        }

        public void Set(InputAction.CallbackContext ctx)
        {
            holding = !ctx.canceled;
            firstFrame = true;
        }
        public void Reset()
        {
            firstFrame = false;
        }
    }

    //Movement Buttons
    private ButtonState up;
    public ButtonState Up => up;

    private ButtonState down;
    public ButtonState Down => down;

    private ButtonState left;
    public ButtonState Left => left;

    private ButtonState right;
    public ButtonState Right => right;

    private ButtonState jump;
    public ButtonState Jump => jump;

    //Combat Buttons
    private ButtonState grapple;
    public ButtonState Grapple => grapple;

    private ButtonState parry;
    public ButtonState Parry => parry;

    //Interaction/Misc buttons
    private ButtonState interact;
    public ButtonState Interact => interact;

    private ButtonState menu;
    public ButtonState Menu => menu;

    private ButtonState map;
    public ButtonState Map => map;

    // Update is called once per frame
    void LateUpdate()
    {
        direction = GetDirection();

        //Rest direction buttons
        up.Reset();
        down.Reset();
        left.Reset();
        right.Reset();

        //reset input stuff
        jump.Reset();
        grapple.Reset();
        parry.Reset();

        interact.Reset();
        menu.Reset();
        map.Reset();
    }

    private Vector2 GetDirection()
    {
        float x = 0;
        float y = 0;

        if (up.holding)
            y++;
        if (down.holding)
            y--;

        if (left.holding)
            x--;
        if (right.holding)
            x++;

        Vector2 dir = new Vector2(x, y);

        if (dir.magnitude > 0.10f) //Player is definitely pressing something
            return dir.normalized;

        return new Vector2(0, 0);
    }

    private Vector2 direction;
    public Vector2 Direction => direction;

    //Set movement
    public void Button_Up(InputAction.CallbackContext ctx)
    {
        up.Set(ctx);
    }
    public void Button_Down(InputAction.CallbackContext ctx)
    {
        down.Set(ctx);
    }
    public void Button_Left(InputAction.CallbackContext ctx)
    {
        left.Set(ctx);
    }
    public void Button_Right(InputAction.CallbackContext ctx)
    {
        right.Set(ctx);
    }
    public void Button_Jump(InputAction.CallbackContext ctx)
    {
        jump.Set(ctx);
    }

    //Set Buttons
    public void Button_Grapple(InputAction.CallbackContext ctx)
    {
        grapple.Set(ctx);
    }
    public void Button_Parry(InputAction.CallbackContext ctx)
    {
        parry.Set(ctx);
    }

    public void Button_Interact(InputAction.CallbackContext ctx)
    {
        interact.Set(ctx);
    }
    public void Button_Menu(InputAction.CallbackContext ctx)
    {
        menu.Set(ctx);
    }
    public void Button_Map(InputAction.CallbackContext ctx)
    {
        map.Set(ctx);
    }
}
