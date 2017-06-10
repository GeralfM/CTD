using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public Dictionary<Coord, Cell> allCells;
    public GameObject cellPrefab;

	// Use this for initialization
	void Start () {
        
    }

    public void Initialize()
    {
        allCells = new Dictionary<Coord, Cell>();
        
        // Creation du quadrillage principal
        for (int i = 0; i <= 6; i++)
            for (int j = 0; j <= 6; j++)
            {
                GameObject newCell = Instantiate(cellPrefab, gameObject.transform);

                float x = (1f / 4 + (i > 0 ? 1f / 4 : 0) + (i == 6 ? 1f / 4 : 0) + i) / 8;
                float y = (1f / 4 + (j > 0 ? 1f / 4 : 0) + (j == 6 ? 1f / 4 : 0) + j) / 8;

                newCell.GetComponent<RectTransform>().anchorMin = new Vector2(x, y);
                newCell.GetComponent<RectTransform>().anchorMax = new Vector2(x + 1f / 8, y + 1f / 8);
                newCell.GetComponent<RectTransform>().localScale = Vector3.one;

                allCells.Add(new Coord(i, j), newCell.GetComponent<Cell>());
            }

        foreach (Coord toRemove in new List<Coord>() { new Coord(0, 0), new Coord(0, 6), new Coord(6, 0), new Coord(6, 6) })
        {
            Cell byebye = allCells[toRemove];
            allCells.Remove(toRemove);
            Destroy(byebye.gameObject);
        }

        foreach (Coord aCoord in allCells.Keys)
            allCells[aCoord].Initialize(aCoord.x > 0 && aCoord.x < 6 && aCoord.y > 0 && aCoord.y < 6 ? true : false);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
