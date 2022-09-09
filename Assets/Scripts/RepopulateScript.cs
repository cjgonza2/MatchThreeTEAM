using UnityEngine;
using System.Collections;

public class RepopulateScript : MonoBehaviour {
	
	protected GameManagerScript gameManager; //gamemanager reference

	public virtual void Start () {
		gameManager = GetComponent<GameManagerScript>(); //saves the reference to it
	}

	public virtual void AddNewTokensToRepopulateGrid(){ //adds new tokens to the grid positions where it's needed
		for(int x = 0; x < gameManager.gridWidth; x++){ //for each x position in the grid width
			GameObject token = gameManager.gridArray[x, gameManager.gridHeight - 1]; //we check the token saved in the position above the x
			if(token == null){ //if there's nothing there
				gameManager.AddTokenToPosInGrid(x, gameManager.gridHeight - 1, gameManager.grid); //we generate a token on the said above position
			}
		}
	}
}
