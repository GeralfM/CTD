using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cond_Eff_Handler : MonoBehaviour {

    public Dictionary<int, Coord> keyToCoord = new Dictionary<int, Coord>()
    { { 0, new Coord(0, 1) }, { 1, new Coord(-1, 1) }, { 2, new Coord(-1, 0) }, { 3, new Coord(-1, -1) }, { 4, new Coord(0, -1) }, { 5, new Coord(1, -1) }, { 6, new Coord(1, 0) }, { 7, new Coord(1, 1) } };
    public Dictionary<Coord, int> coordToKey = new Dictionary<Coord, int>()
    { { new Coord(0, 0),7 }, { new Coord(0, 1),0 }, {new Coord(-1, 1),1 }, { new Coord(-1, 0),2 }, { new Coord(-1, -1),3 }, { new Coord(0, -1),4 }, { new Coord(1, -1),5 }, { new Coord(1, 0),6 }, { new Coord(1, 1),7 } };

    public int cumulative;

    // Use this for initialization
    void Start () {
		
	}
	
    public void TestCE(Zyx_Object subject)
    {
        if (subject.myCaracs.conditions.Count > 0)
        {
            
            string[] descrCond = subject.myCaracs.conditions[0].Split(new string[] { "%" }, System.StringSplitOptions.None);
            string[] descrEff = subject.myCaracs.effects[0].Split(new string[] { "%" }, System.StringSplitOptions.None);

            for (int j = 0; j < descrCond.Length; j += 2)
            {
                cumulative = 0;

                bool success = TestCond(subject, descrCond[j], System.Int32.Parse(descrCond[j + 1]));
                
                if (!descrCond[j].Contains("Cum"))
                    cumulative = 1;

                if (success)
                    for (int k = 1; k <= cumulative; k++)
                        DoEffect(subject, descrEff[j], System.Int32.Parse(descrEff[j + 1]));
            }
        }
    }
    public bool TestCond(Zyx_Object obj, string test, int value)
    {
        bool result = false;
        int count = 0;
        switch (test)
        {
            case ("True"):
                result = true;
                break;
            case ("Support"):
                foreach (Cell neighb in obj.getZone("circle", 1))
                    if (!neighb.available && neighb.occupant.friendly) // Attation : en vrai c'est un Wielder qu'il faut !
                        count++;
                result = result || count >= value;
                break;
            case ("SupportCum"):
                int valueCum = value;
                while (TestCond(obj, "Support", valueCum))
                {
                    valueCum += value;
                    cumulative++;
                }
                result = cumulative > 0;
                break;
            default:
                break;
        }
        return result;
    }
    public void DoEffect(Zyx_Object obj, string effect, int value)
    {
        switch (effect)
        {
            case ("Nbatt"):
                obj.myCaracs.NB_ATT++;
                break;
            case ("Rotate"):
                int go = (value == 8 ? 1 : (value == 4 ? 2 : 0));
                obj.myOrientation = keyToCoord[(go + coordToKey[obj.myOrientation]) % 8];
                obj.gameObject.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 360 / value));
                break;
            case ("Opposite"):
                obj.myCaracs.tempActions.Add("Opposite");
                break;
            case ("Att"):
                obj.myCaracs.ATT += value;//print(value);
                break;
            default:
                break;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
