using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

    public List<GameObject> myDeck = new List<GameObject>();
    public List<GameObject> myHand = new List<GameObject>();
    public Reader myReader { get; set; }
    public GameObject cardPrefab;

    public GameObject selection { get; set; }

    // Use this for initialization
    void Start() {

        myReader = GameObject.Find("MainHandler").GetComponent<Reader>();

        SetDeck();
        Shuffle();
        Draw();

    }

    public void Selecting(GameObject _selection)
    {
        if (_selection.GetComponent<Card>().selected)
            selection = _selection;
        else
            selection = null;
        foreach (GameObject card in myHand) if (card != _selection)
                card.GetComponent<Card>().properties["selectable"] = !_selection.GetComponent<Card>().selected;
    }
    public void SettingProperties(string property, bool state)
    {
        foreach (GameObject card in myHand)
        {
            card.GetComponent<Card>().properties[property] = state;
            if (property=="selectable") card.GetComponent<Card>().selected = !state;
        }
    }

    public void Play(Cell cible) {
        if (selection != null) selection.GetComponent<Card>().PlayOnCible(cible);
        myHand.Remove(selection);
        Destroy(selection);

        SettingProperties("selectable",false);
    }

    public void SetDeck() {
        for (int i = 1; i <= 15; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, gameObject.transform);
            myDeck.Add(newCard);
            newCard.GetComponent<Card>().myDeck = this;
            newCard.GetComponent<Card>().myName = "Spark watchtower";
            newCard.GetComponent<Card>().myDescription = myReader.myDescr[newCard.GetComponent<Card>().myName];
            newCard.GetComponent<Card>().SetVisualInfos();

            newCard.SetActive(false);
        }
    }
    public void Draw()
    {
        for(int i=0;i<myHand.Count; i++)
        {
            myHand[i].GetComponent<RectTransform>().anchorMin = new Vector2(0.1f, 0.05f + 0.25f * i);
            myHand[i].GetComponent<RectTransform>().anchorMax = new Vector2(0.9f, 0.25f + 0.25f * i);
        }
        while (myHand.Count < 3 && myDeck.Count>0)
        {
            GameObject drewCard = myDeck[0];
            drewCard.SetActive(true);
            myDeck.RemoveAt(0);

            drewCard.GetComponent<RectTransform>().anchorMin = new Vector2(0.1f, 0.05f + 0.25f * myHand.Count);
            drewCard.GetComponent<RectTransform>().anchorMax = new Vector2(0.9f, 0.25f + 0.25f * myHand.Count);
            drewCard.GetComponent<RectTransform>().localScale = Vector3.one;

            myHand.Add(drewCard);
        }
    }
    public void CycleCard(GameObject cardToCycle)
    {
        myDeck.Add(cardToCycle);
        myHand.Remove(cardToCycle);
        cardToCycle.SetActive(false);
        SettingProperties("cyclable", false);
    }
    public void Shuffle()
    {
        List<GameObject> shuffled = new List<GameObject>();

        while (myDeck.Count > 0) {
            int position = Random.Range(0, myDeck.Count);
            shuffled.Add(myDeck[position]);
            myDeck.RemoveAt(position);
        }

        myDeck = shuffled;
    }
    public void NextTurn()
    {
        SettingProperties("selectable",true);
        SettingProperties("cyclable", true);
        Draw();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
