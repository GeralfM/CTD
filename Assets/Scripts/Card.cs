using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public Deck myDeck { get; set; }

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

            print("Selected ? " + selected);
        }
    }
    public void PlayOnCible(Cell cible)
    {
        //DoTheThing !
        selected = !selected;
    }
    public void Cycle()
    {
        if(properties["cyclable"])
            myDeck.CycleCard(gameObject);
    }

}
