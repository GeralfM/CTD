using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour {

    public GameObject selectorPrefab;
    public Selector selection { get; set; }

    public CursorHandler myCursor;
    public Reader myReader { get; set; }
    public List<Selector> mySelectors { get; set; }
    public Dictionary<string, GameObject> resourcesButton = new Dictionary<string, GameObject>();

    public Dictionary<string, int> myList;
    public Dictionary<string, int> resources;

    // Use this for initialization
    void Awake() {
        myReader = GameObject.Find("MainHandler").GetComponent<Reader>();
        myCursor = GameObject.Find("MainHandler").GetComponent<CursorHandler>();

        foreach (string str in new List<string>() { "W" })
            resourcesButton.Add(str, GameObject.Find(str + "_res"));
    }

    public void Initialize()
    {
        myList = new Dictionary<string, int>();
        resources = new Dictionary<string, int>() { { "W", 0 }, { "U", 0 }, { "B", 0 }, { "R", 0 }, { "G", 0 } };

        foreach (Transform child in GameObject.Find("Background Menu").transform)
            if (child.gameObject.name == "ObjectSelector(Clone)")
                Destroy(child.gameObject);
        mySelectors = new List<Selector>();

        SetDeck();
        
        AddResource(10);//Initial value 10
    }

    public void Selecting(Selector _selection)
    {
        selection = _selection;
        myCursor.stateCursor = "selected";
    }
    

    public void Play(Cell cible) {

        string myText, myDescr;
        if (selection != null && selection.IsPlayable())
        {
            myText = selection.transform.Find("Button").GetComponentInChildren<Text>().text;

            foreach (string key in selection.cost.Keys)
                AddResource(key, -selection.cost[key]);

            GameObject newZyx = Instantiate(Resources.Load<GameObject>("Enemies prefabs/Basic Building"), cible.gameObject.transform);
            newZyx.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            newZyx.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            newZyx.GetComponent<RectTransform>().localScale = Vector3.one;
            myDescr = GameObject.Find("MainHandler").GetComponent<Reader>().myDescr[myText][1];
            newZyx.GetComponent<Zyx_Object>().Initialize(myText, myDescr, true, GameObject.Find("Background Game").GetComponent<Grid>(), cible.myCoords, new Coord());

            selection.adjustRemaining(-1);
        }
    }

    public void AddResource(string res, int amount)
    {
        resources[res] += amount;
        resourcesButton[res].GetComponentInChildren<Text>().text = resources[res] + "";
        foreach (Selector sel in mySelectors)
            sel.IsPlayable();
    }
    public void AddResource(int amount) // to be improved
    {
        AddResource("W", amount);
    }

    public void SetDeck() {
        myList.Add("Spark watchtower", 8);
        myList.Add("Metal reparator", 2);
        myList.Add("Shock Propagator D", 2);
        myList.Add("Shock Propagator M", 2);
        myList.Add("Electrofury Citadel", 1);

        int i = 0;
        foreach (string key in myList.Keys)
        {
            GameObject newSelector = Instantiate(selectorPrefab, gameObject.transform);
            newSelector.GetComponent<RectTransform>().anchorMin = new Vector2(0.05f, 0.55f - 0.075f * i);
            newSelector.GetComponent<RectTransform>().anchorMax = new Vector2(0.45f, 0.6f - 0.075f * i);
            newSelector.GetComponent<RectTransform>().localScale = Vector3.one;

            newSelector.transform.GetChild(0).GetComponentInChildren<Text>().text = key;
            newSelector.transform.GetChild(1).GetComponent<Text>().text = myList[key] + "";
            
            newSelector.GetComponent<Selector>().resources = resources;
            newSelector.GetComponent<Selector>().setCost(myReader.myDescr[key][2]);

            newSelector.GetComponent<Selector>().adjustRemaining(myList[key]);
            mySelectors.Add(newSelector.GetComponent<Selector>());

            i++;
        }
    }
    
    public void NextTurn()
    {
        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
