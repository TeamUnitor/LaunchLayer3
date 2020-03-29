using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AutoPlayMainHandle {
    public string feat;
    public int chain;
    public int x;
    public int y;
    public int delay;
    public AutoPlayMainHandle(string feat, int chain, int x, int y, int delay) {
        this.feat = feat;
        this.chain = chain;
        this.x = x;
        this.y = y;
        this.delay = delay;
    }
}
