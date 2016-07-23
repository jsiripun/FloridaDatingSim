﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

public class DialogueParser : MonoBehaviour {

    public string fileName;

    List<DialogueLine> lines;
    List<Sprite> chris;
    List<Sprite> bella;
    Sprite mystery;

    struct DialogueLine
    {
        public string name;
        public string content;
        public string pose;
        public string position;
        public int lineJump;
        public int relationshipNum;

        public DialogueLine(string n, string c, string p, string pos, int lj, int relnum)
        {
            name = n;
            content = c;
            pose = p;
            position = pos;
            lineJump = lj;
            relationshipNum = relnum;
        }
    }

	// Use this for initialization
	void Start () {
        lines = new List<DialogueLine>();
        chris = new List<Sprite>();
        bella = new List<Sprite>();

        mystery = (Sprite)Resources.Load("Sprites/Characters/mystery", typeof(Sprite));


        LoadDialogue(fileName);
        LoadImages();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int GetLineCount()
    {
        return lines.Count;
    }

    public string GetName(int lineNumber)
    {
        if (lineNumber < lines.Count)
            return lines[lineNumber].name;

        return "";
    }

    public string GetContent(int lineNumber)
    {
        if(lineNumber < lines.Count)
            return lines[lineNumber].content;

        return "";
    }

    public Sprite GetPose(int lineNumber)
    {
        /*
        if (lineNumber < lines.Count)
            return images[int.Parse(lines[lineNumber].pose)];

        return null;
        */
        string temp = GetName(lineNumber);
        if (lineNumber < lines.Count)
            if (temp.Equals("Chris"))
                return chris[int.Parse(lines[lineNumber].pose)];
            else if (temp.Equals("Bella"))
                return bella[int.Parse(lines[lineNumber].pose)];
            else if (temp.Equals("???"))
                return mystery;

        return null;
    }

    public string GetPosition (int lineNumber)
    {
        if (lineNumber < lines.Count)
            return lines[lineNumber].position;

        return "";
    }

    public int GetLineJump(int lineNumber)
    {
        if (lineNumber < lines.Count)
            return lines[lineNumber].lineJump;

        return 0;
    }

    public string GetOptionZero(int lineNumber)
    {
        if (lineNumber < lines.Count)
            return lines[lineNumber].name;

        return "";
    }

    public string GetOptionOne(int lineNumber)
    {
        if (lineNumber < lines.Count)
            return lines[lineNumber].content;

        return "";
    }

    public string GetOptionTwo(int lineNumber)
    {
        if (lineNumber < lines.Count)
            return lines[lineNumber].pose;

        return "";
    }
   
    public int GetRelationshipNum(int lineNumber)
    {
        if (lineNumber < lines.Count)
            return lines[lineNumber].relationshipNum;

        return 0;
    }

    void LoadDialogue(string filename)
    {
        string file = "Assets/Resources/Dialogue/" + filename;
        string line;
        StreamReader r = new StreamReader(file);

        using (r)
        {
            do
            {
                line = r.ReadLine();
                if (line != null)
                {
                    string[] line_values = SplitCsvLine(line);
                    DialogueLine line_entry = new DialogueLine(line_values[0], line_values[1], line_values[2], line_values[3], int.Parse(line_values[4]), int.Parse(line_values[5]));
                    lines.Add(line_entry);
                }
            } while (line != null);

            r.Close();
        }
    }

    string[] SplitCsvLine(string line)
    {
        string pattern = @"
     # Match one value in valid CSV string.
     (?!\s*$)                                      # Don't match empty last value.
     \s*                                           # Strip whitespace before value.
     (?:                                           # Group for value alternatives.
       '(?<val>[^'\\]*(?:\\[\S\s][^'\\]*)*)'       # Either $1: Single quoted string,
     | ""(?<val>[^""\\]*(?:\\[\S\s][^""\\]*)*)""   # or $2: Double quoted string,
     | (?<val>[^,'""\s\\]*(?:\s+[^,'""\s\\]+)*)    # or $3: Non-comma, non-quote stuff.
     )                                             # End group of value alternatives.
     \s*                                           # Strip whitespace after value.
     (?:,|$)                                       # Field ends on comma or EOS.
     ";

        string[] values = (from Match m in Regex.Matches(line, pattern, RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline)
                           select m.Groups[1].Value).ToArray();

        return values;
    }

    void LoadImages()
    {
        // currently only loads the sprites that will be used in that script
        // other option is load all sprites.. then hardcore each into the dialogue

        /*
        for(int i = 0; i < lines.Count; i++)
        {
            string imageName = lines[i].name;
            if(imageName == "???")
            {
                Sprite unknown = (Sprite)Resources.Load("Sprites/Characters/Chris1", typeof(Sprite));
                if (!images.Contains(unknown))
                    images.Add(unknown);
            }
            else
            {
                Sprite image = (Sprite)Resources.Load("Sprites/Characters/" + imageName, typeof(Sprite));
                if (!images.Contains(image))
                {
                    images.Add(image);
                }
            }
        }
        */

        // hardcode for now
        // chris
        Sprite chris_basic = (Sprite)Resources.Load("Sprites/Characters/chris/chris_basic", typeof(Sprite));
        Sprite chris_laugh = (Sprite)Resources.Load("Sprites/Characters/chris/chris_laugh", typeof(Sprite));
        Sprite chris_pleased = (Sprite)Resources.Load("Sprites/Characters/chris/chris_pleased", typeof(Sprite));
        Sprite chris_angry = (Sprite)Resources.Load("Sprites/Characters/chris/chris_angry", typeof(Sprite));
        Sprite chris_sad = (Sprite)Resources.Load("Sprites/Characters/chris/chris_sad", typeof(Sprite));
        Sprite chris_confused = (Sprite)Resources.Load("Sprites/Characters/chris/chris_confused", typeof(Sprite));

        chris.Add(chris_basic); // 0
        chris.Add(chris_laugh); // 1
        chris.Add(chris_pleased); // 2
        chris.Add(chris_angry); // 3
        chris.Add(chris_sad); // 4
        chris.Add(chris_confused); // 5

        //bella
        Sprite bella_basic = (Sprite)Resources.Load("Sprites/Characters/bella/bella_basic", typeof(Sprite));
        Sprite bella_laugh = (Sprite)Resources.Load("Sprites/Characters/bella/bella_laugh", typeof(Sprite));
        Sprite bella_pleased = (Sprite)Resources.Load("Sprites/Characters/bella/bella_pleased", typeof(Sprite));
        Sprite bella_angry = (Sprite)Resources.Load("Sprites/Characters/bella/bella_angry", typeof(Sprite));
        Sprite bella_sad = (Sprite)Resources.Load("Sprites/Characters/bella/bella_sad", typeof(Sprite));
        Sprite bella_confused = (Sprite)Resources.Load("Sprites/Characters/bella/bella_confused", typeof(Sprite));

        bella.Add(bella_basic); // 6
        bella.Add(bella_laugh); // 7
        bella.Add(bella_pleased); // 8
        bella.Add(bella_angry); // 9
        bella.Add(bella_sad); // 10
        bella.Add(bella_confused); // 11

    }

}
