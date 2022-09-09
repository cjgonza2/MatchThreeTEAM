using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public int gridWidth = 8;  //creates value the grid width
	public int gridHeight = 8;	//creates value for grid height
	public float tokenSize = 1;		//creates value for how big token is. ******

	//these assign references to scripts attached the game Manager.
	protected MatchManagerScript matchManager; 
	protected InputManagerScript inputManager;
	protected RepopulateScript repopulateManager;
	protected MoveTokensScript moveTokenManager;

	public GameObject grid; //The grid gameobject where the game is held lol
	public  GameObject[,] gridArray; //array that holds grid coordinates
	protected Object[] tokenTypes;	//array that holds the token objects. 
	GameObject selected; //objected by the playe.r 

	public virtual void Start () {
		tokenTypes = (Object[])Resources.LoadAll("Tokens/"); //load "Token" object in resources into the object array. 
		gridArray = new GameObject[gridWidth, gridHeight]; //Sets dimensions of the grid Array. 
		MakeGrid();	//calls make grid function lol.

		//these assign the attached scripts to the relevant local variables. 
		matchManager = GetComponent<MatchManagerScript>();
		inputManager = GetComponent<InputManagerScript>();
		repopulateManager = GetComponent<RepopulateScript>();
		moveTokenManager = GetComponent<MoveTokensScript>();
	}

	public virtual void Update(){
		if(!GridHasEmpty()){ //checks if gridhasempty returned false.  
			if(matchManager.GridHasMatch()){	//checks if there is a match between tokens. 
				matchManager.RemoveMatches();	//removes the matches. 
			} else {	//others, selects the token based on player input. 
				inputManager.SelectToken();
			}
		} else { //if gridhasEmpty returned true
			if(!moveTokenManager.move){ //and if the tokens aren't moving already
				moveTokenManager.SetupTokenMove(); //we're setting them up to move
			}
			if(!moveTokenManager.MoveTokensToFillEmptySpaces()){ //if the methos returns false
				repopulateManager.AddNewTokensToRepopulateGrid(); //we repopulate the grid with new tokens
			}
		}
	}

	void MakeGrid() {
		grid = new GameObject("TokenGrid"); //it creates a new game object and names it
		for(int x = 0; x < gridWidth; x++){ //for each element in the grid width 
			for(int y = 0; y < gridHeight; y++){ //and height 
				AddTokenToPosInGrid(x, y, grid); //we populate the grid with objects based on it's grid coords
			}
		}
	}

	public virtual bool GridHasEmpty(){ //checks if grid is empty
		for(int x = 0; x < gridWidth; x++){ //for each object in the grid width array?
			for(int y = 0; y < gridHeight ; y++){ //for each object height of the gridarray. 
				if(gridArray[x, y] == null){ //if spot in grid is empty.
					return true; //if yes returns true. 
				}
			}
		}

		return false; //otherwise return false.
	}


	public Vector2 GetPositionOfTokenInGrid(GameObject token){ //vector 2 based on given Token GameObject. 
		for(int x = 0; x < gridWidth; x++){ //for each posiiton in grid width, 
			for(int y = 0; y < gridHeight ; y++){ //and each position in grid height,
				if(gridArray[x, y] == token){ //if Grid position matches given token,
					return(new Vector2(x, y)); //returns it's position. 
				}
			}
		}
		return new Vector2();//this has function has return something, so if all else will return empty vector 2 *WILL NEVER HAPPEN <:(*
	}
		
	public Vector2 GetWorldPositionFromGridPosition(int x, int y){ //creates a worldPosition equivalent to the grid position
		return new Vector2( //it returns this vector2
			(x - gridWidth/2) * tokenSize, //calculating the x's world position in relation to the grid position
			//the tokensize is used as the step of how far away the object needs to be placed from each other 
			(y - gridHeight/2) * tokenSize); //same for y
	}

	public void AddTokenToPosInGrid(int x, int y, GameObject parent){ //this creates the tokens to the given position
		Vector3 position = GetWorldPositionFromGridPosition(x, y); //creates a vector3 for each token to be places in the world based on its grid position
		GameObject token = //instantiates a game object
			Instantiate(tokenTypes[Random.Range(0, tokenTypes.Length)], //it selects a random token object from the array we've created before
			            position, //positions it in the world the previously created local vector3
			            Quaternion.identity) as GameObject; //and gives it no rotation and casts it as a gameobject
		token.transform.parent = parent.transform; //puts the instantiated token under the created grid gameobject
		gridArray[x, y] = token; //if fills up the correlating grid array with the created token
	}
}
