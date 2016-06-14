using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public static int relationshipLvlBella = 0;

    public void addRelationshipNum(string relName, int relNum)
    {
        if (relName == "Bella")
            relationshipLvlBella += relNum;
        
    }
}
