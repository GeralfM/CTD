using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zyx_building : Zyx_Object {

	// Use this for initialization
	void Start () {
		
	}
    
    public override IEnumerator Attack()
    {
        StartCoroutine(base.Attack());
        int att_left = myCaracs.NB_ATT;

        List<Zyx_Object> ennemies = getAllEnnemies(getZone(myCaracs.zoneAtt, myCaracs.RANGE));
        int choice;
        while (ennemies.Count>0 && att_left > 0)
        {
            choice = Random.Range(0, ennemies.Count);
            ShootToTarget(ennemies[choice].gameObject);
            yield return new WaitForSeconds(0.8f / GameObject.Find("MainHandler").GetComponent<GameHandler>().maxShots);
            ennemies[choice].AddToPV(-myCaracs.ATT);
            att_left--;
            yield return new WaitForSeconds(0.01f);
            ennemies = getAllEnnemies(getZone(myCaracs.zoneAtt, myCaracs.RANGE));
        }
        
    }
    public List<Zyx_Object> getAllEnnemies(List<Cell> zone)
    {
        List<Zyx_Object> ennemies = new List<Zyx_Object>();

        foreach (Cell aCell in getZone(myCaracs.zoneAtt, myCaracs.RANGE))
            if (!aCell.available && !aCell.occupant.friendly) ennemies.Add(aCell.occupant);
        
        return ennemies;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
