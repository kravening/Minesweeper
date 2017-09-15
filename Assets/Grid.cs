using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour { //Sets up the game board depending on properties given.
	int gridSizeX 			= 		9;
	int gridSizeY 			= 		9;
	int tileCounter 		= 		0;
	int maxMines 			= 		10;
	int currentMineCounter 	= 		0;
	int[] neighbourListNumbers = new int[8];

	List<GameObject> tileList = new List<GameObject> (); // the array of the board
	[SerializeField]GameObject tile;

	// Use this for initialization
	void Start () { //setup the board here
		int currentXRow = 0;
		int currentYRow = 0;
		GameObject tileFolder = new GameObject(); 	// make a new empty game object to put the tiles, to keep the hierarchy clean in unity engine.
		tileFolder.name = "tileFolder"; 			//change the name of the gameObject to something descriptive.

			for (int i = gridSizeX; i > 0; i--) { //instantiate all tiles and push them into an array
				currentXRow++;
				currentYRow = 0;

				for (int j = gridSizeY; j > 0; j--) {
					GameObject spawnedTile = Instantiate (tile,new Vector2(currentXRow,currentYRow),new Quaternion(90,0,0,0));
					spawnedTile.transform.parent = tileFolder.transform;
					currentYRow++;
					tileCounter++;
					tileList.Add (spawnedTile);
				}
			}
		while (maxMines >= currentMineCounter) {
			// go through the tile array randomly and check the mine flag on empty tiles.
			currentMineCounter++;
		}

		neighbourListNumbers [0] = -1;
		neighbourListNumbers [1] = 1;
		neighbourListNumbers [2] = -gridSizeY - 1;
		neighbourListNumbers [3] = -gridSizeY;
		neighbourListNumbers [4] = -gridSizeY + 1;
		neighbourListNumbers [5] = gridSizeY - 1;
		neighbourListNumbers [6] = gridSizeY;
		neighbourListNumbers [7] = gridSizeY + 1;
		//Debug.Log (tileList.Count);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (tileList.Count);
	}

	public void RevealNeighbouringTiles(GameObject origin){ //finds and returns possible neighbours, if checking for game object doesn work instead check for a max number and min number, for example if the grid size is 10x10, max number is 100, min number will always be 0.
		List<GameObject> foundNeighbours = new List<GameObject>(); // holds the neighbouring tiles
		for (int i = tileList.Count-1; i >= 0 ; i--) {
			

			if (tileList[i].gameObject.transform == origin.gameObject.transform) {
				for (int j = 0; j < 8 ; j++) {
					int number = i + neighbourListNumbers [j];
					if (number >= 0 && number <= gridSizeX * gridSizeY - 1) {
						foundNeighbours.Add (tileList [number].gameObject);
					}
				}
			}
		}
		Debug.Log (foundNeighbours.Count);
		if (foundNeighbours.Count > 0) {
			for (int i = foundNeighbours.Count ; i < 0; i++) {
				
				//foundNeighbours [i].gameObject.GetComponent<Tile> ().ToggleFlag();
			}
		}
		// go into each found neighbour and reveal all empty tiles and or calculate numbers off the tile, 
	}
}
