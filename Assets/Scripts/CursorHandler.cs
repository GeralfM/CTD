using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorHandler : MonoBehaviour {

    public string stateCursor { get; set; }

    public GameObject hoverPanel;

	// Use this for initialization
	void Start () {
        stateCursor = "none";
	}

    public void showMyInfos(Cell aCell, bool isOn)
    {
        hoverPanel.SetActive(isOn);
        hoverPanel.transform.position = aCell.transform.position+ new Vector3(-0.675f,0.7f,0);
        hoverPanel.GetComponentInChildren<Text>().text = aCell.PV + " / " + aCell.PVMax + " PV";
    }
	
	// Update is called once per frame
	void Update () {
        
    }

}
