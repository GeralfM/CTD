using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zyx_mobile : Zyx_Object {

	// Use this for initialization
	void Start () {
		
	}

    public override void DoEndOfTurnAction()
    {
        StartCoroutine(Attack());
    }
    public override IEnumerator Attack()
    {
        StartCoroutine(base.Attack());

        if (!myGrid.allCells[myCoords + myOrientation].fallen)
            myGrid.allCells[myCoords + myOrientation].addToPV(-myCaracs.ATT);
        else if (myGrid.allCells[myCoords + myOrientation].available)
            Move();
        yield return new WaitForSeconds(0f);
    }
    public void Move()
    {
        myGrid.allCells[myCoords].available = true;
        myGrid.allCells[myCoords].occupant = null;
        myCoords = myCoords + myOrientation;
        myGrid.allCells[myCoords].available = false;
        myGrid.allCells[myCoords].occupant = this;
        gameObject.transform.SetParent(myGrid.allCells[myCoords].gameObject.transform);

        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
