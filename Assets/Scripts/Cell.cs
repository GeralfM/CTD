using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {

    public bool initiallyPlayer;

    public int PVMax;
    public int PV;

	// Use this for initialization
	void Start () {
		
	}

    public void Initialize(bool toPlayer)
    {
        initiallyPlayer = toPlayer;
        if (!toPlayer)
            gameObject.GetComponent<Image>().color = new Color(10f / 255, 47f / 255, 88f / 255, 1);
    }
    public void isPlayedOn()
    {
        GameObject.Find("Background Menu").GetComponent<Deck>().Play(this);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
