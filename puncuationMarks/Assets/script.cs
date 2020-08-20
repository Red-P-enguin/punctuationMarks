using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class script : MonoBehaviour
{
    public KMBombInfo bomb;
    public KMAudio audio;

    static int ModuleIdCounter = 1;
    int ModuleId;
    private bool moduleSolved = false;
    private bool incorrect = false;

    public KMSelectable moduleButton;
    public KMSelectable[] logicButtons;
    public KMSelectable[] wireSelectables;
    public KMSelectable[] colorButtons;
    public KMSelectable[] punctuationButtons;
    public KMSelectable[] pianoButtons;
    public KMSelectable[] messageButtons;
    public KMSelectable messagePlayButton;

    //glitch effects
    public Color[] glitchColors;
    public SpriteRenderer[] glitchSquares;
    private bool bombStarted = true;
    //memory banks
    private int memoryBankNumber = 0;
    public TextMesh memoryBankText;
    private string memoryBanks =
        "rioygcbpmw" +
        "imwcrypgob" +
        "wbymogirpc" +
        "orbpiwcmgy" +
        "pcibmroywg" +
        "gymowbricp" +
        "cgrwpmyobi" +
        "mopgciwbyr" +
        "ywgrbpmcio" +
        "bpciyogwrm"; //(r)ed p(i)nk (o)range (y)ellow (g)reen (c)yan (b)lue (p)urple (m)agenta (w)hite
    //starting stuff
    private bool startingSoundEnabled = true;
    private bool moduleDetermined = false;
    private bool memoryBankSeen = false;
    private char memoryBankColor;
    private int memoryBankColumn = 0;
    public GameObject moduleButtonGameObject;
    //logic dive
    private bool logicDiveActive = false;
    public GameObject logicDiveGameObject;
    public Material[] logicDiveMats;
    private List<Material> logicDiveNotCorrectMats = new List<Material>();
    private int logicDiveCorrectButtonNum = 0;
    private int logicDiveIndex = 0;
    public Renderer[] logicDiveButtons;
    //general second-half module stuff
    private int whichModule = 0;
    public Material[] notMemoryBankMats;
    private string[] moduleVoids = { "threeWires", "coloredButtons", "puncuationButtons", "coloredPiano", "colorfulMessage" };
    //three wires
    public Renderer[] wires;
    public GameObject threeWiresGameObject;
    public Mesh[] wireCondition;
    private string[] wirescolors = { "Crimson", "Brown", "Dark Yellow", "Green", "Blue", "Magenta" };
    private string[,] wireCombinations = new string[6, 10] { { "123", "132", "213", "231", "312", "213", "123", "312", "132", "321" },
                                          { "132", "321", "123", "312", "213", "231", "321", "132", "123", "231" },
                                          { "213", "123", "312", "321", "231", "132", "312", "231", "213", "123" },
                                          { "231", "312", "132", "213", "123", "321", "231", "123", "321", "213" },
                                          { "312", "231", "321", "132", "321", "123", "213", "321", "312", "132" },
                                          { "321", "213", "231", "123", "132", "312", "132", "213", "231", "312" } };
    private int wireIndex = 0;
    private int nextWire = 0;
    private bool[] cutWires = { false, false, false };
    //colored buttons
    public GameObject coloredButtonsGameObject;
    public Renderer[] coloredButtonss;
    private int[,] firstButtonPresses = { { 4, 5, 3, 6, 1, 2, 5, 6, 3, 4 }, { 4, 1, 6, 2, 2, 1, 5, 2, 6, 6 }, { 4, 5, 5, 4, 3, 3, 5, 3, 6, 1 }, { 5, 5, 2, 6, 5, 2, 5, 4, 4, 4 }, { 2, 6, 2, 3, 1, 4, 1, 1, 6, 6 }, { 1, 3, 3, 1, 2, 4, 6, 2, 3, 3 } };
    private int[,] secondButtonPresses = { { 3, 4, 3, 1, 1, 1, 1, 1, 4, 3 }, { 5, 5, 4, 1, 6, 5, 6, 6, 2, 4 }, { 5, 2, 1, 2, 1, 2, 3, 4, 2, 4 }, { 5, 3, 5, 6, 6, 3, 2, 4, 2, 6 }, { 3, 5, 4, 5, 4, 6, 2, 6, 5, 3 }, { 3, 1, 6, 6, 2, 1, 3, 4, 2, 6 } };
    private int buttonIndex = 0;
    private int buttonWhichButton = 0;
    private int buttonColor = 0;
    private List<Material> buttonNotUsedMats = new List<Material>();
    private Material buttonMat;
    public Material buttonFlashingMat;
    private bool buttonsSecondStage = false;
    //puncuation
    public GameObject punctuationGameObject;
    public TextMesh[] punctuationText;
    public Renderer punctuationDisplay;
    private string[,] punctuationList = new string[6, 10] { { "!,.", "\".?", "\"!?", ".!,", ",\".", ".!\"", ",.?", ",.\"", ",?.", "!?.", },
                                         { "\",!", ".?,", "!,\"", ".,\"", "?,.", "\"?!", "?.\"", "?\".", "?!.", "!?\"", },
                                         { ",.\"", "?.,", ".!,", "!,\"", "\",!", "!?,", "!?.", "\"!.", "!,\"", "\"!?", },
                                         { "!?.", "!.?", ".\",", ",!\"", "\".,", "\".,", "?\",", "!?,", "\"?!", "?!.", },
                                         { "?!,", "?!.", "?.,", ",!.", "?.\"", "!,?", "?!\"", "?,!", "!.\"", "?,.", },
                                         { ",\"?", ".\"!", "?,!", ".?!", ".!\"", "\".?", ".!\"", ".!?", "?,\"", "\".?" } };
    private string correctText;
    private int correctTextButton = 0;
    private int displayMat = 0;
    private int[] randomNums = new int[2];
    private List<string> takenPunctuations = new List<string>();
    //colored piano
    public GameObject pianoGameObject;
    public Renderer[] pianoKeys;
    private int pianoCorrectButton;
    private int[,] pianoKeyColorPositions = { { 0,3,5,4,0,2,4,1,4,2, },
                                             { 1,2,4,3,1,4,0,2,5,3, },
                                             { 2,4,1,0,5,1,2,0,3,4, },
                                             { 3,0,3,2,4,5,5,5,1,0, },
                                             { 4,5,2,1,3,0,1,3,0,5, },
                                             { 5,1,0,5,2,3,3,4,2,1 } };
    private char[] pianoKeyNames = { 'C', 'D', 'E', 'F', 'G', 'A' };
    private List<int> pianoMaterialNums = new List<int>();
    private List<int> pianoChosenMats = new List<int>();
    private int pianoCorrectMaterialNum = 0;

    private int[] pianoKeyNums = new int[6] { 6,6,6,6,6,6 };
    private int[] pianoPlaceholders = new int[2];
    //colorful message
    public GameObject messageGameObject;
    public GameObject messageDisplayButton;
    public Renderer messageDispaly;
    public TextMesh[] messageLetters;
    public Material messageBlackMat;
    private int[,] messageWords1 =
      { { 3,1,4,5,0,2 },
        { 0,1,4,5,2,3 },
        { 1,0,2,5,3,4 },
        { 2,3,4,0,5,1 },
        { 1,3,4,0,2,5 },
        { 2,1,3,4,0,5 },
        { 5,3,0,1,4,2 },
        { 5,0,1,4,2,3 },
        { 0,4,3,2,5,1 },
        { 4,3,5,2,1,0 } };
    private int[,] messageWords2 =
      { { 2,0,3,1,5,4 },
        { 5,4,1,3,0,2 },
        { 1,0,2,5,3,4 },
        { 2,3,1,5,0,4 },
        { 4,5,0,2,1,3 },
        { 3,4,0,1,2,5 },
        { 5,3,0,4,1,2 },
        { 3,0,1,4,2,5 },
        { 1,0,4,3,2,5 },
        { 0,3,2,4,1,5 } };
    private int[,] messageWords3 =
      { { 1,0,2,5,4,3 },
        { 5,1,4,0,2,3 },
        { 4,0,1,3,2,5 },
        { 0,3,4,2,5,1 },
        { 1,3,0,2,4,5 },
        { 3,4,1,2,0,5 },
        { 5,4,1,3,0,2 },
        { 3,0,5,2,1,4 },
        { 4,3,2,5,1,0 },
        { 2,5,1,3,4,0 } };
    private int[,] messageSelectedWordsChart =
      { { 1,0,2,5,4,3 },
        { 5,1,4,0,2,3 },
        { 4,0,1,3,2,5 },
        { 0,3,4,2,5,1 },
        { 1,3,0,2,4,5 },
        { 3,4,1,2,0,5 },
        { 5,4,1,3,0,2 },
        { 3,0,5,2,1,4 },
        { 4,3,2,5,1,0 },
        { 2,5,1,3,4,0 } };
    private int messageIndex;
    private int messageButtonPresses;
    private string[] messageColumnLetters = { "ATMSRE", "LOEDOP", "ECLARL", "TDSEAE", "CRUESE", "EARSHS", "REYALB", "USERTD", "TSLBAE", "SEIALD" };
    private string messageSelectedString;
    private bool messageDisplayButtonPressed = false;

    bool TwitchPlaysActive;
    private bool TPActive = false;
    private bool tpNumHigherThanThree;
    private bool tpNumHigherThanSix;

    void OnActivate()
    {
        if (TwitchPlaysActive == true)
        {
            TPActive = true;
            DebugMsg("Twitch Plays mode active.");
        }
        else
        {
            TPActive = false;
        }
    }

    void Start()
    {
        logicDiveGameObject.SetActive(false);
        messageGameObject.SetActive(false);
        pianoGameObject.SetActive(false);
        punctuationGameObject.SetActive(false);
        threeWiresGameObject.SetActive(false);
        coloredButtonsGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Awake()
    {
        StartCoroutine(glitchEffect());
        ModuleId = ModuleIdCounter++;

        GetComponent<KMBombModule>().OnActivate += OnActivate;

        moduleButton.OnInteract += delegate () { modulePressed(); return false; };
        messagePlayButton.OnInteract += delegate () { playMessage(); return false; };
        for (int i = 0; i < 9; i++)
        {
            int dummy = i;
            logicButtons[dummy].OnInteract += delegate () { logicPressed(dummy); return false; };
        }
        for (int i = 0; i < 3; i++)
        {
            int dummy = i;
            wireSelectables[dummy].OnInteract += delegate () { wireCut(dummy); return false; };
        }
        for (int i = 0; i < 6; i++)
        {
            int dummy = i;
            colorButtons[dummy].OnInteract += delegate () { coloredButtonPressed(dummy); return false; };
        }
        for (int i = 0; i < 6; i++)
        {
            int dummy = i;
            punctuationButtons[dummy].OnInteract += delegate () { punctuationPressed(dummy); return false; };
        }
        for (int i = 0; i < 6; i++)
        {
            int dummy = i;
            pianoButtons[dummy].OnInteract += delegate () { pianoPressed(dummy); return false; };
        }
        for (int i = 0; i < 6; i++)
        {
            int dummy = i;
            messageButtons[dummy].OnInteract += delegate () { messagePressed(dummy); return false; };
        }
    }

    private IEnumerator glitchEffect()
    {
        yield return true;
        if (!bombStarted)
        {
            audio.PlaySoundAtTransform("glitch" + Rnd.Range(1, 6), transform);
        }
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < glitchSquares.Count(); j++)
            {
                glitchSquares[j].color = glitchColors[Rnd.Range(0, 8)];
            }
            yield return new WaitForSeconds(.02f);
        }
        for (int j = 0; j < glitchSquares.Count(); j++)
        {
            glitchSquares[j].color = glitchColors[8];
        }
    }

    void memoryBankTextGoByeBye()
    {
        memoryBankText.text = "";
    }

    void modulePressed()
    {
        if (startingSoundEnabled && !moduleDetermined)
        {
            startingSoundEnabled = false;
            audio.PlaySoundAtTransform("beginningNoise", transform);
        }
        else if (!memoryBankSeen)
        {
            bombStarted = false;
            StartCoroutine(glitchEffect());
            memoryBankNumber = Rnd.Range(0, 100);
            if (memoryBankNumber < 10)
            {
                memoryBankText.text = "0" + memoryBankNumber;
                DebugMsg("The displayed number is " + 0 + memoryBankNumber + ".");
            }
            else
            {
                memoryBankText.text = "" + memoryBankNumber;
                DebugMsg("The displayed number is " + memoryBankNumber + ".");
            }
            memoryBankColor = memoryBanks[memoryBankNumber];
            for (int i = 0; i < 10; i++)
            {
                if (memoryBanks[i] == memoryBankColor)
                {
                    memoryBankColumn = i;
                    break;
                }
            }
            memoryBankSeen = true;
            Invoke("memoryBankTextGoByeBye", 3);
        }
        else
        {
            moduleButtonGameObject.SetActive(false);
            logicDiveGameObject.SetActive(true);
            memoryBankText.text = "";
            DebugMsg("Going into logic dive...");
            logicDiveActive = true;
            StartCoroutine(LogicDive());
        }
    }

    void logicPressed(int index)
    {
        logicDiveActive = false;
        logicDiveGameObject.SetActive(false);
        if (index != logicDiveCorrectButtonNum)
        {
            moduleButtonGameObject.SetActive(true);
            GetComponent<KMBombModule>().HandleStrike();
            DebugMsg("Strike! Pressed incorrect button.");
            startingSoundEnabled = true;
            memoryBankSeen = false;
            incorrect = false;
            StartCoroutine(glitchEffect());
        }
        else
        {
            StartCoroutine(glitchEffect());
            DebugMsg("Logic Dive completed successfully. Remembering module...");
            determineModule();
        }
    }

    void wireCut(int index)
    {
        if (!cutWires[index])
        {
            cutWires[index] = true;
            wires[index].GetComponent<MeshFilter>().mesh = wireCondition[1];
            if (wireCombinations[memoryBankColumn + wireIndex,nextWire] != (index + "1"))
            {
                DebugMsg("Strike! Pressed wire " + (index + 1) + ", while the correct wire was wire " + wireCombinations[memoryBankColumn + wireIndex,nextWire] + ".");
                incorrect = true;
            }
            else
            {
                nextWire++;
            }
        }
        if (!incorrect && nextWire == 3)
        {
            DebugMsg("Module solved!");
            moduleSolved = true;
            GetComponent<KMBombModule>().HandlePass();
        }
        else if (incorrect)
        {
            GetComponent<KMBombModule>().HandleStrike();
            incorrect = false;
            nextWire = 0;
            threeWires();
        }
    }

    void coloredButtonPressed(int index)
    {
        if (buttonsSecondStage)
        {
            StopAllCoroutines();
            if (index != secondButtonPresses[buttonColor, memoryBankColumn])
            {
                DebugMsg("Strike! Pressed button " + colorButtons[index].name + ", while the correct button for that stage was " + colorButtons[firstButtonPresses[buttonColor, memoryBankColumn]].name + ".");
                GetComponent<KMBombModule>().HandleStrike();
                buttonsSecondStage = false;
                coloredButtons();
            }
            else
            {
                DebugMsg("Module solved!");
                moduleSolved = true;
                GetComponent<KMBombModule>().HandlePass();
            }
        }
        else
        {
            if (index != firstButtonPresses[buttonColor, memoryBankColumn])
            {
                StopAllCoroutines();
                DebugMsg("Strike! Pressed button " + colorButtons[index].name + ", while the correct button for that stage was " + colorButtons[secondButtonPresses[buttonColor, memoryBankColumn]].name + ".");
                GetComponent<KMBombModule>().HandleStrike();
                buttonsSecondStage = false;
                coloredButtons();
            }
            else
            {
                buttonsSecondStage = true;
                DebugMsg("Correct button. Advancing to next stage.");
            }
        }
    }

    void punctuationPressed(int index)
    {
        if (index != correctTextButton)
        {
            DebugMsg("Strike! Pressed button " + (index + 1) + ", while the correct button was " + (correctTextButton + 1) + ".");
            GetComponent<KMBombModule>().HandleStrike();
            puncuationButtons();
        }
        else
        {
            DebugMsg("Module solved!");
            moduleSolved = true;
            GetComponent<KMBombModule>().HandlePass();
        }
    }

    void pianoPressed(int index)
    {
        if (index == pianoCorrectButton)
        {
            DebugMsg("Module solved!");
            moduleSolved = true;
            GetComponent<KMBombModule>().HandlePass();
        }
        else
        {
            DebugMsg("Strike! Pressed key " + pianoKeyNames[index] + ".");
            GetComponent<KMBombModule>().HandleStrike();
            coloredPiano();
        }
    }

    void messagePressed(int index)
    {
        if(!messageDisplayButtonPressed)
        {
            return;
        }
        if (messageLetters[index].text == ("" + messageSelectedString[messageSelectedWordsChart[memoryBankColumn, messageButtonPresses]]))
        {
            DebugMsg("Correct button pressed.");
            messageButtonPresses++;
            if (messageButtonPresses == 6)
            {
                DebugMsg("Module solved!");
                moduleSolved = true;
                GetComponent<KMBombModule>().HandlePass();
            }
        }
        else
        {
            DebugMsg("Strike! Pressed letter " + messageLetters[index].text + ", while the correct letter was " + messageSelectedString[messageSelectedWordsChart[memoryBankColumn, messageButtonPresses]]);
            GetComponent<KMBombModule>().HandleStrike();
            messageDisplayButton.SetActive(true);
            messageDisplayButtonPressed = false;
            colorfulMessage();
        }
    }

    void playMessage()
    {
        StopAllCoroutines();
        StartCoroutine(DisplayUpdater());
        messageDisplayButtonPressed = true;
        messageDisplayButton.SetActive(false);
    }

    private IEnumerator LogicDive()
    {
        if (!TPActive)
        {
            for (int k = 0; k < 6; k++)
            {
                logicDiveNotCorrectMats.Clear();
                logicDiveCorrectButtonNum = Rnd.Range(0, 9);
                for (int i = 0; i < 10; i++)
                {
                    if (logicDiveMats[i].name.Equals(memoryBanks[memoryBankNumber].ToString()))
                    {
                        logicDiveButtons[logicDiveCorrectButtonNum].material = logicDiveMats[i];
                        for (int j = 0; j < 10; j++)
                        {
                            if (i != j)
                            {
                                logicDiveNotCorrectMats.Add(logicDiveMats[j]);
                            }
                        }
                        break;
                    }
                }
                for (int i = 0; i < 9; i++)
                {
                    if (i != logicDiveCorrectButtonNum)
                    {
                        logicDiveIndex = Rnd.Range(0, logicDiveNotCorrectMats.Count);
                        logicDiveButtons[i].material = logicDiveNotCorrectMats[logicDiveIndex];
                        logicDiveNotCorrectMats.RemoveAt(logicDiveIndex);
                    }
                }
                if (logicDiveActive)
                {
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    k = 6;
                }
            }
            if (logicDiveActive) //just to be safe
            {
                moduleButtonGameObject.SetActive(true);
                logicDiveActive = false;
                startingSoundEnabled = true;
                memoryBankSeen = false;
                GetComponent<KMBombModule>().HandleStrike();
                DebugMsg("Strike! Took too long in logic dive.");
                logicDiveGameObject.SetActive(false);
                StartCoroutine(glitchEffect());
            }
        }
        else
        {
            logicDiveNotCorrectMats.Clear();
            logicDiveCorrectButtonNum = Rnd.Range(0, 9);
            for (int i = 0; i < 10; i++)
            {
                if (logicDiveMats[i].name.Equals(memoryBanks[memoryBankNumber].ToString()))
                {
                    logicDiveButtons[logicDiveCorrectButtonNum].material = logicDiveMats[i];
                    for (int j = 0; j < 10; j++)
                    {
                        if (logicDiveMats[j] != logicDiveMats[i])
                        {
                            logicDiveNotCorrectMats.Add(logicDiveMats[j]);
                        }
                    }
                    break;
                }
            }
            for (int i = 0; i < 9; i++)
            {
                if (i != logicDiveCorrectButtonNum)
                {
                    logicDiveIndex = Rnd.Range(0, logicDiveNotCorrectMats.Count);
                    logicDiveButtons[i].material = logicDiveNotCorrectMats[logicDiveIndex];
                    logicDiveNotCorrectMats.RemoveAt(logicDiveIndex);
                }
            }
            yield return new WaitForSeconds(15f);
            if (logicDiveActive) //just to be safe
            {
                moduleButtonGameObject.SetActive(true);
                logicDiveActive = false;
                startingSoundEnabled = true;
                memoryBankSeen = false;
                GetComponent<KMBombModule>().HandleStrike();
                DebugMsg("Strike! Took too long in logic dive.");
                logicDiveGameObject.SetActive(false);
                StartCoroutine(glitchEffect());
            }
        }
    }

    void determineModule()
    {
        moduleDetermined = true;
        whichModule = Rnd.Range(0,5);
        Invoke(moduleVoids[whichModule], 0.1f);
    }

    void threeWires()
    {
        threeWiresGameObject.SetActive(true);
        for(int i = 0; i < 3; i++)
        {
            cutWires[i] = false;
        }
        foreach(Renderer wire in wires)
        {
            wire.GetComponent<MeshFilter>().mesh = wireCondition[0];
            if (wire == wires[1])
            {
                wireIndex = Rnd.Range(0, 6);
                DebugMsg("The middle wire's color is " + wirescolors[wireIndex] + ".");
                wire.material = notMemoryBankMats[wireIndex];
            }
            else
            {
                wire.material = notMemoryBankMats[Rnd.Range(0, 6)];
            }
        }
    }

    void coloredButtons()
    {
        buttonNotUsedMats.Clear();
        for(int i = 0; i < 6; i++)
        {
            buttonNotUsedMats.Add(notMemoryBankMats[i]);
        }
        coloredButtonsGameObject.SetActive(true);
        buttonWhichButton = Rnd.Range(0,6);
        foreach (Renderer button in coloredButtonss)
        {
            if (button == coloredButtonss[buttonWhichButton])
            {
                buttonColor = Rnd.Range(0, buttonNotUsedMats.Count);
                button.material = buttonNotUsedMats[buttonColor];
                buttonMat = button.material;
                buttonIndex = buttonColor;
                for (int i = 0; i < 6; i++)
                {
                    if (notMemoryBankMats[i] == buttonNotUsedMats[buttonColor])
                    {
                        buttonColor = i;
                        DebugMsg("The flashing button's color is " + wirescolors[buttonColor] + ".");
                        break;
                    }
                }
                buttonNotUsedMats.RemoveAt(buttonIndex);
                StartCoroutine(Flashy(button));
            }
            else
            {
                buttonIndex = Rnd.Range(0, buttonNotUsedMats.Count);
                button.material = buttonNotUsedMats[buttonIndex];
                buttonNotUsedMats.RemoveAt(buttonIndex);
            }
        }
    }

    private IEnumerator Flashy(Renderer button)
    {
        while(true)
        {
            button.material = buttonMat;
            yield return new WaitForSeconds(.5f);
            button.material = buttonFlashingMat;
            yield return new WaitForSeconds(.25f);
        }
    }

    void puncuationButtons()
    {
        punctuationGameObject.SetActive(true);
        takenPunctuations.Clear();
        correctTextButton = Rnd.Range(0,6);
        correctText = punctuationList[Rnd.Range(0, 6) , memoryBankColumn];
        displayMat = Rnd.Range(0,6);
        punctuationDisplay.material = notMemoryBankMats[displayMat];
        DebugMsg("The display's color is " + wirescolors[displayMat] + ".");
        DebugMsg("The correct punctuation marks are " + correctText);
        takenPunctuations.Add(correctText);
        for (int i = 0; i < 6; i++)
        {
            if (i == correctTextButton)
            {
                punctuationText[i].text = correctText;
            }
            else
            {
                do
                {
                    punctuationText[i].text = "";
                    randomNums[0] = Rnd.Range(0, 6);
                    randomNums[1] = Rnd.Range(0, 10);
                    punctuationText[i].text = punctuationList[randomNums[0], randomNums[1]];
                } while (takenPunctuations.Contains(punctuationText[i].text));
                takenPunctuations.Add(punctuationText[i].text);
            }
        }
    }

    void coloredPiano()
    {
        pianoGameObject.SetActive(true);
        pianoMaterialNums.Clear();
        pianoChosenMats.Clear();
        pianoCorrectButton = Rnd.Range(0, 6);
        for (int i = 0; i < 6; i++)
        {
            pianoMaterialNums.Add(i);
        }
        for (int j = 0; j < 6; j++)
        {
            if (pianoKeyColorPositions[j, memoryBankColumn] == pianoCorrectButton)
            {
                pianoCorrectMaterialNum = j;
                break;
            }
        }
        for (int i = 0; i < 6; i++)
        {
            if (i == pianoCorrectMaterialNum)
            {
                pianoKeyNums[pianoCorrectButton] = i;
            }
            else
            {
                do
                {
                    pianoPlaceholders[0] = Rnd.Range(0, 6);
                } while (pianoKeyNums[pianoPlaceholders[0]] != 6 || pianoPlaceholders[0] == pianoCorrectButton);
                pianoKeyNums[pianoPlaceholders[0]] = i;
            }
        }
        DebugMsg("The correct key to press is Key " + pianoKeyNames[pianoCorrectButton]);
        checkforcorrects:
        for(int i = 0; i < 6; i++)
        {
            if(i != pianoCorrectButton)
            {
                if(pianoKeyColorPositions[pianoKeyNums[i], memoryBankColumn] == i)
                {
                    do
                    {
                        pianoPlaceholders[1] = Rnd.Range(0, 6);
                    } while (pianoPlaceholders[1] == pianoCorrectButton);
                    pianoPlaceholders[0] = pianoKeyNums[pianoPlaceholders[1]];
                    pianoKeyNums[pianoPlaceholders[1]] = pianoKeyNums[i];
                    pianoKeyNums[i] = pianoPlaceholders[0];
                    i = 6;
                    goto checkforcorrects;
                }
            }
        }
        for (int i = 0; i < 6; i++)
        {
            pianoKeys[i].material = notMemoryBankMats[pianoKeyNums[i]];
        }
    }

    void colorfulMessage()
    {
        messageGameObject.SetActive(true);
        messageDisplayButton.SetActive(true);
        messageButtonPresses = 0;
        messageIndex = Rnd.Range(0, 3);
        messageSelectedString = messageColumnLetters[memoryBankColumn];
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                if(messageIndex == 0)
                {
                    messageSelectedWordsChart[i, j] = messageWords1[i, j];
                }
                else if (messageIndex == 1)
                {
                    messageSelectedWordsChart[i, j] = messageWords2[i, j];
                }
                else
                {
                    messageSelectedWordsChart[i, j] = messageWords3[i, j];
                }
            }
        }
        DebugMsg("The word for Colorful Message is " + messageSelectedString[messageSelectedWordsChart[memoryBankColumn, 0]] + messageSelectedString[messageSelectedWordsChart[memoryBankColumn, 1]] + messageSelectedString[messageSelectedWordsChart[memoryBankColumn, 2]] + messageSelectedString[messageSelectedWordsChart[memoryBankColumn, 3]] + messageSelectedString[messageSelectedWordsChart[memoryBankColumn, 4]] + messageSelectedString[messageSelectedWordsChart[memoryBankColumn, 5]]);
        foreach(TextMesh theText in messageLetters)
        {
            theText.text = "?";
        }
    }

    private IEnumerator DisplayUpdater()
    {
        messageDispaly.material = notMemoryBankMats[messageSelectedWordsChart[memoryBankColumn, 0]];
        yield return new WaitForSeconds(1f);
        messageDispaly.material = notMemoryBankMats[messageSelectedWordsChart[memoryBankColumn, 1]];
        yield return new WaitForSeconds(1f);
        messageDispaly.material = notMemoryBankMats[messageSelectedWordsChart[memoryBankColumn, 2]];
        yield return new WaitForSeconds(1f);
        messageDispaly.material = notMemoryBankMats[messageSelectedWordsChart[memoryBankColumn, 3]];
        yield return new WaitForSeconds(1f);
        messageDispaly.material = notMemoryBankMats[messageSelectedWordsChart[memoryBankColumn, 4]];
        yield return new WaitForSeconds(1f);
        messageDispaly.material = notMemoryBankMats[messageSelectedWordsChart[memoryBankColumn, 5]];
        yield return new WaitForSeconds(1f);
        messageDispaly.material = messageBlackMat;
        for (int i = 0; i < 6; i++)
        {
            messageLetters[i].text = "" + messageSelectedString[i];
        }
        
    }

    private bool isCommandValid(string cmd)
    {
        string[] validbtns = { "logic", "cut", "press", "tap", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        var parts = cmd.ToLowerInvariant().Split(new[] { ' ' });

        foreach (var btn in parts)
        {
            if (!validbtns.Contains(btn.ToLower()))
            {
                return false;
            }
            else if (btn.ToLower() == "4" || btn.ToLower() == "5" || btn.ToLower() == "6")
            {
                tpNumHigherThanThree = true;
            }
            else if (btn.ToLower() == "7" || btn.ToLower() == "8" || btn.ToLower() == "9")
            {
                tpNumHigherThanSix = true;
            }
        }
        return true;
    }

#pragma warning disable 414
    public string TwitchHelpMessage = "!{0} tap to tap the module before the logic dive. !{0} logic 1-9 to tap the button in that position in reading order. !{0} cut 1-3 to cut wires (Three Wires). !{0} press/tap 1-6 to press a button (Colored Buttons/Punctuation Buttons/Colored Piano/Colorful Message).  Commands that are related to a selected module after a logic dive can be chained. To press the small button in Colorful Message, do !{0} message.";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string cmd)
    {
        tpNumHigherThanThree = false;
        tpNumHigherThanSix = false;
        TPActive = true;
        var parts = cmd.ToLowerInvariant().Split(new[] { ' ' });
        if(parts.Count() == 1 && parts[0] == "tap")
        {
            if(logicDiveActive || moduleDetermined)
            {
                yield return "sendtochat I'm not in a state to do this right now.";
                yield break;
            }
            else
            {
                yield return null;
                yield return new KMSelectable[] { moduleButton };
            }
        }
        else if(parts.Count() == 1 && parts[0] == "message" && whichModule == 4 && !messageDisplayButtonPressed)
        {
            yield return null;
            yield return new KMSelectable[] { messagePlayButton };
        }
        else if (isCommandValid(cmd))
        {
            if(parts[0] == "logic")
            {
                if(parts.Count() > 2)
                {
                    yield return "sendtochat Logic Dive cannot press more than 1 button.";
                    yield break;
                }
                else if (parts.Count() < 2)
                {
                    yield return "sendtochat You didn't include a button to press!";
                    yield break;
                }
                else if(moduleDetermined || !logicDiveActive)
                {
                    yield return "sendtochat I'm not in a state to do this right now.";
                    yield break;
                }
                else
                {
                    for(int i = 0; i < 9; i++)
                    {
                        if((i + 1).ToString() == parts[1])
                        {
                            yield return null;
                            yield return new KMSelectable[] { logicButtons[i] };
                        }
                    }
                }
            }
            else if(!moduleDetermined)
            {
                yield return "sendtochat I'm not in a state to do this right now.";
                yield break;
            }
            else if(parts[0] == "cut")
            {
                if(whichModule == 0)
                {
                    if(tpNumHigherThanThree || tpNumHigherThanSix)
                    {
                        yield return "sendtochat I don't have any wires that are in a position higher than 3!";
                        yield break;
                    }
                    else
                    {
                        for(int i = 0; i < (parts.Count() - 1); i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                if ((j + 1).ToString() == parts[i + 1])
                                {
                                    yield return null;
                                    yield return new KMSelectable[] { wireSelectables[j] };
                                }
                            }
                        }
                    }
                }
                else
                {
                    yield return "sendtochat My selected module is not Three Wires!";
                    yield break;
                }
            }
            else if(parts[0] == "press" || parts[0] == "tap")
            {
                if(whichModule == 1)
                {
                    if(parts.Count() > 3)
                    {
                        yield return "sendtochat Colored Buttons can only press 2 buttons!";
                        yield break;
                    }
                    else if (tpNumHigherThanSix)
                    {
                        yield return "sendtochat I don't have any buttons that are in a position higher than 6!";
                        yield break;
                    }
                    else
                    {
                        for (int i = 0; i < (parts.Count() - 1); i++)
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                if ((j + 1).ToString() == parts[i + 1])
                                {
                                    yield return null;
                                    yield return new KMSelectable[] { colorButtons[j] };
                                }
                            }
                        }
                    }
                }
                else if (whichModule == 2)
                {
                    if (parts.Count() > 2)
                    {
                        yield return "sendtochat Punctuation Buttons can only press 1 button!";
                        yield break;
                    }
                    else if (tpNumHigherThanSix)
                    {
                        yield return "sendtochat I don't have any buttons that are in a position higher than 6!";
                        yield break;
                    }
                    else
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            if ((j + 1).ToString() == parts[1])
                            {
                                yield return null;
                                yield return new KMSelectable[] { punctuationButtons[j] };
                            }
                        }
                    }
                }
                else if (whichModule == 3)
                {
                    if (parts.Count() > 2)
                    {
                        yield return "sendtochat Colored Piano can only press 1 key!";
                        yield break;
                    }
                    else if (tpNumHigherThanSix)
                    {
                        yield return "sendtochat I don't have any keys that are in a position higher than 6!";
                        yield break;
                    }
                    else
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            if ((j + 1).ToString() == parts[1])
                            {
                                yield return null;
                                yield return new KMSelectable[] { pianoButtons[j] };
                            }
                        }
                    }
                }
                else if (whichModule == 4)
                {
                    if (parts.Count() > 7)
                    {
                        yield return "sendtochat Colorful Message can only press 6 buttons!";
                        yield break;
                    }
                    else if (tpNumHigherThanSix)
                    {
                        yield return "sendtochat I don't have any buttons that are in a position higher than 6!";
                        yield break;
                    }
                    else
                    {
                        for (int i = 0; i < (parts.Count() - 1); i++)
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                if ((j + 1).ToString() == parts[i + 1])
                                {
                                    yield return null;
                                    yield return new KMSelectable[] { messageButtons[j] };
                                }
                            }
                        }
                    }
                }
                else
                {
                    yield return "sendtochat My module is Three Wires!";
                    yield break;
                }
            }
        }
        else
        {
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        if (startingSoundEnabled)
        {
            moduleButton.OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        if (!memoryBankSeen)
        {
            moduleButton.OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        if (!logicDiveActive && !moduleDetermined)
        {
            moduleButton.OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        if (logicDiveActive)
        {
            logicButtons[logicDiveCorrectButtonNum].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void DebugMsg(string msg)
    {
        Debug.LogFormat("[...? #{0}] {1}", ModuleId, msg);
    }
}
 