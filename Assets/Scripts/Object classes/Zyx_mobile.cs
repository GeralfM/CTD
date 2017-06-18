using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zyx_mobile : Zyx_Object {

    public bool isMoving = false;
    public Vector3 initial;
    public Vector3 target;

    // Use this for initialization
	void Start () {
		
	}

    public override void DoEndOfTurnAction()
    {
        if (!myGrid.allCells[myCoords + myOrientation].fallen)
            base.DoEndOfTurnAction();
        
        else if (myGrid.allCells[myCoords + myOrientation].available)
            Move();
        
    }
    public override IEnumerator Attack()
    {
        foreach (Cell aCell in getZone(myCaracs.zoneAtt,myCaracs.RANGE))
            aCell.addToPV(-myCaracs.ATT);
        yield return StartCoroutine(base.Attack());
    }
        public void Move()
    {
        initial = gameObject.transform.position;

        myGrid.allCells[myCoords].available = true;
        myGrid.allCells[myCoords].occupant = null;

        myCoords = myCoords + myOrientation;
        target = myGrid.allCells[myCoords].gameObject.transform.position;
        isMoving = true;

        myGrid.allCells[myCoords].available = false;
        myGrid.allCells[myCoords].occupant = this;
    }

    public override void DisplayInfos(bool yes)
    {
        GameObject infosPanel = GameObject.Find("Description");
        infosPanel.transform.Find("Text").GetComponent<Text>().text = yes ? myName + "\n\nPV : " + myCaracs.PV 
            + "\nNB ATT : " + myCaracs.NB_ATT + "     DAMAGE : " + myCaracs.ATT : "No selection";
        infosPanel.transform.Find("Cost").GetComponentInChildren<Text>().text = "#";

        GameObject.Find("Background Menu").GetComponent<Deck>().selection = null;
    }

    // Update is called once per frame
    void Update () {
        if (isMoving)
        {
            if((Mathf.Abs((transform.position - initial).x / (target - initial).x) <= 1f || (initial.x == target.x)) &&
            (Mathf.Abs((transform.position - initial).y / (target - initial).y) <= 1f || (initial.y == target.y)))
                transform.position += (target - initial) * 1.5f * Time.deltaTime;
            else
                isMoving = false;
        }
    }
}
