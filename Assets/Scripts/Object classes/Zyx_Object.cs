using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zyx_Object : MonoBehaviour {

    public string myName { get; set; }

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

    public void Initialize(string _name, string infosBuild, bool _friendly, Grid _grid, Coord _coords, Coord _orientation)
    {
        myName = _name;
        myGrid = _grid;
        myHelper = GameObject.Find("MainHandler").GetComponent<Cond_Eff_Handler>();
        myCoords = _coords;
        myOrientation = _orientation;
        friendly = _friendly;

        myGrid.allCells[myCoords].available = false;
        myGrid.allCells[myCoords].occupant = this;
        GameObject.Find("MainHandler").GetComponent<GameHandler>().allObjects.Add(this);

        Build(infosBuild);

        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(myName);

        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
    }
    public virtual void DoEndOfTurnAction()
    {
        if (!new List<string>(myCaracs.effectsOverTime.Keys).Contains("Freeze"))
            foreach (string act in myCaracs.actions.Keys)
                switch (act)
                {
                    case "Attack":
                        StartCoroutine(Attack());
                        break;
                    case "Heal":
                        Heal();
                        break;
                    case "Freeze":
                        Freeze();
                        break;
                    case "Kamikaze":
                        Kamikaze();
                        break;
                    default:
                        break;
                }

        else
        if (CheckEffect_IsFinished("Freeze")) gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);

    }

    //====================================== IN GAME ==================================================

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
                aCell.addToPV(myCaracs.actions["Heal"][0]); // Et pas ATT qui peut varier...
    }
    public void Freeze() // to be generalized
    {
        foreach (Cell aCell in getZone(myCaracs.zoneAtt, myCaracs.RANGE))
            if(AddEffect(aCell.occupant,"Freeze")) aCell.occupant.gameObject.GetComponent<Image>().color = new Color(116f / 255, 234f / 255, 240f / 255);
    }
    public void Kamikaze()
    {
        foreach (Cell aCell in getZone(myCaracs.zoneAtt, myCaracs.RANGE)) if (!aCell.fallen)
                aCell.addToPV(-myCaracs.actions["Kamikaze"][0]);
        IsDestroyed();
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

    public List<Cell> getZone(string typeZone, int range)
    {
        List<Cell> result = new List<Cell>();
        switch (typeZone) // check if the Sign is useful in calculus
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
            case "cross":
                for (int i = -range; i <= Mathf.Abs(range); i++)
                    if (myGrid.allCells.ContainsKey(myCoords + (int)Mathf.Sign(range) * i * myOrientation)) result.Add(myGrid.allCells[myCoords + (int)Mathf.Sign(range) * i * myOrientation]);
                for (int i = -range; i <= Mathf.Abs(range); i++)
                    if (myGrid.allCells.ContainsKey(myCoords + (int)Mathf.Sign(range) * i * new Coord(-myOrientation.y, myOrientation.x) ))
                        result.Add(myGrid.allCells[myCoords + (int)Mathf.Sign(range) * i * new Coord(-myOrientation.y, myOrientation.x) ]);
                break;
            default:
                break;
        }
        if (myCaracs.tempActions.Contains("Opposite") && range > 0)
            result.AddRange(getZone(typeZone, -range));
        return result;
    }

    public bool AddEffect(Zyx_Object target, string eff)
    {
        if (target != null && (target.friendly != friendly) && CheckDelay(eff))
        {
            if (new List<string>(target.myCaracs.effectsOverTime.Keys).Contains(eff))
                target.myCaracs.effectsOverTime[eff] += myCaracs.actions[eff][0];
            else target.myCaracs.effectsOverTime.Add(eff, 1);
            return true;
        }
        return false;
    }
    public bool CheckEffect_IsFinished(string eff)
    {
        myCaracs.effectsOverTime[eff]--;
        if (myCaracs.effectsOverTime[eff] == 0)
        {
            myCaracs.effectsOverTime.Remove(eff);
            return true;
        }
        return false;
    }
    public bool CheckDelay(string act)
    {
        if (myCaracs.actionsDelay[act] == myCaracs.actions[act][1])
        {
            myCaracs.actionsDelay[act] = 1;
            return true;
        }
        else
        {
            myCaracs.actionsDelay[act]++;
            return false;
        }
    }

    //====================================== OTHER FEATURES ==================================================
    
    public void IsDestroyed()
    {
        myGrid.allCells[myCoords].occupant = null;
        myGrid.allCells[myCoords].available = true;
        GameObject.Find("MainHandler").GetComponent<GameHandler>().allObjects.Remove(this);
        Destroy(gameObject);
    }
    
    public void Build(string data)
    {
        string[] descr = data.Split(new string[] { ":" }, System.StringSplitOptions.None);
        myCaracs.PV_MAX = System.Int32.Parse(descr[0]); myCaracs.PV = myCaracs.PV_MAX;

        string[] actions = descr[1].Split(new string[] { "%" }, System.StringSplitOptions.None);
        for (int i = 0; i < actions.Length; i += 3)
        {
            myCaracs.actions.Add(actions[i], new List<int>() { System.Int32.Parse(actions[i + 1]), System.Int32.Parse(actions[i + 2]) });
            myCaracs.actionsDelay.Add(actions[i], System.Int32.Parse(actions[i + 2]));
        }

        myCaracs.zoneAtt = descr[2];
        myCaracs._RANGE = System.Int32.Parse(descr[3]);

        if (descr[4] != "")
            myCaracs.conditions.Add(descr[4]);
        if (descr[5] != "")
            myCaracs.effects.Add(descr[5]);

        if (descr[6] == "D") // initialement une diagonale ?
            myOrientation = new Coord(1, 1);
        else if (descr[6] == "R") // c'est moche
            myOrientation = new Coord(0, 1);

        myCaracs.restart();
    }

    public virtual void DisplayInfos(bool yes)
    {
        GameObject infosPanel = GameObject.Find("Description");

        string result = "";
        foreach (string str in myCaracs.actions.Keys)
        {
            if (str == "Attack")
                result += "\nNB ATT : " + myCaracs.NB_ATT + "     DAMAGE : " + myCaracs.ATT;
            else
                result += "\n" + str + " : " + myCaracs.actions[str][0] + " every " + (myCaracs.actions[str][1] == 1 ? "" : myCaracs.actions[str][1]+"") + " turn";
        }
        infosPanel.transform.Find("Text").GetComponent<Text>().text = yes ? myName + "\n" + result : "No selection";
        infosPanel.transform.Find("Cost").GetComponentInChildren<Text>().text = "#";

        GameObject.Find("Background Menu").GetComponent<Deck>().selection = null;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
