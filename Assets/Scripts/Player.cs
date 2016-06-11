using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public List<string> allMyBoos = new List<string>();
    public bool booBobert;

    void Start()
    {
        booBobert = false;
    }


    public string getBooStatusOf(string booName)
    {

        if (allMyBoos.Contains(booName))
            return "You hooked it up with " + booName + "!";
        else
            return "You messed it up with " + booName + "... way to go slick";
    }

    public void getAllBooStatus()
    {
        foreach(string boo in allMyBoos)
        {

        }
    }

    public void setBoo(bool gotBoo, string booName)
    {
        if (gotBoo)
        {
            allMyBoos.Add(booName);
        }
        else
        {
            allMyBoos.Remove(booName);
        }
    }
}
