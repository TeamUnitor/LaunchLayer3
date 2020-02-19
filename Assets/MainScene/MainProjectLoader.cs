using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.UI;
using NAudio.Midi;

public class MainProjectLoader : MonoBehaviour
{
#region "keySound"
    public static Dictionary<string, string> keySound_Library;
    public static string[,,,] keySound_Name;
    public static int[,,] keySound_Maximum;
    public static int[,,] keySound_Index;
    public static int[,,,] keySound_Loop;
    public static int[,,,] keySound_Wormhole;

    public static string[,,] keySound_MC_Name;
    public static int[,] keySound_MC_Maximum;
    public static int[,] keySound_MC_Index;
    public static int[,,] keySound_MC_Loop;
    public static int[,,] keySound_MC_Wormhole;
#endregion
#region "keyLED"
    public static List<LEDStructure>[,,,] LED_Scripts;
    public static int[,,,] LED_Loop;
    public static int[,,] LED_Maximum;
    public static int[,,] LED_Index;

    public static List<LEDStructure>[,,] LED_MC_Scripts;
    public static int[,,] LED_MC_Loop;
    public static int[,] LED_MC_Maximum;
    public static int[,] LED_MC_Index;
#endregion
public static List<LEDStructure> LEDThreadQueue;
public static List<LEDStructure> LEDMCThreadQueue;
public static float fadeDuration = 0.001f;

public static string unipackPath = string.Empty;
public static bool startAtLoad = false;

public static Dictionary<string, Button> ctrl = new Dictionary<string, Button>();
public static SoundManager soundManager;
public static int UniPack_SelectedChain = 1;
public static int UniPack_Chains = 1;

