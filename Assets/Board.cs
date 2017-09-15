using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	static int GRIDX = 30;
	static int GRIDY = 30;
	static int PROPERTIES = 6;// isClicked, isMine, isFlagged, isVisited, neighbouringMines, wasCheckedThisRound.
	int bombs = 80;
	int previousTileX, previousTileY;

	int[,,] gridArray = new int[GRIDX, GRIDY, PROPERTIES];
	//positionx, positiony, tile state.
	GameObject[] tileArray = new GameObject[GRIDX * GRIDY];
	[SerializeField]GameObject tile;

	[SerializeField]List<Sprite> imageList = new List<Sprite> ();

	// Use this for initialization
	void Start ()
	{
		SetBoard ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		FloodFill ();
	}

	void SetBoard ()
	{
		int tileInArray = 0;
		for (int i = 0; i < GRIDY; i++) {
			for (int j = 0; j < GRIDX; j++) {
				for (int k = 0; k < PROPERTIES; k++) {
					gridArray [i, j, k] = 0; //resets all tiles
				}
			}
		}
		for (int i = 0; i < bombs; i++) {
			int randomX = Random.Range (0, GRIDX - 1);
			int randomY = Random.Range (0, GRIDY - 1);
			if (gridArray [randomX, randomY, 2] != 1) {
				gridArray [randomX, randomY, 2] = 1; //sets mine property to "true"
				for (int j = -1; j <= 1; j++) {
					for (int k = -1; k <= 1; k++) {
						if (CheckBounds (randomX + j, randomY + k)) {
							if (gridArray [randomX + j, randomY + k, 2] != 1) {
								gridArray [randomX + j, randomY + k, 4] += 1; // increase mine counter
							}
						}
					}
				}
			} else {
				i--; //there already was a mine here, try again!
			}
		}
			
		for (int i = 0; i < GRIDY; i++) {
			for (int j = 0; j < GRIDX; j++) {
				//create board here

				GameObject spawnedTile = Instantiate (tile, new Vector2 (i, j), new Quaternion (90, 0, 0, 0));
				spawnedTile.GetComponent<Tile> ().TileX = j;
				spawnedTile.GetComponent<Tile> ().TileY = i;
				tileArray [processCoördinateToNumber (j, i)] = spawnedTile.gameObject;
				tileInArray++;
			}
		}
	}

	int processCoördinateToNumber (int x, int y)
	{ //link coOrdinates to a number, so i can access the proper tile in the tileArray.
		int processedNumber = 0;
		int curYRow = 0;
		int curXRow = 0;
		for (int i = GRIDY; i > 0; i--) {
			curXRow = 0;
			for (int j = GRIDX; j > 0; j--) {
				processedNumber++;
				if (curXRow == x && curYRow == y) {
					return processedNumber - 1;
				}
				curXRow++;
			}
			curYRow++;
		}
		return processedNumber - 1;
	}

	void setupTile ()
	{

		tile.GetComponent<SpriteRenderer> ().sprite = imageList [0];
	}

	bool CheckBounds (int x, int y)
	{
		if (x >= 0 && y >= 0 && x <= GRIDX - 1 && y <= GRIDY - 1) {
			return true;
		} else {
			return false;
		}
	}

	public void CheckTile (int x, int y)
	{
		if (CheckBounds (x, y)) { // check if incoming tile is not outside of the bounds
			if (gridArray [x, y, 0] == 0 && gridArray [x, y, 1] == 0) {
				if (gridArray [x, y, 2] == 1) {
				
					for (int i = 0; i < GRIDY; i++) {
						for (int j = 0; j < GRIDX; j++) {
							if (gridArray [i, j, 2] == 1) {
								tileArray [processCoördinateToNumber (i, j)].GetComponent<SpriteRenderer> ().sprite = imageList [3]; // uncover mines
								//GameOver
							}
						}
					}
				} else if (gridArray [x, y, 2] != 1) {
					if (gridArray [x, y, 4] >= 1) { // found numbered tile
						tileArray [processCoördinateToNumber (x, y)].GetComponent<SpriteRenderer> ().sprite = imageList [gridArray [x, y, 4] + 3];
						gridArray [x, y, 3] = 1; //set isVisited flag to true
						gridArray [x, y, 0] = 1; //set isClicked flag to true

					} else { //found empty tile
						tileArray [processCoördinateToNumber (x, y)].GetComponent<SpriteRenderer> ().sprite = imageList [2]; // empty tile

						gridArray [x, y, 3] = 1; //set isVisited flag to true
						gridArray [x, y, 0] = 1; //set isClicked flag to true
					}
				}
			}
		}
	}

	void FloodFill ()
	{
		for (int i = 0; i < GRIDX; i++) {
			for (int j = 0; j < GRIDY; j++) {
				if (gridArray [j, i, 0] == 1 && gridArray [j, i, 5] <= 7 && gridArray [j, i, 4] == 0 && gridArray [j, i, 1] == 0) {
					if (CheckBounds (j + 1, i)) {
						if (gridArray [j + 1, i, 2] != 1 && gridArray [j, i, 5] == 0) {
							CheckTile (j + 1, i);
						}
					}
					if (CheckBounds (j, i + 1)) {
						if (gridArray [j, i + 1, 2] != 1 && gridArray [j, i, 5] == 1) {
							CheckTile (j, i + 1);
						}
					}
					if (CheckBounds (j + 1, i + 1)) {
						if (gridArray [j + 1, i + 1, 2] != 1 && gridArray [j, i, 5] == 2) {
							CheckTile (j + 1, i + 1);
						}
					}
					if (CheckBounds (j - 1, i)) {
						if (gridArray [j - 1, i, 2] != 1 && gridArray [j, i, 5] == 3) {
							CheckTile (j - 1, i);
						}
					}
					if (CheckBounds (j, i - 1)) {
						if (gridArray [j, i - 1, 2] != 1 && gridArray [j, i, 5] == 4) {
							CheckTile (j, i - 1);
						}
					}
					if (CheckBounds (j - 1, i - 1)) {
						if (gridArray [j - 1, i - 1, 2] != 1 && gridArray [j, i, 5] == 5) {
							CheckTile (j - 1, i - 1);
						}
					}
					if (CheckBounds (j + 1, i - 1)) {
						if (gridArray [j + 1, i - 1, 2] != 1 && gridArray [j, i, 5] == 6) {
							CheckTile (j + 1, i - 1);
						}
					}
					if (CheckBounds (j - 1, i + 1)) {
						if (gridArray [j - 1, i + 1, 2] != 1 && gridArray [j, i, 5] == 7) {
							CheckTile (j - 1, i + 1);
						}
					}
					gridArray [j, i, 5] += 1;

					Debug.Log ("NEXT!");
				}
			}
		}
	}

	public void CheckFlag (int x, int y)
	{
		if (gridArray [x, y, 0] == 0) {
			if (gridArray [x, y, 1] == 0) {
				gridArray [x, y, 1] = 1;		
				tileArray [processCoördinateToNumber (x, y)].GetComponent<SpriteRenderer> ().sprite = imageList [1];
			} else {
				gridArray [x, y, 1] = 0;
				tileArray [processCoördinateToNumber (x, y)].GetComponent<SpriteRenderer> ().sprite = imageList [0];
			}
		}
	}
}