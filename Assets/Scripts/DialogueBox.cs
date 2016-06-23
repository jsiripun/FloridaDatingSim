using UnityEngine;
using System.Collections;

public class DialogueBox : MonoBehaviour {

    DialogueParser parser;

    public string dialogue;
    public string name;
    public Sprite pose;
    public string position;
    public int lineJump;
    public int relNum;

    // for question options
    private string optionZero;
    private string optionOne;
    private string optionTwo;
    private string questionOption;

    //player stats
    public static Player playa;

    // for current text display
    private string currText;
    private float letterPause = 0.05f;
    private bool stillRunning;
    private IEnumerator coroutine;

    bool clickedDialogue;
    bool clickedQuestion;
    int lineNum;

    public GUIStyle customStyle, customStyleName, questionStyle, answerStyle;

	// Use this for initialization
	void Start () {
        
        dialogue = "";
        parser = GameObject.Find("DialogueParserObject").GetComponent<DialogueParser>();
        playa = GameObject.Find("Player").GetComponent<Player>();
        lineNum = 0;
        clickedDialogue = false;
        clickedQuestion = false;
        currText = "";
        stillRunning = false;
        coroutine = TypeText();
        plainDisplay();
	}
	
	// Update is called once per frame
	void Update () {

        setBackground();
       
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            position = parser.GetPosition(lineNum);
            dialogue = parser.GetContent(lineNum);

            if (position == "Q")
            {
                clickedDialogue = false;
                clickedQuestion = true;
            }
            else if (stillRunning)
            {
                StopCoroutine(coroutine);
                currText = dialogue;
                stillRunning = false;

                lineNum = lineNum + 1 + lineJump;
            }
            else
            {
                plainDisplay();
            }
        }
	    
	}

    void setBackground()
    {
        GameObject background = GameObject.Find("background");

        SpriteRenderer sr = background.GetComponent<SpriteRenderer>();

        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        background.transform.localScale = new Vector3(
            worldScreenWidth / sr.sprite.bounds.size.x * 1.75f,
            worldScreenHeight / sr.sprite.bounds.size.y * 1.75f, 0);
    }

    void plainDisplay()
    {
        ResetImages();
        clickedDialogue = true;
        currText = "";

        name = parser.GetName(lineNum);
        dialogue = parser.GetContent(lineNum);
        pose = parser.GetPose(lineNum);
        position = parser.GetPosition(lineNum);
        lineJump = parser.GetLineJump(lineNum);
        relNum = parser.GetRelationshipNum(lineNum);

        // add relationship points
        playa.addRelationshipNum(name, relNum);  

        DisplayImages();
        coroutine = TypeText();
        StartCoroutine(coroutine);


        if (lineNum > parser.GetLineCount())
            clickedDialogue = false;
        
           
    }

    IEnumerator TypeText()
    {
        stillRunning = true;
        foreach(char letter in dialogue.ToCharArray())
        {
            currText += letter;
            yield return new WaitForSeconds(letterPause);
        }
        lineNum = lineNum + 1 + lineJump;
        stillRunning = false;
    }


    void OnGUI()
    {
        customStyle.fontSize = (Screen.width + Screen.height) / 35;
        customStyleName.fontSize = (Screen.width + Screen.height) / 40;
        questionStyle.fontSize = (Screen.width + Screen.height) / 35;
        answerStyle.fontSize = (Screen.width + Screen.height) / 40;

        if (clickedDialogue)
        {
            GUI.FocusControl(null);
            dialogue = GUI.TextField(new Rect(Screen.width * (.1f), Screen.height * (.7f), Screen.width * (.8f), Screen.height * (.3f)), currText, customStyle);
            name = GUI.TextField(new Rect(Screen.width * (0), Screen.height * (.6f), Screen.width * (.2f), Screen.height * (.1f)), name, customStyleName);
        }
        
        if(clickedQuestion)
        {
            optionZero = parser.GetOptionZero(lineNum);
            optionOne = parser.GetOptionOne(lineNum);
            optionTwo = parser.GetOptionTwo(lineNum);
            lineJump = parser.GetLineJump(lineNum);
            questionOption = parser.GetContent(lineNum - 1);

            questionOption = GUI.TextField(new Rect(Screen.width * (.1f), Screen.height * (0), Screen.width * (.8f), Screen.height * (.3f)), questionOption, questionStyle);

            if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.32f), Screen.width * (.6f), Screen.height * (.2f)), optionZero, answerStyle))
            {
                // option 0
                lineNum = lineNum + 1;
                clickedQuestion = false;
                clickedDialogue = true;
                plainDisplay();
            }

            if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.53f), Screen.width * (.6f), Screen.height * (.2f)), optionOne, answerStyle))
            {
                // option 1
                lineNum = lineNum + 2;
                clickedQuestion = false;
                clickedDialogue = true;
                plainDisplay();
            }

            if (GUI.Button(new Rect(Screen.width * (.2f), Screen.height * (.74f), Screen.width * (.6f), Screen.height * (.2f)), optionTwo, answerStyle))
            {
                // option 2
                lineNum = lineNum + 3;
                clickedQuestion = false;
                clickedDialogue = true;
                plainDisplay();
            }
        }
    }

    void ResetImages()
    {
        if(name != "")
        {
            GameObject character = GameObject.Find(name);
            SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
            currSprite.sprite = null;
        }
    }

    void DisplayImages()
    {
        if(name == "???")
        {
            GameObject character = GameObject.Find("???");

            SetSpritePositions(character);

            SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
            currSprite.sprite = pose;
        }
        if(name != "")
        {
            GameObject character = GameObject.Find(name);

            SetSpritePositions(character);

            SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
            currSprite.sprite = pose;
        }
    }

    void SetSpritePositions(GameObject spriteObj)
    {
        if(position == "L")
        {
            spriteObj.transform.position = new Vector3(-4, 2, 0);
        }
        else if(position == "R")
        {
            spriteObj.transform.position = new Vector3(3, 2, 0);
        }
    }
}
