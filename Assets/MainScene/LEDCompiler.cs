using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LEDCompiler
{
    public static List<LEDStructure> GetLED(int chain, int x, int y, string text) {
        List<LEDStructure> a = new List<LEDStructure>();

        string[] il = MainProjectLoader.SplitByLine(text);
        for (int i = 0; i < il.Length; i++) {
            string[] sp = il[i].Split(' ');

            if (string.IsNullOrWhiteSpace(il[i]) == true) continue;

            switch(sp[0]) {
                case "o":
                case "on":

                int x_ = 0;
                int y_ = int.Parse(sp[2]);
                int velocity = 0;
                Color color = Color.gray;

                if (sp[1] == "mc") x_ = -1;
                else x_ = int.Parse(sp[1]);
                if ((sp[3] == "a" || sp[3] == "auto") && int.TryParse(sp[4], out velocity) == true)
                    ColorUtility.TryParseHtmlString("#" + ColorLibrary.Velo2HTML(velocity), out color);

                a.Add(new LEDStructure(x_, y_, color, velocity, false, chain, x, y));
                break;

                case "f":
                case "off":

                x_ = 0;
                y_ = int.Parse(sp[2]);

                if (sp[1] == "mc") x_ = -1;
                else x_ = int.Parse(sp[1]);

                a.Add(new LEDStructure(x_, y_, chain, x, y));
                break;

                case "d":
                case "delay":

                int delay = int.Parse(sp[1]);

                a.Add(new LEDStructure(delay, chain, x, y));
                break;
            }
        }

        return a;
    }
}
