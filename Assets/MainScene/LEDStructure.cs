using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDStructure
{
    public int feat;
    public int x;
    public int y;
    public Color color;
    public int velocity;
    public int delay = 0;
    public bool pulseMode;
    public int manage_chain;
    public int manage_x;
    public int manage_y;

    public LEDStructure(int x_, int y_, Color color_, int velocity_, bool pulseMode_, int manage_chain_, int manage_x_, int manage_y_) { //Command On
        feat = 0;
        x = x_;
        y = y_;
        velocity = velocity_;
        if (velocity != 0) color = color_;
        else color = Color.gray;
        pulseMode = pulseMode_;
        manage_chain = manage_chain_;
        manage_x = manage_x_;
        manage_y = manage_y_;
    }

    public LEDStructure(int x_, int y_, int manage_chain_, int manage_x_, int manage_y_) { //Command Off
        feat = 1;
        x = x_;
        y = y_;
        velocity = 0;
        color = Color.gray;
        manage_chain = manage_chain_;
        manage_x = manage_x_;
        manage_y = manage_y_;
    }

    public LEDStructure(int delay_, int manage_chain_, int manage_x_, int manage_y_) { //Command Dleay
        feat = 2;
        delay = delay_;
        manage_chain = manage_chain_;
        manage_x = manage_x_;
        manage_y = manage_y_;
    }
}
