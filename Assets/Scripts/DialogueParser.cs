using UnityEngine;
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
    List<Sprite> images;

    struct DialogueLine
    {
        public string name;
        public string content;
        public string pose;
        public string position;
        public int lineJump;

        public DialogueLine(string n, string c, string p, string pos, int lj)
        {
            name = n;
            content = c;
            pose = p;
            position = pos;
            lineJump = lj;
        }
    }

	// Use this for initialization
	void Start () {
        lines = new List<DialogueLine>();
        images = new List<Sprite>();

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
        if (lineNumber < lines.Count)
            return images[int.Parse(lines[lineNumber].pose)];

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
   

    void LoadDialogue(string filename)
    {
        string file = "Assets/Dialogue/" + filename;
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
                    DialogueLine line_entry = new DialogueLine(line_values[0], line_values[1], line_values[2], line_values[3], int.Parse(line_values[4]));
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
        for(int i = 0; i < lines.Count; i++)
        {
            string imageName = lines[i].name;
            if(imageName != "???")
            {
                Sprite image = (Sprite)Resources.Load("Sprites/" + imageName, typeof(Sprite));
                if (!images.Contains(image))
                {
                    images.Add(image);
                }
            }
            else
            {
                Sprite unknown = (Sprite)Resources.Load("Sprites/Unknown", typeof(Sprite));
                if(!images.Contains(unknown))
                    images.Add(unknown);
            }
        }
    }

}
