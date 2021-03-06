﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour {

    public Dictionary<string, int> cost = new Dictionary<string, int>();
    public Dictionary<string, int> resources;

    public GameObject infosPanel;
    public Reader myReader;
    public CursorHandler myCursor;
    
    public int remaining=0;

    // Use this for initialization
    void Start () {
        infosPanel = GameObject.Find("Description");
        myReader = GameObject.Find("MainHandler").GetComponent<Reader>();
        myCursor = GameObject.Find("MainHandler").GetComponent<CursorHandler>();
	}

    public void signalMyself()
    {
        GameObject.Find("Background Menu").GetComponent<Deck>().Selecting(this);
        DisplayInfos(true);
    }
    public bool IsPlayable()
    {
        bool result = remaining > 0;
        if(new List<string>(cost.Keys).Contains("T"))
        {
            int total = 0;
            foreach (string key in resources.Keys)
                total += resources[key];
            result = result && cost["T"] <= total;
        }

        foreach (string key in cost.Keys) if (key != "T")
            result = result && cost[key] <= resources[key];
        transform.GetComponentInChildren<Button>().interactable = result;
        return result;
    }
    public void adjustRemaining(int val)
    {
        remaining += val;
        gameObject.transform.GetChild(1).GetComponent<Text>().text = remaining+"";
    }

    public void setCost(string data)
    {
        string[] parsed = data.Split(new string[] { "%" }, System.StringSplitOptions.None);
        for (int i = 0; i < parsed.Length; i += 2)
            cost.Add(parsed[i][0] + "", System.Int32.Parse(parsed[i].Substring(1, parsed[i].Length - 1)));
        IsPlayable();
    }
	
    public void Hovered(bool isOn)
    {
        if (myCursor.stateCursor == "none")
            DisplayInfos(isOn);
    }
    public void DisplayInfos(bool yes)
    {
        string myName = gameObject.transform.GetChild(0).GetComponentInChildren<Text>().text;
        infosPanel.transform.Find("Text").GetComponent<Text>().text = yes ? myName + "\n\n" + myReader.myDescr[myName][0] : "No selection";
        infosPanel.transform.Find("Cost").GetComponentInChildren<Text>().text = yes ? myReader.myDescr[myName][2].Substring(1, myReader.myDescr[myName][2].Length - 1) : "#";
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject.Find("Background Menu").GetComponent<Deck>().selection = null;
            myCursor.stateCursor = "none";
            DisplayInfos(false);
        }
            
    }
}
