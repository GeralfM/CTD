using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cond_Eff_Handler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    public void TestCE(Zyx_Object subject) // Un seul cond-effet pour le moment
    {
        int taille = subject.myCaracs.conditions.Count;
        for(int i = 0; i < taille; i++)
        {
            string[] descrCond = subject.myCaracs.conditions[i].Split(new string[] { "%" }, System.StringSplitOptions.None);
            string[] descrEff = subject.myCaracs.effects[i].Split(new string[] { "%" }, System.StringSplitOptions.None);
            if (TestCond(subject, descrCond[0], System.Int32.Parse(descrCond[1]))) DoEffect(subject, descrEff[0], System.Int32.Parse(descrEff[1]));
        }
    }
    public bool TestCond(Zyx_Object obj, string test, int value)
    {
        bool result = false;
        int count = 0;
        switch (test)
        {
            case ("Support"):
                foreach (Cell neighb in obj.getZone("circle", 1))
                    if (!neighb.available && neighb.occupant.friendly) // Attation : en vrai c'est un Wielder qu'il faut !
                        count++;
                result = result || count >= value;
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
            default:
                break;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
