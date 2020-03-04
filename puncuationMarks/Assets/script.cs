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

    public KMSelectable[] buttons;
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
    private int logicDiveNumber = 0;
    public GameObject logicDiveGameObject;
    public Material[] logicDiveMats;
    private List<Material> logicDiveNotCorrectMats = new List<Material>();
    private int logicDiveCorrectButtonNum = 0;
    private int logicDiveIndex = 0;
    public Renderer[] logicDiveButtons;
    //general second-half module stuff
    private int whichModule = 0;
    public Material[] notMemoryBankMats;
    private string[] moduleVoids = { "threeWires", "coloredButtons", "puncuationButtons","coloredPiano","colorfulMessage" };
    private bool moduleDown = false;
    //three wires
    public Renderer[] wires;
    public GameObject threeWiresGameObject;
    public Mesh[] wireCondition;
    private string[] wirescolors = { "Crimson","Brown","Dark Yellow","Green","Blue","Magenta" };
    private string[] wireCombinations = { "123", "132", "213", "231", "312", "213", "123", "312", "132", "321",
                                          "132", "321", "123", "312", "213", "231", "321", "132", "123", "231",
                                          "213", "123", "312", "321", "231", "132", "312", "231", "213", "123",
                                          "231", "312", "132", "213", "123", "321", "231", "123", "321", "213",
                                          "312", "231", "321", "132", "321", "123", "213", "321", "312", "132",
                                          "321", "213", "231", "123", "132", "312", "132", "213", "231", "312" };
    private int wireIndex = 0;  
    private int nextWire = 0;
    //colored buttons
    public GameObject coloredButtonsGameObject;
    public Renderer[] coloredButtonss;
    private int[,] firstButtonPresses = { { 4, 5, 3, 6, 1, 2, 5, 6, 3, 4 }, { 4, 1, 6, 2, 2, 1, 5, 2, 6, 6 }, { 4, 5, 5, 4, 3, 3, 5, 3, 6, 1 }, { 5, 5, 2, 6, 5, 2, 5, 4, 4, 4 }, { 2, 6, 2, 3, 1, 4, 1, 1, 6, 6 }, { 1, 3, 3, 1, 2, 4, 6, 2, 3, 3 } };
    private int[,] secondButtonPresses = { { 3,4,3,1,1,1,1,1,4,3 },{ 5,5,4,1,6,5,6,6,2,4 },{ 5,2,1,2,1,2,3,4,2,4 },{ 5,3,5,6,6,3,2,4,2,6 },{ 3,5,4,5,4,6,2,6,5,3 },{ 3,1,6,6,2,1,3,4,2,6 } };
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
    //Qs respresent " because the character " ends the string
    private string[] punctuationList = { "!,.", "Q.?", "Q!?", ".!,", ",Q.", ".!Q", ",.?", ",.Q", ",?.", "!?.",
                                         "Q,!", ".?,", "!,Q", ".,Q", "?,.", "Q?!", "?.Q", "?Q.", "?!.", "!?Q",
                                         ",.Q", "?.,", ".!,", "!,Q", "Q,!", "!?,", "!?.", "Q!.", "!,Q", "Q!?",
                                         "!?.", "!.?", ".Q,", ",!Q", "Q.,", "Q.,", "?Q,", "!?,", "Q?!", "?!.",
                                         "?!,", "?!.", "?.,", ",!.", "?.Q", "!,?", "?!Q", "?,!", "!.Q", "?,.",
                                         ",Q?", ".Q!", "?,!", ".?!", ".!Q", "Q.?", ".!Q", ".!?", "?,Q", "Q.?", };
    private string correctText;
    private int correctTextButton = 0;
    private int displayMat = 0;
    private int[] randomNums = new int[2];
    private List<int> takenPunctuations1 = new List<int>();
    private List<int> takenPunctuations2 = new List<int>();
    //colored piano
    public GameObject pianoGameObject;
    public Renderer[] pianoKeys;
    private int pianoCorrectButton;
    private int[] pianoKeyColorPositions = { 0,3,5,4,0,2,4,1,4,2,
                                             1,2,4,3,1,4,0,2,5,3,
                                             2,4,1,0,5,1,2,0,3,4,
                                             3,0,3,2,4,5,5,5,1,0,
                                             4,5,2,1,3,0,1,3,0,5,
                                             5,1,0,5,2,3,3,4,2,1 };
    private char[] pianoKeyNames = { 'C','D','E','F','G','A' };
    private List<int> pianoMaterialNums = new List<int>();
    private int[] pianoChosenMats = { 6,6,6,6,6,6 };
    private int pianoIndex = 0;
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

    // Update is called once per frame
    void Awake()
    {
        StartCoroutine(glitchEffect());
        ModuleId = ModuleIdCounter++;

        foreach (KMSelectable button in buttons)
        {
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { buttonPressed(pressedButton); return false; };
        }
    }

    private IEnumerator glitchEffect()
    {
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

    void buttonPressed(KMSelectable pressedButton)
    {
        pressedButton.AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransformWithRef(KMSoundOverride.SoundEffect.ButtonPress, transform);

        incorrect = false;

        if (moduleSolved)
        {
            return;
        }
        else
        {
            for (int j = 0; j < glitchSquares.Count(); j++)
            {
                glitchSquares[j].color = glitchColors[8];
            }
            if (startingSoundEnabled && !moduleDetermined)
            {
                startingSoundEnabled = false;
                audio.PlaySoundAtTransform("beginningNoise", transform);
            }
            else
            {
                if (!memoryBankSeen)
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
                    for(int i = 0; i < 10; i++)
                    {
                        if(memoryBanks[i] == memoryBankColor)
                        {
                            memoryBankColumn = i;
                            i = 10;
                        }
                    }
                    memoryBankSeen = true;
                    Invoke("memoryBankTextGoByeBye", 3);
                }
                else if (!moduleDetermined)
                {
                    if (!logicDiveActive)
                    {
                        logicDiveGameObject.transform.localPosition = new Vector3(logicDiveGameObject.transform.localPosition.x, logicDiveGameObject.transform.localPosition.y - 100f, logicDiveGameObject.transform.localPosition.z);
                        buttons[0].transform.localPosition = new Vector3(buttons[0].transform.localPosition.x, buttons[0].transform.localPosition.y + 100f, buttons[0].transform.localPosition.z);
                        moduleButtonGameObject.SetActive(false);
                        logicDiveGameObject.SetActive(true);
                        memoryBankText.text = "";
                        DebugMsg("Going into logic dive...");
                        logicDiveActive = true;
                        logicDive();
                    }
                    else
                    {
                        logicDiveGameObject.transform.localPosition = new Vector3(logicDiveGameObject.transform.localPosition.x, logicDiveGameObject.transform.localPosition.y + 100f, logicDiveGameObject.transform.localPosition.z);
                        logicDiveGameObject.SetActive(false);
                        for (int i = 0; i < 9; i++)
                        {
                            if(buttons[i + 1] == pressedButton)
                            {
                                if(i != logicDiveCorrectButtonNum)
                                {
                                    incorrect = true;
                                }
                                i = 9;
                            }
                        }
                        if (!incorrect)
                        {
                            logicDiveActive = false;
                            StartCoroutine(glitchEffect());
                            DebugMsg("Logic Dive completed successfully. Remembering module...");
                            determineModule();
                        }
                        else
                        {
                            buttons[0].transform.localPosition = new Vector3(buttons[0].transform.localPosition.x, buttons[0].transform.localPosition.y - 100f, buttons[0].transform.localPosition.z);
                            moduleButtonGameObject.SetActive(true);
                            GetComponent<KMBombModule>().HandleStrike();
                            DebugMsg("Strike! Pressed incorrect button.");
                            logicDiveActive = false;
                            logicDiveNumber = 0;
                            startingSoundEnabled = true;
                            memoryBankSeen = false;
                            incorrect = false;
                            StartCoroutine(glitchEffect());
                        }
                    }
                }
                else
                {
                    if (whichModule == 0) //3 wires
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if(buttons[10 + i] == pressedButton)
                            {
                                wires[i].GetComponent<MeshFilter>().mesh = wireCondition[1];
                                if (wireCombinations[memoryBankColumn + (wireIndex * 10)][nextWire].ToString() != (i + 1).ToString())
                                {
                                    DebugMsg("Strike! Pressed wire " + (i + 1) + ", while the correct wire was wire " + wireCombinations[memoryBankColumn + (wireIndex * 10)][nextWire] + ".");
                                    incorrect = true;
                                }
                                else
                                {
                                    nextWire++;
                                }
                                i = 3;
                            }
                        }
                        if (!incorrect && nextWire == 3)
                        {
                            DebugMsg("Module solved!");
                            moduleSolved = true;
                            GetComponent<KMBombModule>().HandlePass();
                        }
                        else if(incorrect)
                        {
                            GetComponent<KMBombModule>().HandleStrike();
                            incorrect = false;
                            nextWire = 0;
                            threeWires();
                        }
                    }
                    else if(whichModule == 1)
                    {
                        if(buttonsSecondStage)
                        {
                            StopAllCoroutines();
                            if (pressedButton != buttons[(secondButtonPresses[buttonColor, memoryBankColumn]) + 12])
                            {
                                DebugMsg("Strike! Pressed button " + pressedButton.name + ", while the correct button for that stage was " + buttons[(firstButtonPresses[buttonColor, memoryBankColumn]) + 12].name + ".");
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
                            if (pressedButton != buttons[(firstButtonPresses[buttonColor, memoryBankColumn]) + 12])
                            {
                                StopAllCoroutines();
                                DebugMsg("Strike! Pressed button " + pressedButton.name + ", while the correct button for that stage was " + buttons[(secondButtonPresses[buttonColor, memoryBankColumn]) + 12].name + ".");
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
                    else if(whichModule == 2)
                    {
                        for(int i = 0; i < 6; i++)
                        {
                            if(pressedButton == buttons[i + 19])
                            {
                                if(i != correctTextButton)
                                {
                                    DebugMsg("Strike! Pressed button " + (i + 1) + ", while the correct button was " + (correctTextButton + 1) + ".");
                                    GetComponent<KMBombModule>().HandleStrike();
                                    puncuationButtons();
                                }
                                else
                                {
                                    DebugMsg("Module solved!");
                                    moduleSolved = true;
                                    GetComponent<KMBombModule>().HandlePass();
                                }
                                i = 6;
                            }
                        }
                    }
                    else if(whichModule == 3)
                    {
                        for(int i = 0; i < 6; i++)
                        {
                            if(pressedButton == buttons[i + 25])
                            {
                                if(i == pianoCorrectButton)
                                {
                                    DebugMsg("Module solved!");
                                    moduleSolved = true;
                                    GetComponent<KMBombModule>().HandlePass();
                                }
                                else
                                {
                                    DebugMsg("Strike! Pressed key " + pianoKeyNames[i] + ".");
                                    GetComponent<KMBombModule>().HandleStrike();
                                    puncuationButtons();
                                }
                            }
                        }
                    }
                    else if(whichModule == 4)
                    {
                        if(pressedButton == buttons[37])
                        {
                            StopAllCoroutines();
                            StartCoroutine(DisplayUpdater());
                            messageDisplayButton.SetActive(false);
                        }
                        else
                        {
                            for(int i = 0; i < 6; i++)
                            {
                                if(pressedButton == buttons[31 + i])
                                {
                                    if(messageLetters[i].text == ("" + messageSelectedString[messageSelectedWordsChart[memoryBankColumn, messageButtonPresses]]))
                                    {
                                        DebugMsg("Correct button pressed.");
                                        messageButtonPresses++;
                                        if(messageButtonPresses == 6)
                                        {
                                            DebugMsg("Module solved!");
                                            moduleSolved = true;
                                            GetComponent<KMBombModule>().HandlePass();
                                        }
                                    }
                                    else
                                    {
                                        DebugMsg("Strike! Pressed letter " + messageLetters[i].text + ", while the correct letter was " + messageSelectedString[messageSelectedWordsChart[memoryBankColumn, messageButtonPresses]]);
                                        GetComponent<KMBombModule>().HandleStrike();
                                        colorfulMessage();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void logicDive()
    {
        logicDiveNumber++;
        if (logicDiveNumber < 7)
        {
            logicDiveNotCorrectMats.Clear();
            logicDiveCorrectButtonNum = Rnd.Range(0,9);
            for(int i = 0; i < 10; i++)
            {
                if(logicDiveMats[i].name.Equals(memoryBanks[memoryBankNumber].ToString()))
                {
                    logicDiveButtons[logicDiveCorrectButtonNum].material = logicDiveMats[i];
                    for(int j = 0; j < 10; j++)
                    {
                        if(logicDiveMats[j] != logicDiveMats[i])
                        {
                            logicDiveNotCorrectMats.Add(logicDiveMats[j]);
                        }
                    }
                    i = 10;
                }
            }
            for(int i = 0; i < 9; i++)
            {
                if(i != logicDiveCorrectButtonNum)
                {
                    logicDiveIndex = Rnd.Range(0, logicDiveNotCorrectMats.Count);
                    logicDiveButtons[i].material = logicDiveNotCorrectMats[logicDiveIndex];
                    logicDiveNotCorrectMats.RemoveAt(logicDiveIndex);
                }
            }
            if (logicDiveActive)
            {
                Invoke("logicDive", 1);
            }
        }
        else if (logicDiveActive) //just to be safe
        {
            logicDiveGameObject.transform.localPosition = new Vector3(logicDiveGameObject.transform.localPosition.x, logicDiveGameObject.transform.localPosition.y + 100f, logicDiveGameObject.transform.localPosition.z);
            buttons[0].transform.localPosition = new Vector3(buttons[0].transform.localPosition.x, buttons[0].transform.localPosition.y - 100f, buttons[0].transform.localPosition.z);
            moduleButtonGameObject.SetActive(true);
            logicDiveActive = false;
            logicDiveNumber = 0;
            startingSoundEnabled = true;
            memoryBankSeen = false;
            GetComponent<KMBombModule>().HandleStrike();
            DebugMsg("Strike! Took too long in logic dive.");
            StartCoroutine(glitchEffect());
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
        if(!moduleDown)
        {
            moduleDown = true;
            threeWiresGameObject.transform.localPosition = new Vector3(threeWiresGameObject.transform.localPosition.x, threeWiresGameObject.transform.localPosition.y - 100f, threeWiresGameObject.transform.localPosition.z);
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
        if (!moduleDown)
        {
            moduleDown = true;
            coloredButtonsGameObject.transform.localPosition = new Vector3(threeWiresGameObject.transform.localPosition.x, threeWiresGameObject.transform.localPosition.y - 100f, threeWiresGameObject.transform.localPosition.z);
        }
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
                        i = 6;
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
        if (!moduleDown)
        {
            moduleDown = true;
            punctuationGameObject.transform.localPosition = new Vector3(punctuationGameObject.transform.localPosition.x, punctuationGameObject.transform.localPosition.y - 100f, punctuationGameObject.transform.localPosition.z);
        }
        takenPunctuations1.Clear();
        takenPunctuations2.Clear();
        correctTextButton = Rnd.Range(0,6);
        correctText = "";
        displayMat = Rnd.Range(0,6);
        punctuationDisplay.material = notMemoryBankMats[displayMat];
        for (int j = 0; j < 3; j++)
        {
            if (punctuationList[(displayMat * 10) + memoryBankColumn][j] == 'Q')
            {
                correctText = correctText + '"';
            }
            else
            {
                correctText = correctText + punctuationList[(displayMat * 10) + memoryBankColumn][j];
            }
        }
        DebugMsg("The display's color is " + wirescolors[displayMat] + ".");
        DebugMsg("The correct punctuation marks are " + correctText);
        foreach (TextMesh text in punctuationText)
        {
            text.text = "";
            if(text == punctuationText[correctTextButton])
            {
                for(int j = 0; j < 3; j++)
                {
                    text.text = correctText;
                }
            }
            else
            {
                text.text = correctText;
                while (text.text == correctText)
                {
                    text.text = "";
                    randomNums[0] = Rnd.Range(0, 6) * 10;
                    while(takenPunctuations1.Contains(randomNums[0]))
                    {
                        randomNums[0] = Rnd.Range(0, 6) * 10;
                    }
                    takenPunctuations1.Add(randomNums[0]);
                    randomNums[1] = Rnd.Range(0, 10);
                    while (takenPunctuations2.Contains(randomNums[1]))
                    {
                        randomNums[1] = Rnd.Range(0, 10);
                    }
                    takenPunctuations2.Add(randomNums[1]);
                    for (int j = 0; j < 3; j++)
                    {
                        if (punctuationList[randomNums[0] + randomNums[1]][j] == 'Q')
                        {
                            text.text = text.text + '"';
                        }
                        else
                        {
                            text.text = text.text + punctuationList[randomNums[0] + randomNums[1]][j];
                        }
                    }
                }
            }
        }
    }

    void coloredPiano()
    {
        if (!moduleDown)
        {
            moduleDown = true;
            pianoGameObject.transform.localPosition = new Vector3(pianoGameObject.transform.localPosition.x, pianoGameObject.transform.localPosition.y - 100f, pianoGameObject.transform.localPosition.z);
        }
        pianoMaterialNums.Clear();
        pianoCorrectButton = Rnd.Range(0, 6);
        for (int i = 0; i < 6; i++)
        {
            pianoMaterialNums.Add(i);
            pianoChosenMats[i] = 6;
        }
        DebugMsg("The correct key to press is Key " + pianoKeyNames[pianoCorrectButton]);
        for(int i = 0; i < 6; i++)
        {
            if(i == pianoCorrectButton)
            {
                for(int j = 0; j < 6; j++)
                {
                    if(pianoKeyColorPositions[(j * 10) + memoryBankColumn] == i)
                    {
                        pianoChosenMats[i] = j;
                    }
                }
            }
            else
            {
                for (int j = 0; j < 6; j++)
                {
                    if (pianoKeyColorPositions[(j * 10) + memoryBankColumn] == i)
                    {
                        pianoIndex = j;
                        while(pianoIndex == j || pianoChosenMats.Contains(pianoIndex))
                        { 
                            pianoIndex = Rnd.Range(0, 6);
                        }
                        pianoChosenMats[i] = pianoIndex;
                    }
                }
            }
            pianoKeys[i].material = notMemoryBankMats[pianoChosenMats[i]];
        }
    }

    void colorfulMessage()
    {
        if (!moduleDown)
        {
            moduleDown = true;
            messageGameObject.transform.localPosition = new Vector3(pianoGameObject.transform.localPosition.x, pianoGameObject.transform.localPosition.y - 100f, pianoGameObject.transform.localPosition.z);
        }
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

    /*private bool isCommandValid(string cmd)
    {
        string[] validbtns = { "1", "2", "3", "4" };

        var parts = cmd.ToLowerInvariant().Split(new[] { ' ' });

        foreach (var btn in parts)
        {
            if (!validbtns.Contains(btn.ToLower()))
            {
                return false;
            }
        }
        return true;
    }

    public string TwitchHelpMessage = "Use !{0} i am a help message >:3";
    IEnumerator ProcessTwitchCommand(string cmd)
    {
        var parts = cmd.ToLowerInvariant().Split(new[] { ' ' });

        if (isCommandValid(cmd))
        {
            yield return null;
            for (int i = 0; i < parts.Count(); i++)
            {
                if (parts[i].Equals(1))
                {
                    yield return new KMSelectable[] { buttons[0] };
                }
                else if (parts[i].Equals(2))
                {
                    yield return new KMSelectable[] { buttons[1] };
                }
                else if (parts[i].Equals(3))
                {
                    yield return new KMSelectable[] { buttons[2] };
                }
                else if (parts[i].Equals(4))
                {
                    yield return new KMSelectable[] { buttons[3] };
                }
            }
        }
        else
        {
            yield break;
        }
    }*/

    void DebugMsg(string msg)
    {
        Debug.LogFormat("[...? #{0}] {1}", ModuleId, msg);
    }
}
 