using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {

    public bool initiallyPlayer;
    public bool fallen;

    public Coord myCoords { get; set; }

    public Zyx_Object occupant { get; set; }

    public bool available = true;
    public int PVMax;
    public int PV;

	// Use this for initialization
	void Start () {
        PVMax = 10;
        PV = PVMax;
	}

    public void addToPV(int val)
    {
        PV = Mathf.Min(PVMax, PV + val); // dépasse PVMax pour le moment;
        gameObject.GetComponent<Image>().color = new Color(1f, (float)PV / (float)PVMax, PV / PVMax, 1);
        if (PV < 0)
            setFallen(true);
    }
    public void setFallen(bool isFallen)
    {
        fallen = isFallen;
        if (fallen && !available && occupant.friendly)
            occupant.IsDestroyed();
        gameObject.GetComponent<Image>().color = new Color(10f / 255, 47f / 255, 88f / 255, 1);
    }

    public void Initialize(bool toPlayer)
    {
        initiallyPlayer = toPlayer;
        fallen = !initiallyPlayer;

        if (!toPlayer)
            gameObject.GetComponent<Image>().color = new Color(10f / 255, 47f / 255, 88f / 255, 1);
    }
    public void isPlayedOn()
    {
        if (!fallen)
            GameObject.Find("Background Menu").GetComponent<Deck>().Play(this);
    }
    public void isHovered(bool isOn)
    {
        if (!fallen)
            GameObject.Find("MainHandler").GetComponent<CursorHandler>().showMyInfos(this, isOn);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
