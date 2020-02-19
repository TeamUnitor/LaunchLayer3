using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitorSetups : MonoBehaviour
{
    public static void SetupButtons() {
        for (int x = 1; x <= 8; x++) {
            for (int y = 1; y <= 8; y++) {
                MainProjectLoader.ctrl.Add("u" + x.ToString() + y.ToString(), GameObject.Find("u" + x.ToString() + y.ToString()).GetComponent<Button>());
            }
        }

        for (int i = 1; i <= 32; i++) {
                MainProjectLoader.ctrl.Add("mc" + i.ToString(), GameObject.Find("mc" + i.ToString()).GetComponent<Button>());
        }
    }
}
