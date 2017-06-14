using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zyx_Object : MonoBehaviour {

    public Grid myGrid { get; set; }
    public Cond_Eff_Handler myHelper { get; set; }
    public Coord myCoords { get; set; }
    public Coord myOrientation { get; set; }

    public bool friendly { get; set; }

    public Caracteristics myCaracs = new Caracteristics();

    public GameObject lazor;

	// Use this for initialization
	void Start () {
		
	}

    public void Initialize(string name, string infosBuild, bool _friendly, Grid _grid, Coord _coords, Coord _orientation)
    {
        myGrid = _grid;
        myHelper = GameObject.Find("MainHandler").GetComponent<Cond_Eff_Handler>();
        myCoords = _coords;
        myOrientation = _orientation;
        friendly = _friendly;

        myGrid.allCells[myCoords].available = false;
        myGrid.allCells[myCoords].occupant = this;
        GameObject.Find("MainHandler").GetComponent<GameHandler>().allObjects.Add(this);

        Build(infosBuild);

        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(name);

        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
    }
    public virtual void DoEndOfTurnAction()
    {
        foreach(string action in myCaracs.actions)
        {
            switch (action)
            {
                case "Attack":
                    StartCoroutine(Attack());
                    break;
                case "Heal":
                    Heal();
                    break;
                default:
                    break;
            }
        }
    }
    public void PrepareAttack()
    {
        myHelper.TestCE(this);
    }
    public virtual IEnumerator Attack()
    {
        yield return new WaitForSeconds(0f);
    }
    public void Heal()
    {
        foreach (Cell aCell in getZone(myCaracs.zoneAtt, myCaracs.RANGE)) if (!aCell.fallen)
                aCell.addToPV(myCaracs._ATT); // Et pas ATT qui peut varier...
    }
    public void ShootToTarget(GameObject _target)
    {
        GameObject newShot = Instantiate(lazor, gameObject.transform);
        newShot.GetComponent<Shot>().Go(_target.transform.position, newShot.transform.position);
        newShot.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        newShot.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        newShot.GetComponent<RectTransform>().localScale = Vector3.one*0.1f;
    }
    public void AddToPV(int val)
    {
        myCaracs.PV = Mathf.Min(myCaracs.PV_MAX, myCaracs.PV + val);
        if (myCaracs.PV <= 0)
            IsDestroyed();
    }
    public void IsDestroyed()
    {
        myGrid.allCells[myCoords].occupant = null;
        myGrid.allCells[myCoords].available = true;
        GameObject.Find("MainHandler").GetComponent<GameHandler>().allObjects.Remove(this);
        Destroy(gameObject);
    }

    public List<Cell> getZone(string typeZone, int range)
    {
        List<Cell> result = new List<Cell>();
        switch (typeZone)
        {
            case "circle":
                for (int i = -range; i <= range; i++)
                    for (int j = -range; j <= range; j++)
                        if ((i != 0 || j != 0) && myGrid.allCells.ContainsKey(myCoords + new Coord(i, j))) result.Add(myGrid.allCells[myCoords + new Coord(i, j)]);
                break;
            case "front":
                for (int i = 1; i <= Mathf.Abs(range); i++)
                    if (myGrid.allCells.ContainsKey(myCoords + (int)Mathf.Sign(range) * i * myOrientation)) result.Add(myGrid.allCells[myCoords + (int)Mathf.Sign(range) * i * myOrientation]);
                break;
            default:
                break;
        }
        if (myCaracs.tempActions.Contains("Opposite") && range > 0)
            result.AddRange(getZone(typeZone, -range));
        return result;
    }

    public void Build(string data)
    {
        string[] descr = data.Split(new string[] { ":" }, System.StringSplitOptions.None);
        myCaracs.PV_MAX = System.Int32.Parse(descr[0]); myCaracs.PV = myCaracs.PV_MAX;
        myCaracs._ATT = System.Int32.Parse(descr[1]);
        myCaracs._NB_ATT = System.Int32.Parse(descr[2]);
        myCaracs.zoneAtt = descr[3];
        myCaracs._RANGE = System.Int32.Parse(descr[4]);

        myCaracs.actions.Add(descr[5]);

        if (descr[6] != "")
            myCaracs.conditions.Add(descr[6]);
        if (descr[7] != "")
            myCaracs.effects.Add(descr[7]);
        
        myCaracs.restart();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
