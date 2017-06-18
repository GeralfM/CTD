using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caracteristics {

    public int PV_MAX { get; set; }
    public int _RANGE { get; set; }

    public int PV { get; set; }
    public int ATT { get; set; }
    public int NB_ATT { get; set; }
    public int RANGE { get; set; }
    public string zoneAtt { get; set; }

    public Dictionary<string, List<int>> actions = new Dictionary<string, List<int>>();
    public Dictionary<string, int> actionsDelay = new Dictionary<string, int>();
    public List<string> tempActions = new List<string>(); // opposite, etc...

    public Dictionary<string, int> effectsOverTime = new Dictionary<string, int>();

    public List<string> conditions = new List<string>();
    public List<string> effects = new List<string>();

    // Use this for initialization
    void Start () {
		
	}

    public void restart()
    {
        if (new List<string>(actions.Keys).Contains("Attack"))
        {
            ATT = actions["Attack"][0];
            NB_ATT = actions["Attack"][1];
        }
        RANGE = _RANGE;

        tempActions = new List<string>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
