using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    public Grid myGrid { get; set; }
    public Deck myDeck;

    public List<Zyx_Object> allObjects = new List<Zyx_Object>();
    public GameObject zyxPrefab;

	// Use this for initialization
	void Start () {
        myGrid = GameObject.Find("Background Game").GetComponent<Grid>();
        myDeck = GameObject.Find("Background Menu").GetComponent<Deck>();
        myGrid.Initialize();
	}
	
    public void DoATurn()
    {
        // Effects defender
        GoAttackers();
        SpawnMinion();
        myDeck.NextTurn(); //Inclut la phase de draw 
        // Mainphase joueur. La suite au prochain bouton NextTurn
    }
    public void SpawnMinion()
    {
        int x = 0, y = 0, rotation;
        do { int cellSpawn = Random.Range(1, 21);
            x = cellSpawn > 5 ? (cellSpawn < 16 ? (cellSpawn-1) % 5 +1: 6) : 0;
            y = cellSpawn > 5 && cellSpawn < 16 ? (cellSpawn < 11 ? 0 : 6) : (cellSpawn-1) % 5 + 1;
            rotation = cellSpawn > 5 ? (cellSpawn > 10 ? (cellSpawn > 15 ? 90 : 180) : 0) : 270;
        } while (!myGrid.allCells[new Coord(x,y)].available);
        
        GameObject newZyx = Instantiate(zyxPrefab, myGrid.allCells[new Coord(x, y)].gameObject.transform);
        myGrid.allCells[new Coord(x, y)].available = false;

        newZyx.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, rotation));
        newZyx.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        newZyx.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        newZyx.GetComponent<RectTransform>().localScale = Vector3.one;

        newZyx.GetComponent<Zyx_Object>().Initialize("Outlaw zyx", false, myGrid, new Coord(x, y), new Coord((rotation - 180) % 180 / 90, -(rotation - 90) % 180 / 90));
        allObjects.Add(newZyx.GetComponent<Zyx_Object>());
    }
    public void GoAttackers()
    {
        foreach (Zyx_Object obj in allObjects) if (!obj.friendly) obj.DoEndOfTurnAction();
        int fallenAmount = 0;
        foreach (Cell aCell in myGrid.allCells.Values) if (aCell.fallen) fallenAmount++;
        if (fallenAmount >= 33)
            print("Game Over !");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
