using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    public Deck myDeck { get; set; }

    public string myName { get; set; }
    public List<string> myDescription = new List<string>();
    public Dictionary<string, bool> properties = new Dictionary<string, bool>();
    
    public bool selected = false;

    // Use this for initialization
    void Start () {
        properties.Add("selectable", true);
        properties.Add("cyclable", true);
    }

    public void Select()
    {
        if (properties["selectable"])
        {
            selected = !selected;
            myDeck.Selecting(gameObject);
            gameObject.GetComponent<Image>().color = new Color(1f,1f,(selected?150f:200f)/255,1f);
        }
    }
    public void PlayOnCible(Cell cible)
    {
        GameObject newZyx = Instantiate(Resources.Load<GameObject>("Enemies prefabs/Basic Building"), cible.gameObject.transform);
        newZyx.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        newZyx.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        newZyx.GetComponent<RectTransform>().localScale = Vector3.one;
        newZyx.GetComponent<Zyx_Object>().Initialize(myName, myDescription[1], true, GameObject.Find("Background Game").GetComponent<Grid>(), cible.myCoords, new Coord());
        selected = !selected;
    }
    public void Cycle()
    {
        if(properties["cyclable"])
            myDeck.CycleCard(gameObject);
    }
    public void SetVisualInfos()
    {
        gameObject.GetComponentInChildren<Text>().text = myName + "\n" + myDescription[0];
    }

}
