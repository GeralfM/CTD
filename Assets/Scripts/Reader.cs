using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reader : MonoBehaviour {

    public Dictionary<string, List<string>> myDescr = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> myDescrUnits = new Dictionary<string, List<string>>();

    // Use this for initialization
    void Awake()
    {

        if (!(Application.platform == RuntimePlatform.WindowsPlayer))
        {
            string[] descr = Resources.Load<TextAsset>("Files/Descriptions_buildings").text.Split
                (new string[] { "===" + System.Environment.NewLine, "@" }, System.StringSplitOptions.None);
            for (int k = 0; k < descr.Length; k += 3)
                myDescr.Add(descr[k].Replace(System.Environment.NewLine, ""), new List<string>() { descr[k + 1], descr[k + 2] });
        }
        else
        {
            string[] descr = Resources.Load<TextAsset>("Files/Descriptions_buildings").text.Split
                (new string[] { "===", "@" }, System.StringSplitOptions.None);
            for (int k = 0; k < descr.Length; k += 3)
                myDescr.Add(descr[k].Trim(), new List<string>() { descr[k + 1], descr[k + 2] });
        }

        if (!(Application.platform == RuntimePlatform.WindowsPlayer))
        {
            string[] descr = Resources.Load<TextAsset>("Files/Descriptions_units").text.Split
                (new string[] { "===" + System.Environment.NewLine, "@" }, System.StringSplitOptions.None);
            for (int k = 0; k < descr.Length; k += 3)
                myDescrUnits.Add(descr[k].Replace(System.Environment.NewLine, ""), new List<string>() { descr[k + 1], descr[k + 2] });
        }
        else
        {
            string[] descr = Resources.Load<TextAsset>("Files/Descriptions_units").text.Split
                (new string[] { "===", "@" }, System.StringSplitOptions.None);
            for (int k = 0; k < descr.Length; k += 3)
                myDescrUnits.Add(descr[k].Trim(), new List<string>() { descr[k + 1], descr[k + 2] });
        }
    }

        // Update is called once per frame
        void Update () {
		
	}
}
