using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zyx_Object : MonoBehaviour {

    public Grid myGrid { get; set; }
    public Coord myCoords { get; set; }
    public Coord myOrientation { get; set; }

    public bool friendly { get; set; }
    public int myAttack { get; set; }

	// Use this for initialization
	void Start () {
		
	}

    public void Initialize(string name, bool _friendly, Grid _grid, Coord _coords, Coord _orientation)
    {
        myGrid = _grid;
        myCoords = _coords;
        myOrientation = _orientation;
        friendly = _friendly;

        myAttack = 1;

        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(name);

        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
    }
    public void DoEndOfTurnAction()
    {
        Attack();
    }
    public void Attack()
    {
        if (!myGrid.allCells[myCoords + myOrientation].fallen)
            myGrid.allCells[myCoords + myOrientation].addToPV(-myAttack);
        else if (myGrid.allCells[myCoords + myOrientation].available)
            GoForward();
    }
    public void GoForward()
    {
        myGrid.allCells[myCoords].available = true;
        myCoords = myCoords + myOrientation;
        myGrid.allCells[myCoords].available = false;
        gameObject.transform.SetParent(myGrid.allCells[myCoords].gameObject.transform);

        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
