using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {

    public Grid myGrid { get; set; }
    public Deck myDeck;
    public Text waveText;

    public int waveValue=0;
    public int pause=2;

    public List<Zyx_Object> allObjects = new List<Zyx_Object>();
    public GameObject zyxPrefab;

	// Use this for initialization
	void Start () {
        myGrid = GameObject.Find("Background Game").GetComponent<Grid>();
        myDeck = GameObject.Find("Background Menu").GetComponent<Deck>();
        myGrid.Initialize();

        waveText.text = "Wave : " + (waveValue==0?0:(waveValue / 5 + 1)) + "\nPause : " + pause;
    }
	
    public void ClickNewTurn()
    {
        StartCoroutine(DoATurn());
    }
    public IEnumerator DoATurn()
    {
        foreach (Zyx_Object obj in allObjects)
            obj.myCaracs.restart();
        
        int attMaxDefender = 0;
        int attMaxAttacker = 0;
        foreach (Zyx_Object obj in allObjects)
        {
            obj.PrepareAttack();
            attMaxDefender = obj.friendly ? Mathf.Max(attMaxDefender, obj.myCaracs.NB_ATT) : 0;
        }

        GoAttackers(false);
        yield return new WaitForSeconds(attMaxDefender * 0.85f); // Passe à 0 a partir d'un certain temps... WTF ?
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
        if (pause > 1)
            pause--;
        else
        {
            if (pause == 1)
                pause--;
            waveValue++;

            int toDistribute = waveValue, set, boost;
            List<int> repartition = new List<int>();
            while (toDistribute > 0)
            {
                set = Random.Range(1, Mathf.Min((waveValue - 1) / 5 + 1, toDistribute));
                toDistribute -= set;
                repartition.Add(set);

                boost = Mathf.Min(toDistribute, (waveValue - 1) / 5 + 1 - set);
                repartition.Add(boost);
                toDistribute -= boost;
            }
            //print(repartition.Count + "");
            for (int i = 0; i < repartition.Count; i += 2)
                SpawnMinion(repartition[i], repartition[i + 1]);

            //waveValue++;
            if (waveValue % 5 == 0)
                pause = 2;
        }
        waveText.text = "Wave : " + (waveValue == 0 ? 0 : (waveValue / 5 + 1)) + "\nPause : " + pause;
    }
    public void SpawnMinion(int level, int boost)
    {
        int x = 0, y = 0, rotation;
        do { int cellSpawn = Random.Range(1, 21);
            x = cellSpawn > 5 ? (cellSpawn < 16 ? (cellSpawn-1) % 5 +1: 6) : 0;
            y = cellSpawn > 5 && cellSpawn < 16 ? (cellSpawn < 11 ? 0 : 6) : (cellSpawn-1) % 5 + 1;
            rotation = cellSpawn > 5 ? (cellSpawn > 10 ? (cellSpawn > 15 ? 90 : 180) : 0) : 270;
        } while (!myGrid.allCells[new Coord(x,y)].available);
        
        //Choisir de manière random un minion dans un pool de level approprié

        GameObject newZyx = Instantiate(Resources.Load<GameObject>("Enemies prefabs/Basic Zyx"), myGrid.allCells[new Coord(x, y)].gameObject.transform);
        myGrid.allCells[new Coord(x, y)].available = false;

        newZyx.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, rotation));
        newZyx.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        newZyx.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        newZyx.GetComponent<RectTransform>().localScale = Vector3.one;

        string unitName = "Outlaw zyx";
        string descr = gameObject.GetComponent<Reader>().myDescrUnits[unitName][1];

        newZyx.GetComponent<Zyx_Object>().Initialize(unitName, descr, false, myGrid, new Coord(x, y), new Coord((rotation - 180) % 180 / 90, -(rotation - 90) % 180 / 90));
        BuffMinion(newZyx.GetComponent<Zyx_Object>(), boost);
    }
    public void BuffMinion(Zyx_Object unit, int val)
    {
        unit.myCaracs.PV += val - val / 3;
        unit.myCaracs._ATT += val / 3;
    }

    public void GoAttackers(bool attackersTurn)
    {
        List<Zyx_Object> copyAllObjects = new List<Zyx_Object>();
        foreach (Zyx_Object obj in allObjects) copyAllObjects.Add(obj);

        foreach (Zyx_Object obj in copyAllObjects) if (obj.friendly != attackersTurn) obj.DoEndOfTurnAction();
        int fallenAmount = 0;
        foreach (Cell aCell in myGrid.allCells.Values) if (aCell.fallen) fallenAmount++;
        if (fallenAmount >= 33)
            print("Game Over !");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
