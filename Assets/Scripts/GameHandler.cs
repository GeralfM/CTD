using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {

    public Grid myGrid { get; set; }
    public Deck myDeck { get; set; }
    public Text waveText;
    public Text overText;

    public int waveValue { get; set; }
    public int waveGlobal { get; set; }
    public int pause { get; set; }

    public List<Zyx_Object> allObjects { get; set; }
    public GameObject zyxPrefab;

    public GameObject gameOverScreen;

	// Use this for initialization
	void Start () {
        myGrid = GameObject.Find("Background Game").GetComponent<Grid>();
        myDeck = GameObject.Find("Background Menu").GetComponent<Deck>();

        Initialize();
    }

    public void Initialize()
    {
        foreach (Transform child in myGrid.gameObject.transform) if(child.gameObject.name=="Cell(Clone)")
            Destroy(child.gameObject);
        foreach (Transform child in myDeck.gameObject.transform) if (child.gameObject.name == "Carte(Clone)")
            Destroy(child.gameObject);

        allObjects = new List<Zyx_Object>();

        myGrid.Initialize();
        myDeck.Initialize();

        waveValue = 1; waveGlobal = 1;
        pause = 0;
        waveText.text = "Wave : " + waveGlobal + "\nPause : " + pause;
    }

    public void ClickNewTurn()
    {
        StartCoroutine(DoATurn());
    }
    public IEnumerator DoATurn()
    {
        int attMaxDefender = 0;
        int attMaxAttacker = 0;

        foreach (Zyx_Object obj in allObjects)
        {
            obj.myCaracs.restart();
            obj.PrepareAttack();
            attMaxDefender = obj.friendly ? Mathf.Max(attMaxDefender, obj.myCaracs.NB_ATT) : attMaxDefender;
        }

        GoAttackers(false);
        yield return new WaitForSeconds(attMaxDefender * 0.85f);
        GoAttackers(true);
        yield return new WaitForSeconds(.5f);
        NextWave();
        yield return new WaitForSeconds(.5f);
        myDeck.NextTurn(); //Inclut la phase de draw 
        // Mainphase joueur. La suite au prochain bouton NextTurn
        GameObject.Find("NextTurnButton").GetComponent<Button>().interactable = true;
    }
    public void NextWave()
    {
        myDeck.AddResource(1);

        if (pause > 0)
            pause--;
        else
        {
            int toDistribute = (waveGlobal - 1) * 5 + waveValue, set, boost;
            List<int> repartition = new List<int>();
            while (toDistribute > 0)
            {
                set = Random.Range(1, Mathf.Min(waveGlobal, toDistribute) + 1);
                toDistribute -= set;
                repartition.Add(set);

                boost = Mathf.Min(waveGlobal - set, toDistribute);
                repartition.Add(boost);
                toDistribute -= boost;
            }

            for (int i = 0; i < repartition.Count; i += 2)
                SpawnMinion(repartition[i], repartition[i + 1]);

            if (waveValue == 5)
            {
                pause = 2; waveValue = 0;
                waveGlobal++;
            }

            waveValue++;
        }
        
        waveText.text = "Wave : " + waveGlobal + "\nPause : " + pause;
    }
    public void SpawnMinion(int level, int boost)
    {
        int x = 0, y = 0, nbTry=0, rotation;
        do { int cellSpawn = Random.Range(1, 21);
            x = cellSpawn > 5 ? (cellSpawn < 16 ? (cellSpawn-1) % 5 +1: 6) : 0;
            y = cellSpawn > 5 && cellSpawn < 16 ? (cellSpawn < 11 ? 0 : 6) : (cellSpawn-1) % 5 + 1;
            rotation = cellSpawn > 5 ? (cellSpawn > 10 ? (cellSpawn > 15 ? 90 : 180) : 0) : 270;
            nbTry++;
        } while (!myGrid.allCells[new Coord(x,y)].available && nbTry<=20);

        if (nbTry <= 20)
        {
            GameObject newZyx = Instantiate(Resources.Load<GameObject>("Enemies prefabs/Basic Zyx"), GameObject.Find("Background Game").transform);
            myGrid.allCells[new Coord(x, y)].available = false;

            newZyx.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, rotation));
            newZyx.GetComponent<RectTransform>().anchorMin = myGrid.allCells[new Coord(x, y)].gameObject.GetComponent<RectTransform>().anchorMin;
            newZyx.GetComponent<RectTransform>().anchorMax = myGrid.allCells[new Coord(x, y)].gameObject.GetComponent<RectTransform>().anchorMax;
            newZyx.GetComponent<RectTransform>().localScale = Vector3.one;

            string unitName = ChooseMinion(level);
            string descr = gameObject.GetComponent<Reader>().myDescrUnits[unitName][1];

            newZyx.GetComponent<Zyx_Object>().Initialize(unitName, descr, false, myGrid, new Coord(x, y), new Coord((rotation - 180) % 180 / 90, -(rotation - 90) % 180 / 90));
            BuffMinion(newZyx.GetComponent<Zyx_Object>(), boost);
        }
    }
    public string ChooseMinion(int level) // A retravailler
    {
        if (level == 1)
            return "Outlaw Zyx";
        else
            return Random.Range(0f, 1f) > 0.5f ? "Cryozyx" : "Pike Zyx";

    }
    public void BuffMinion(Zyx_Object unit, int val)
    {
        unit.myCaracs.PV += val - val / 3;
        foreach (string action in unit.myCaracs.actions.Keys)
            unit.myCaracs.actions[action][0] += val / 3;
    }

    public void GoAttackers(bool attackersTurn)
    {
        List<Zyx_Object> copyAllObjects = new List<Zyx_Object>();
        foreach (Zyx_Object obj in allObjects) copyAllObjects.Add(obj);

        foreach (Zyx_Object obj in copyAllObjects) if (obj.friendly != attackersTurn) obj.DoEndOfTurnAction();
        int fallenAmount = 0;
        foreach (Cell aCell in myGrid.allCells.Values) if (aCell.fallen) fallenAmount++;
        overText.text = (33 - fallenAmount) + " losses remaining";
        if (fallenAmount >= 33)
            DisplayScreen(gameOverScreen);
    }

    //============================================== UI HANDLER =====================================================

    public void DisplayScreen(GameObject screen)
    {
        screen.SetActive(!screen.activeSelf);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    //============================================== DEBUGGER =====================================================

    public void doDebug(string str)
    {
        GameObject.Find("Debug").GetComponent<Text>().text = str;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
