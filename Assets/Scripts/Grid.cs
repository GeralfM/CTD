using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public Dictionary<Coord, Cell> playerCells = new Dictionary<Coord, Cell>();
    public Dictionary<Coord, Cell> ennemyCells = new Dictionary<Coord, Cell>();
    public GameObject cellPrefab;

	// Use this for initialization
	void Start () {

        // Creation du quadrillage principal
        for (int i = 0; i < 5; i++)
            for (int j = 0; j < 5; j++)
            {
                GameObject newCell = Instantiate(cellPrefab, gameObject.transform);

                newCell.GetComponent<RectTransform>().anchorMin = new Vector2(1.5f / 8 + 1 / 8f * i, 1.5f / 8 + 1 / 8f * j);
                newCell.GetComponent<RectTransform>().anchorMax = new Vector2(2.5f / 8 + 1 / 8f * i, 2.5f / 8 + 1 / 8f * j);
                newCell.GetComponent<RectTransform>().localScale = Vector3.one;

                newCell.GetComponent<Cell>().Initialize(true);
            }
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 5; j++)
            {
                GameObject newCell = Instantiate(cellPrefab, gameObject.transform);

                newCell.GetComponent<RectTransform>().anchorMin = new Vector2(0.25f / 8 + 6.5f / 8 * i, 1.5f / 8 + 1 / 8f * j);
                newCell.GetComponent<RectTransform>().anchorMax = new Vector2(1.25f / 8 + 6.5f / 8 * i, 2.5f / 8 + 1 / 8f * j);
                newCell.GetComponent<RectTransform>().localScale = Vector3.one;

                newCell.GetComponent<Cell>().Initialize(false);

                GameObject newCell2 = Instantiate(cellPrefab, gameObject.transform);

                newCell2.GetComponent<RectTransform>().anchorMin = new Vector2(1.5f / 8 + 1 / 8f * j, 0.25f / 8 + 6.5f / 8 * i);
                newCell2.GetComponent<RectTransform>().anchorMax = new Vector2(2.5f / 8 + 1 / 8f * j, 1.25f / 8 + 6.5f / 8 * i);
                newCell2.GetComponent<RectTransform>().localScale = Vector3.one;

                newCell2.GetComponent<Cell>().Initialize(false);
            }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
