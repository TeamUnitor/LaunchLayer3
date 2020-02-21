using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadInfoStructure
{
    public int chain;
    public int x;
    public int y;
    public int touchMode;
    public PadInfoStructure(int chain_, int x_, int y_, int touchMode_) {
        chain = chain_;
        x = x_;
        y = y_;
        touchMode = touchMode_;
    }
}
