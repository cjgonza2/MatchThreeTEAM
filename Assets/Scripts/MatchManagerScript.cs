using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MatchManagerScript : MonoBehaviour {

	protected GameManagerScript gameManager; //reference to gamemanager

	//public List<Vector2[]> DualMatchList = new List<Vector2[]>();
	//private List<Vector2> dualMatchList = new List<Vector2>();
	//private new Vector2[] xTokens;
	//private new Vector2[] yTokens;
	
	public List<Vector2Int> dualMatchList = new List<Vector2Int>();
 
	public virtual void Start () { 
		gameManager = GetComponent<GameManagerScript>(); //sets gamemanager script reference. 
	}

	
	//Known Bugs:
	//1. NO Vertical Matches
	//2. move objects Diagonal
	//3. two matches sharing a token. 


	//reminder for virtual:
	//similar to how public is modify access,
	//virtual is saying that we can override it. 
	public virtual bool GridHasMatch(){ //checks if there is a match. 
		bool match = false;  //declaring match bool and setting it false. 
		
		for(int x = 0; x < gameManager.gridWidth; x++){ //for each position in grid width, and for each position in grid height,
			for(int y = 0; y < gameManager.gridHeight ; y++){ //
				if(x < gameManager.gridWidth - 2){ //Checks if this x is one of the last elements in the row, since the
												   //match check needs to be performed through tthe first element of the grid row (left ot right)
					match = match || GridHasHorizontalMatch(x, y); //checks if match is true or false based on hasMATCH function. 
				}

				//-----------------------------------
				if (y < gameManager.gridWidth - 2)
				{ //Checks if this x is one of the last elements in the row, since the
				  //match check needs to be performed through tthe first element of the grid row (left ot right)
					match = match || GridHasVerticalMatch(x, y); //checks if match is true or false based on hasMATCH function. 
				}
				
				//----------------------------------------
			}
		}

		return match; //returns the bool value. 
	}

	public bool GridHasHorizontalMatch(int x, int y){ //checks if theres a horizontal match based on given grid position of objects. 
		//generating 3 gameobjects to assign checked objects from left to right.  
		GameObject token1 = gameManager.gridArray[x + 0, y];	
		GameObject token2 = gameManager.gridArray[x + 1, y];
		GameObject token3 = gameManager.gridArray[x + 2, y];
		
		
		//GetHorizontalVectors(token1, token2, token3);
		

		//new Vector2 = GetComponent<Vector2>(token1);
		//dualMatchList.Add(token1);
		
		
		if(token1 != null && token2 != null && token3 != null){ //if none of the tokens are null
			//this is just saves the objects spriteRender component. 
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();

			//checks if token sprites are the same, returns true if so.
			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);


		} else { //otherwise returns false. 
			return false;
		}
	}

	//-------------------------
	public bool GridHasVerticalMatch(int x, int y)
	{ //checks if theres a horizontal match based on given grid position of objects. 
	  //generating 3 gameobjects to assign checked objects from left to right.  
		GameObject token1 = gameManager.gridArray[x, y+0];
		GameObject token2 = gameManager.gridArray[x, y+1];
		GameObject token3 = gameManager.gridArray[x, y+2];

		//GetVerticalVectors(token1, token2, token3);

		if (token1 != null && token2 != null && token3 != null)
		{ //if none of the tokens are null
		  //this is just saves the objects spriteRender component. 
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();

			//checks if token sprites are the same, returns true if so.
			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
		}
		else
		{ //otherwise returns false. 
			return false;
		}
	}
	public bool SharedTokenCheck()
	{
		/*if (DualMatchList.horizontalMatch[0] != null)
		{
			return true;
		}
		else
		{
		}*/
		return this;
	}

	//-------------------------

	public int GetHorizontalMatchLength(int x, int y){ 
		int matchLength = 1; //declaring matchlength integer. 
		
		GameObject first = gameManager.gridArray[x, y]; //declars the first token in the match as a GameObject. 

		if(first != null){ //if first in match is not null:
			SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>(); //saves spriteRenderer component of first token. 
			
			for(int i = x + 1; i < gameManager.gridWidth; i++){ //for each token to the right of the x position of first token,
				GameObject other = gameManager.gridArray[i, y]; //save said token as other gameObject. 

				if(other != null){ //if other is not null:
					SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>(); //gets the spriteRenderer Component of this set other. 

					if(sr1.sprite == sr2.sprite){ //if the sprites are matching;
						matchLength++; //add to the match length integer, check next object. 
					} else { //otherwise stops for loop. 
						break;
					}
				} else { //if null stops for loop. 
					break;
				}
			}
		}
		
		return matchLength; //returns match lenght
	}

	//-----------------------------------
	public int GetVerticalMatchLength(int x, int y)
	{
		int matchLength = 1; //declaring matchlength integer. 

		GameObject first = gameManager.gridArray[x, y]; //declars the first token in the match as a GameObject. 

		if (first != null)
		{ //if first in match is not null:
			SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>(); //saves spriteRenderer component of first token. 

			for (int i = y + 1; i < gameManager.gridHeight; i++)
			{ //for each token to the right of the x position of first token,
				GameObject other = gameManager.gridArray[x, i]; //save said token as other gameObject. 

				if (other != null)
				{ //if other is not null:
					SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>(); //gets the spriteRenderer Component of this set other. 

					if (sr1.sprite == sr2.sprite)
					{ //if the sprites are matching;
						matchLength++; //add to the match length integer, check next object. 
					}
					else
					{ //otherwise stops for loop. 
						break;
					}
				}
				else
				{ //if null stops for loop. 
					break;
				}
			}
		}

		return matchLength; //returns match lenght
	}

	//-----------------------------------

	public virtual int RemoveMatches(){ //funciton to removed matched objects. 
		int numRemoved = 0; //declars integer value and sets to zero. 

		for(int x = 0; x < gameManager.gridWidth; x++){ //for each column of grid;
			for(int y = 0; y < gameManager.gridHeight ; y++){ //and each row in grid, 
				if(x < gameManager.gridWidth - 2 )
				{ 
				//Checks if this x is one of the last elements in the row, since the
				//match check needs to be performed through tthe first element of the grid row (left ot right)
					int horizonMatchLength = GetHorizontalMatchLength(x, y); //checks length of horizontal match and saves it as local integer. 
					

					if (horizonMatchLength > 2 )
					{ //if the match length is greater than 2,

						for(int i = x; i < x + horizonMatchLength; i++){ //for each token in the match, 

									GameObject token = gameManager.gridArray[i, y];  //saves object as a token gameObject. 
									//Destroy(token); //and DESTROYS it. 
									dualMatchList.Add(new Vector2Int(i, y)); //Adds each token in the match to a vector 2 int list, based on each token's x position in the grid. 
									//gameManager.gridArray[i, y] = null; //sets the token's position as null
									numRemoved++; //adds the amount of tokens removed from grid. 
								
						}
					}

				}
				//------------------------

				
				if (y < gameManager.gridHeight - 2)
				{
					//Checks if this x is one of the last elements in the row, since the
					//match check needs to be performed through tthe first element of the grid row (left ot right)

					int verticalMatchLength = GetVerticalMatchLength(x, y); //checks length of vertical match and saves it as local integer. 

					if (verticalMatchLength > 2)
					{ //if the match length is greater than 2,

						for (int i = y; i < y + verticalMatchLength; i++)
						{ //for each token in the match, 
							GameObject token = gameManager.gridArray[x, i];  //saves object as a token gameObject. 
							dualMatchList.Add(new Vector2Int(x,i)); //adds each token in a vertical match into the list based on y position in grid. 
							//gameManager.gridArray[x, i] = null; //sets the token's position as null
							numRemoved++; //adds the amount of tokens removed from grid. 
						}
					}
				}
				
				//------------------------


			}
		}

		//for each object in the list;
		for (int i = 0; i < dualMatchList.Count; i++)
		{
			Destroy(gameManager.gridArray[dualMatchList[i].x, dualMatchList[i].y]); //destroy each object based on it's position in the grid. 
			gameManager.gridArray[dualMatchList[i].x, dualMatchList[i].y] = null;	//sets the grid position to null so that it can be repopulated. 
		}
		dualMatchList.Clear(); //clears the list. 
		return numRemoved;//returns how many tokens were removed. 
	}
}
