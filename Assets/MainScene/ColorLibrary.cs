using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ColorLibrary
{
    public static ColorBlock GetButtonColor(Color color, ColorBlock defaultColorBlock) { //한가지의 색을 아예 버튼에서 적용할 수 있는 ColorBlock을 반환
        ColorBlock col = defaultColorBlock;

        if (color == Color.gray) { //velocity 0
            col.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            col.highlightedColor = new Color(0.96f, 0.96f, 0.96f, 1.0f);
            col.pressedColor = new Color(0.78f, 0.78f, 0.78f, 1.0f);
            col.selectedColor = new Color(0.96f, 0.96f, 0.96f, 1.0f);
            col.disabledColor = color;
            col.fadeDuration = MainProjectLoader.fadeDuration;

            return col;
        }

        col.normalColor = color;
        col.highlightedColor = new Color(color.r, color.g, color.b);
        col.pressedColor = new Color(color.r, color.g, color.b);
        col.selectedColor = new Color(color.r, color.g, color.b);
        col.disabledColor = Color.gray;
        col.fadeDuration = MainProjectLoader.fadeDuration;

        return col;
    }

    public static string Velo2HTML(int velocity)
    {
        switch (velocity)
        {
            case 0:
                {
                    return "808080"; // Disable Color (Erase)
                }

            case 1:
                {
                    return "DADFE4";
                }

            case 2:
                {
                    return "E6E9ED";
                }

            case 3:
                {
                    return "FAFAFA";
                }

            case 4:
                {
                    return "F8BBD0";
                }

            case 5:
                {
                    return "EF5350";
                }

            case 6:
                {
                    return "E57373";
                }

            case 7:
                {
                    return "EF9A9A";
                }

            case 8:
                {
                    return "FFF3E0";
                }

            case 9:
                {
                    return "FFA726";
                }

            case 10:
                {
                    return "FFB960";
                }

            case 11:
                {
                    return "FFCC80";
                }

            case 12:
                {
                    return "FFE0B2";
                }

            case 13:
                {
                    return "FFEE58";
                }

            case 14:
                {
                    return "FFF59D";
                }

            case 15:
                {
                    return "FFF9C4";
                }

            case 16:
                {
                    return "DCEDC8";
                }

            case 17:
                {
                    return "8BC34A";
                }

            case 18:
                {
                    return "AED581";
                }

            case 19:
                {
                    return "BFDF9F";
                }

            case 20:
                {
                    return "5EE2B0";
                }

            case 21:
                {
                    return "00CE3C";
                }

            case 22:
                {
                    return "00BA43";
                }

            case 23:
                {
                    return "119C3F";
                }

            case 24:
                {
                    return "57ECC1";
                }

            case 25:
                {
                    return "00E864";
                }

            case 26:
                {
                    return "00E05C";
                }

            case 27:
                {
                    return "00D545";
                }

            case 28:
                {
                    return "7AFDDD";
                }

            case 29:
                {
                    return "00E4C5";
                }

            case 30:
                {
                    return "00E0B2";
                }

            case 31:
                {
                    return "01EEC6";
                }

            case 32:
                {
                    return "49EFEF";
                }

            case 33:
                {
                    return "00E7D8";
                }

            case 34:
                {
                    return "00E5D1";
                }

            case 35:
                {
                    return "01EFDE";
                }

            case 36:
                {
                    return "6ADDFF";
                }

            case 37:
                {
                    return "00DAFE";
                }

            case 38:
                {
                    return "01D6FF";
                }

            case 39:
                {
                    return "08ACDC";
                }

            case 40:
                {
                    return "73CEFE";
                }

            case 41:
                {
                    return "0D9BF7";
                }

            case 42:
                {
                    return "148DE4";
                }

            case 43:
                {
                    return "2A77C9";
                }

            case 44:
                {
                    return "8693FF";
                }

            case 45:
                {
                    return "2196F3";
                }

            case 46:
                {
                    return "4668F6";
                }

            case 47:
                {
                    return "4153DC";
                }

            case 48:
                {
                    return "B095FF";
                }

            case 49:
                {
                    return "8453FD";
                }

            case 50:
                {
                    return "634ACD";
                }

            case 51:
                {
                    return "5749C5";
                }

            case 52:
                {
                    return "FFB7FF";
                }

            case 53:
                {
                    return "E863FB";
                }

            case 54:
                {
                    return "D655ED";
                }

            case 55:
                {
                    return "D14FE9";
                }

            case 56:
                {
                    return "FC99E3";
                }

            case 57:
                {
                    return "E736C2";
                }

            case 58:
                {
                    return "E52FBE";
                }

            case 59:
                {
                    return "E334B6";
                }

            case 60:
                {
                    return "ED353E";
                }

            case 61:
                {
                    return "FFA726";
                }

            case 62:
                {
                    return "F4DF0B";
                }

            case 63:
                {
                    return "66BB6A";
                }

            case 64:
                {
                    return "5CD100";
                }

            case 65:
                {
                    return "00D29E";
                }

            case 66:
                {
                    return "2388FF";
                }

            case 67:
                {
                    return "3669FD";
                }

            case 68:
                {
                    return "00B4D0";
                }

            case 69:
                {
                    return "475CDC";
                }

            case 70:
                {
                    return "F2F3F5";
                }

            case 71:
                {
                    return "EEF0F2";
                }

            case 72:
                {
                    return "F72737";
                }

            case 73:
                {
                    return "D2EA7B";
                }

            case 74:
                {
                    return "C8DF10";
                }

            case 75:
                {
                    return "7FE422";
                }

            case 76:
                {
                    return "00C931";
                }

            case 77:
                {
                    return "00D7A6";
                }

            case 78:
                {
                    return "00D8FC";
                }

            case 79:
                {
                    return "0B9BFC";
                }

            case 80:
                {
                    return "585CF5";
                }

            case 81:
                {
                    return "AC59F0";
                }

            case 82:
                {
                    return "D980DC";
                }

            case 83:
                {
                    return "B8814A";
                }

            case 84:
                {
                    return "FF9800";
                }

            case 85:
                {
                    return "ABDF22";
                }

            case 86:
                {
                    return "9EE154";
                }

            case 87:
                {
                    return "66BB6A";
                }

            case 88:
                {
                    return "3BDA47";
                }

            case 89:
                {
                    return "6FDEB9";
                }

            case 90:
                {
                    return "27DBDA";
                }

            case 91:
                {
                    return "9CC8FD";
                }

            case 92:
                {
                    return "79B8F7";
                }

            case 93:
                {
                    return "AFAFEF";
                }

            case 94:
                {
                    return "D580EB";
                }

            case 95:
                {
                    return "F74FCA";
                }

            case 96:
                {
                    return "EA8A1F";
                }

            case 97:
                {
                    return "DBDB08";
                }

            case 98:
                {
                    return "9CD60D";
                }

            case 99:
                {
                    return "F3D335";
                }

            case 100:
                {
                    return "C8AF41";
                }

            case 101:
                {
                    return "00CA69";
                }

            case 102:
                {
                    return "24D2B0";
                }

            case 103:
                {
                    return "757EBE";
                }

            case 104:
                {
                    return "5388DB";
                }

            case 105:
                {
                    return "E5C5A6";
                }

            case 106:
                {
                    return "E93B3B";
                }

            case 107:
                {
                    return "F9A2A1";
                }

            case 108:
                {
                    return "ED9C65";
                }

            case 109:
                {
                    return "E1CA72";
                }

            case 110:
                {
                    return "B8DA78";
                }

            case 111:
                {
                    return "98D52C";
                }

            case 112:
                {
                    return "626CBD";
                }

            case 113:
                {
                    return "CAC8A0";
                }

            case 114:
                {
                    return "90D4C2";
                }

            case 115:
                {
                    return "CEDDFE";
                }

            case 116:
                {
                    return "BECCF7";
                }

            case 117:
                {
                    return "D2D7DE";
                }

            case 118:
                {
                    return "DADFE4";
                }

            case 119:
                {
                    return "E6E9ED";
                }

            case 120:
                {
                    return "FE1624";
                }

            case 121:
                {
                    return "CD2724";
                }

            case 122:
                {
                    return "9CCC65";
                }

            case 123:
                {
                    return "009C1B";
                }

            case 124:
                {
                    return "FFFF00";
                }

            case 125:
                {
                    return "BEB212";
                }

            case 126:
                {
                    return "F5D01D";
                }

            case 127:
                {
                    return "E37829";
                }
        }

        return "FFFFFF";
    }
}
