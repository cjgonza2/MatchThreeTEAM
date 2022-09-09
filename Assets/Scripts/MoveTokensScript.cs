using UnityEngine;
using System.Collections;

public class MoveTokensScript : MonoBehaviour {

	protected GameManagerScript gameManager; //reference to gameManager script
	protected MatchManagerScript matchManager; //reference to MatchManager.

	public bool move = false; //creates bool and sets to false. 

	public float lerpPercent; //creates a float value for lerpPercentage, it's shows how close the script is to finishing the lerp
	public float lerpSpeed; //creates a float value for for how fast lerp is. 

	bool userSwap; //creates a bool...

	protected GameObject exchangeToken1; //creates gameObject reference for exchange token. 
	GameObject exchangeToken2;	//creates another gameObject reference for excahange token. 

	//creates two vector2s based on exchangetoken gameObjects. 
	Vector2 exchangeGridPos1;
	Vector2 exchangeGridPos2;

	public virtual void Start () {
		//assigns gameManager and MatchManager scripts 
		gameManager = GetComponent<GameManagerScript>();
		matchManager = GetComponent<MatchManagerScript>();
		lerpPercent = 0; //sets lerp Percentage to 0. 
	}

	public virtual void Update () {

		if(move){ //if move is true,
			lerpPercent += lerpSpeed; //Adds lerpSpeed to lerpPercent. 

			if(lerpPercent >= 1){ //if lerpPercent is greater or equal to one, clamps value to 1.
				lerpPercent = 1;
			}

			if(exchangeToken1 != null){ //if exchange token object is not null, calls exchangeTokens function.
				ExchangeTokens();
			}
		}
	}

	public void SetupTokenMove(){ //this functions resets the movemvent setup and opens the update check for the movement
		move = true;
		lerpPercent = 0;
	}

	public void SetupTokenExchange(GameObject token1, Vector2 pos1,
	                               GameObject token2, Vector2 pos2, bool reversable){
		//given two objects and their grid array positions and whether or not they're reversable:
		
		SetupTokenMove(); //this resets the movement setup

		exchangeToken1 = token1; //we're filling up the local variables with the given tokens
		exchangeToken2 = token2;

		exchangeGridPos1 = pos1; //and their positions
		exchangeGridPos2 = pos2;


		this.userSwap = reversable; //and save if we need to swap this tokens further
	}

	public virtual void ExchangeTokens(){ //moves the tokens 

		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos1.x, (int)exchangeGridPos1.y); //gets a grid position of the first exchange token
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos2.x, (int)exchangeGridPos2.y); //and for the second one

//		Vector3 movePos1 = Vector3.Lerp(startPos, endPos, lerpPercent);
//		Vector3 movePos2 = Vector3.Lerp(endPos, startPos, lerpPercent);

		Vector3 movePos1 = SmoothLerp(startPos, endPos, lerpPercent); //saves the new position for the exchange token by the lerp percentage
		Vector3 movePos2 = SmoothLerp(endPos, startPos, lerpPercent); //same for the second token

		exchangeToken1.transform.position = movePos1; //actually changes the position of tokens
		exchangeToken2.transform.position = movePos2;

		if(lerpPercent == 1){ //of the movement is done
			gameManager.gridArray[(int)exchangeGridPos2.x, (int)exchangeGridPos2.y] = exchangeToken1; //it swaps the two tokens' grid positions with each other
			gameManager.gridArray[(int)exchangeGridPos1.x, (int)exchangeGridPos1.y] = exchangeToken2;

			if(!matchManager.GridHasMatch() && userSwap){ //if the grid doesn't have a match and the tokes are reversable
				SetupTokenExchange(exchangeToken1, exchangeGridPos2, exchangeToken2, exchangeGridPos1, false); 
				//we're setting their exchange again to their original places,
				//but noe are setting the reversable at false, as they don't need to be swapped again
			} else { //but if the gris has a match
				exchangeToken1 = null; //we empty the exchange tokens
				exchangeToken2 = null;
				move = false; //and end the movement check
			}
		}
	}

	private Vector3 SmoothLerp(Vector3 startPos, Vector3 endPos, float lerpPercent){ 
		//it creates a new vector3 for the smooth movement based on the given positions, lerp percent, and the smoothstep method 
		return new Vector3(
			Mathf.SmoothStep(startPos.x, endPos.x, lerpPercent), //smoothstep helps create a stepout effect to the movement
			Mathf.SmoothStep(startPos.y, endPos.y, lerpPercent),
			Mathf.SmoothStep(startPos.z, endPos.z, lerpPercent));
	}

	public virtual void MoveTokenToEmptyPos(int startGridX, int startGridY, //given a start coordinate and an end coordinate and a given token object:
	                                int endGridX, int endGridY,
	                                GameObject token){
	
		//creates two vector 3s in relation to grid position, one set to the start coordinates and one set to the end coordinates. 
		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition(startGridX, startGridY);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition(endGridX, endGridY);

		//creates new vector 3 that smoothly transitions token into position. 
		Vector3 pos = Vector3.Lerp(startPos, endPos, lerpPercent);

		//moves the tokens. 
		token.transform.position =	pos;

		//if lerp is complete:
		if(lerpPercent == 1){
			//saving newly generated token into grid array position. 
			gameManager.gridArray[endGridX, endGridY] = token;
			//empties start coordinates. 
			gameManager.gridArray[startGridX, startGridY] = null;
		}
	}

	public virtual bool MoveTokensToFillEmptySpaces(){
		bool movedToken = false; //creates a bool to signify if a token has been moved

		for(int x = 0; x < gameManager.gridWidth; x++){ //for each x in the grid width, 
			for(int y = 1; y < gameManager.gridHeight ; y++){ //and each y in the grid height, 
				if(gameManager.gridArray[x, y - 1] == null){ //if the grid above given position is empty
					for(int pos = y; pos < gameManager.gridHeight; pos++){ //for each y in grid height, 
						GameObject token = gameManager.gridArray[x, pos]; //adds a token object to y position until gird is filled again. 
						if(token != null){ //if token is not null, 
							MoveTokenToEmptyPos(x, pos, x, pos - 1, token); //given the local x and pos values, and the token, calls the function. **************
							movedToken = true; //indicates token has been moved. 
						}
					}
				}
			}
		}
		//if lerp is complete
		if(lerpPercent == 1){
			move = false; //stop moving. 
		}

		return movedToken; //return if the token has moved. 
	}
}
