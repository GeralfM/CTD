using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caracteristics {

    public int _ATT { get; set; }
    public int _NB_ATT { get; set; }
    public int _RANGE { get; set; }

    public int PV { get; set; }
    public int ATT { get; set; }
    public int NB_ATT { get; set; }
    public int RANGE { get; set; }
    public string zoneAtt { get; set; }

    public List<string> conditions = new List<string>();
    public List<string> effects = new List<string>();

    // Use this for initialization
    void Start () {
		
	}

    public void restart()
    {
        ATT = _ATT;
        NB_ATT = _NB_ATT;
        RANGE = _RANGE;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
