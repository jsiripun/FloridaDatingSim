﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public static int relationshipLvlBella = 0;
    public static string currentDialogueLoad = "DialogueOnTrain.txt";
    //public static string currentDialogueLoad = "DialogueAbuelaHome_Start.txt"; //change after done testing

    public void addRelationshipNum(string relName, int relNum)
    {
        if (relName == "Bella")
            relationshipLvlBella += relNum;
        
    }

    public void loadNextDialogue (string dialogueToLoad)
    {
        currentDialogueLoad = dialogueToLoad;
    }

    public string getCurrentDialogue()
    {
        return currentDialogueLoad;
    }
}
