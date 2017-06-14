using System.Collections;
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
        foreach (string key in cost.Keys)
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
        for (int i = 0; i < data.Length; i += 2)
            cost.Add(data[i]+"", System.Int32.Parse(data[i + 1]+""));
        IsPlayable();
    }
	
    public void Hovered(bool isOn)
    {
        if (myCursor.stateCursor == "none")
            DisplayInfos(isOn);
    }
    public void DisplayInfos(bool yes)
    {
        name = gameObject.transform.GetChild(0).GetComponentInChildren<Text>().text;
        infosPanel.transform.Find("Text").GetComponent<Text>().text = yes ? name + "\n\n" + myReader.myDescr[name][0] : "No selection";
        infosPanel.transform.Find("Cost").GetComponentInChildren<Text>().text = yes ? myReader.myDescr[name][2].Substring(1, myReader.myDescr[name][2].Length - 1) : "#";
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
