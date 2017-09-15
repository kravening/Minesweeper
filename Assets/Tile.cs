using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	int tileX,tileY,bombCounter;

	GameObject board;
	bool hasChecked = false;
	// Use this for initialization
	void Start () {
		board = GameObject.FindGameObjectWithTag ("GameController");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseOver(){
		if (Input.GetMouseButtonUp (0)) {
			board.GetComponent<Board> ().CheckTile (TileX, TileY);
		} else if (Input.GetMouseButtonUp (1)) {
			board.GetComponent<Board> ().CheckFlag (TileX, TileY);
		}
	}

	public int TileX
	{
		get{
			return tileX;
		}
		set{
			tileX = value;
		}
	}

	public int TileY
	{
		get{
			return tileY;
		}
		set{
			tileY = value;
		}
	}
}