    // Start is called before the first frame update
    void Start()
    {
        keySound_Library = new Dictionary<string, string>();
        keySound_Name = new string[25, 9, 9, 152];
        keySound_Maximum = new int[25, 9, 9];
        keySound_Index = new int[25, 9, 9];
        keySound_Loop = new int[25, 9, 9, 152];
        keySound_Wormhole = new int[25, 9, 9, 152];

        keySound_MC_Name = new string[25, 33, 152];
        keySound_MC_Maximum = new int[25, 33];
        keySound_MC_Index = new int[25, 33];
        keySound_MC_Loop = new int[25, 33, 152];
        keySound_MC_Wormhole = new int[25, 33, 152];

        LED_Scripts = new List<LEDStructure>[25, 9, 9, 28];
        LED_Loop = new int[25, 9, 9, 28];
        LED_Maximum = new int[25, 9, 9];
        LED_Index = new int[25, 9, 9];

        LED_MC_Scripts = new List<LEDStructure>[25, 33, 28];
        LED_MC_Loop = new int[25, 33, 28];
        LED_MC_Maximum = new int[25, 33];
        LED_MC_Index = new int[25, 33];

        LEDThreadQueue = new List<LEDStructure>();
        LEDMCThreadQueue = new List<LEDStructure>();

        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();


        UnitorSetups.SetupButtons();

        if (startAtLoad == true) {
            LoadPack(unipackPath);
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<LEDStructure> LEDThreadQueue_RL = new List<LEDStructure>();
        List<LEDStructure> LEDMCThreadQueue_RL = new List<LEDStructure>();

        //LED Thread Worker
        for (int i = 0; i < LEDThreadQueue.Count; i++) {
            LEDStructure item = LEDThreadQueue[i];
            if (Mathf.RoundToInt(Time.time * 1000) >= item.delay) {
            
                switch(item.feat) {
                    case 0: //On
                        ctrl["u" + item.x.ToString() + item.y.ToString()].colors = ColorLibrary.GetButtonColor(item.color, ctrl["u" + item.x.ToString() + item.y.ToString()].colors);
                        break;
                    case 1: //Off
                        ctrl["u" + item.x.ToString() + item.y.ToString()].colors = ColorLibrary.GetButtonColor(Color.gray, ctrl["u" + item.x.ToString() + item.y.ToString()].colors);
                        break;
                    case 3: //LED Wormhole
                        break;
                }

                LEDThreadQueue_RL.Add(item);
            }
        }

        for (int i = 0; i < LEDThreadQueue_RL.Count; i++) {
            LEDThreadQueue.Remove(LEDThreadQueue_RL[i]);
        }
        LEDThreadQueue_RL.Clear();



        //MC LED Thread Worker
        for (int i = 0; i < LEDMCThreadQueue.Count; i++) {
            LEDStructure item = LEDMCThreadQueue[i];
            if (Mathf.RoundToInt(Time.time * 1000) >= item.delay) {
            
                switch(item.feat) {
                    case 0: //On
                        ctrl["mc" + item.y.ToString()].colors = ColorLibrary.GetButtonColor(item.color, ctrl["mc" + item.y.ToString()].colors);
                        break;
                    case 1: //Off
                        ctrl["mc" + item.y.ToString()].colors = ColorLibrary.GetButtonColor(Color.gray, ctrl["mc" + item.y.ToString()].colors);
                        break;
                }

                LEDMCThreadQueue_RL.Add(item);
            }
        }

        for (int i = 0; i < LEDMCThreadQueue_RL.Count; i++) {
            LEDMCThreadQueue.Remove(LEDMCThreadQueue_RL[i]);
        }
        LEDMCThreadQueue_RL.Clear();

    }

    public void LoadPack(string path) {
        if (Directory.Exists(Application.temporaryCachePath + "/Workspace") == true)
            Directory.Delete(Application.temporaryCachePath + "/Workspace", true);
        
        ZipFile.ExtractToDirectory(path, Application.temporaryCachePath + "/Workspace");

        string[] info = File.ReadAllLines(Application.temporaryCachePath + "/Workspace/info");
        for (int i = 0; i < info.Length; i++) {
            string[] sp = info[i].Split('=');

            if (String.IsNullOrWhiteSpace(info[i]) == true) {
                continue;
            }

            if (sp.Length >= 2) {
                switch(sp[0]) {
                    case "title":
                        
                        break;
                    case "producerName":

                        break;
                    
                    case "chain":
                        UniPack_Chains = int.Parse(sp[1]);
                        break;
                }
            }
        }

        string[] keySound = File.ReadAllLines(Application.temporaryCachePath + "/Workspace/keySound");
        for (int i = 0; i < keySound.Length; i++) {
            if (String.IsNullOrWhiteSpace(keySound[i]) == true) {
                continue;
            }

            string[] sp = keySound[i].Split(' ');
            int tmp = 0;

            if (sp.Length >= 4) {
                if (int.TryParse(sp[1], out tmp) == true) {
                    int chain = int.Parse(sp[0]);
                    int x = int.Parse(sp[1]);
                    int y = int.Parse(sp[2]);
                    string soundName = sp[3];
                    string soundPath = Application.temporaryCachePath + "/Workspace/sounds/" + soundName;

                    soundManager.Add(chain, x, y, keySound_Maximum[chain, x, y], 1, soundPath);
                    if (sp.Length == 4) {
                        keySound_Loop[chain, x, y, keySound_Maximum[chain, x, y]] = 1;
                    }
                    else if (sp.Length == 5) {
                        if (int.TryParse(sp[4], out tmp) == false)
                            tmp = 1;
                    }
                    else if (sp.Length == 6) {
                        if (int.TryParse(sp[5], out tmp) == true)
                            keySound_Wormhole[chain, x, y, keySound_Maximum[chain, x, y]] = tmp;
                        if (int.TryParse(sp[4], out tmp) == false)
                            tmp = 1;
                    }
                    keySound_Loop[chain, x, y, keySound_Maximum[chain, x, y]] = tmp;
                    keySound_Maximum[chain, x, y]++;
                }
                else { //mc

                }
            }

        }

        string[] keyLED = Directory.GetFiles(Application.temporaryCachePath + "/Workspace/keyLED");
        for (int i = 0; i < keyLED.Length; i++) {
            string[] name = Path.GetFileName(keyLED[i]).Split(' ');

            int chain = int.Parse(name[0]);
            int x = 0;
            int y = int.Parse(name[2]);
            int loopNumber = int.Parse(name[3]);
            if (name[1] == "mc") x = -1;
            else x = int.Parse(name[1]);

            if (x == -1) { //mc

            }
            else {
                LED_Scripts[chain, x, y, LED_Maximum[chain, x, y]] = LEDCompiler.GetLED(chain, x, y, File.ReadAllText(keyLED[i]));
                LED_Loop[chain, x, y, LED_Maximum[chain, x, y]] = loopNumber;
                LED_Maximum[chain, x, y]++;
            }
        }
    }

    public static void Pad_VTouch(int chain, int x, int y, int touchMode) {
        if (touchMode == 1) {
            if (x == -1) { //mc
                if (y >= 9 && y <= 8 + UniPack_Chains) {
                Pad_CCChain(y - 8);
                }
            
                soundManager.Play(chain, x, y, keySound_MC_Index[chain, y]);

            }
             else {
                soundManager.Play(chain, x, y, keySound_Index[chain, x, y]);
                PrepareToQueue(chain, x, y, LED_Scripts[chain, x, y, LED_Index[chain, x, y]]);
            }
        }
        else {
            if (x == -1) {
                keySound_MC_Index[chain, y]++;
                LED_MC_Index[chain, y]++;

                if (keySound_MC_Index[chain, y] > keySound_MC_Maximum[chain, y]) {
                    keySound_MC_Index[chain, y] = 0;
                }
                if (LED_MC_Index[chain, y] > LED_MC_Maximum[chain, y]) {
                    LED_MC_Index[chain, y] = 0;
                }
            }
            else {
                keySound_Index[chain, x, y]++;
                LED_Index[chain, x, y]++;

                if(keySound_Index[chain, x, y] >= keySound_Maximum[chain, x, y]) {
                    keySound_Index[chain, x, y] = 0;
                }
                if(LED_Index[chain, x, y] >= LED_Maximum[chain, x, y]) {
                    LED_Index[chain, x, y] = 0;
                }
            }
        }
    }

    public static void Pad_CCChain(int chain) {
        if (UniPack_Chains >= chain && UniPack_SelectedChain != chain) {
            FlushMultiMappings();
            UniPack_SelectedChain = chain;
        }
    }

    public static void PrepareToQueue(int chain, int x, int y, List<LEDStructure> LED_Script) {
        List<LEDStructure> LEDs = LED_Script;
        
        if (x == -1) { //mc
        
        }
        else {
            switch(LED_Loop[chain, x, y, LED_Index[chain, x, y]]) {
                case 0:

                    break;
                case 1:
                    int totalD = Mathf.RoundToInt(Time.time * 1000); //default
                    for (int i = 0; i < LED_Script.Count; i++) {
                        if (LEDs[i].feat == 2) {
                            totalD += LEDs[i].delay;
                        }
                        else {
                            LEDs[i].delay = totalD;
                            if (LEDs[i].x != -1) LEDThreadQueue.Add(LEDs[i]);
                            else LEDMCThreadQueue.Add(LEDs[i]);
                        }
                    }
                    break;
                default:
                    totalD = Mathf.RoundToInt(Time.time * 1000);
                    for (int loopNumber = 1; loopNumber <= LED_Loop[chain, x, y, LED_Index[chain, x, y]]; loopNumber++) {
                        for (int i = 0; i < LED_Script.Count; i++) {
                            if (LEDs[i].feat == 2) {
                                totalD += LEDs[i].delay;
                            }
                            else {
                                LEDs[i].delay = totalD;
                                if (LEDs[i].x != -1) LEDThreadQueue.Add(LEDs[i]);
                                else LEDMCThreadQueue.Add(LEDs[i]);
                            }
                        }
                    }
                    break;
            }
        }
    }

    public static void Launchpad_MessageReceived(object sender, MidiInMessageEventArgs e) {

    }

    public static string[] SplitByLine(string str) {
        return str.Split(Environment.NewLine.ToCharArray());
    }

    public static void FlushMultiMappings() {
        for (int x = 1; x <= 8; x++) {
            for (int y = 1; y <= 8; y++) {
                keySound_Index[UniPack_SelectedChain, x, y] = 0;
                LED_Index[UniPack_SelectedChain, x, y] = 0;
            }
        }

        for (int i = 1; i <= 32; i++) {
            keySound_MC_Index[UniPack_SelectedChain, i] = 0;
            LED_MC_Index[UniPack_SelectedChain, i] = 0;
        }
    }
}