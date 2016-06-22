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
        if (clickedDialogue)
        {
            GUI.FocusControl(null);
            dialogue = GUI.TextField(new Rect(100, 400, 600, 200), currText, customStyle);
            name = GUI.TextField(new Rect(0, 350, 200, 50), name, customStyleName);
        }
        
        if(clickedQuestion)
        {
            optionZero = parser.GetOptionZero(lineNum);
            optionOne = parser.GetOptionOne(lineNum);
            optionTwo = parser.GetOptionTwo(lineNum);
            lineJump = parser.GetLineJump(lineNum);
            questionOption = parser.GetContent(lineNum - 1);

            questionOption = GUI.TextField(new Rect(50, 0, 550, 150), questionOption, questionStyle);

            if (GUI.Button(new Rect(200, 230, 400, 100), optionZero, answerStyle))
            {
                // option 0
                lineNum = lineNum + 1;
                clickedQuestion = false;
                clickedDialogue = true;
                plainDisplay();
            }

            if (GUI.Button(new Rect(200, 340, 400, 100), optionOne, answerStyle))
            {
                // option 1
                lineNum = lineNum + 2;
                clickedQuestion = false;
                clickedDialogue = true;
                plainDisplay();
            }

            if (GUI.Button(new Rect(200, 450, 400, 100), optionTwo, answerStyle))
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
