using UnityEngine;
using System.Collections;

public class MatchManagerScript : MonoBehaviour {

	protected GameManagerScript gameManager; //reference to gamemanager

	public virtual void Start () { 
		gameManager = GetComponent<GameManagerScript>(); //sets gamemanager script reference. 
	}

	
	//Known Bugs:
	//1. NO Vertical Matches

	
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
			}
		}

		return match; //returns the bool value. 
	}

	public bool GridHasHorizontalMatch(int x, int y){ //checks if theres a horizontal match based on given grid position of objects. 
		//generating 3 gameobjects to assign checked objects from left to right.  
		GameObject token1 = gameManager.gridArray[x + 0, y];	
		GameObject token2 = gameManager.gridArray[x + 1, y];
		GameObject token3 = gameManager.gridArray[x + 2, y];
		
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
		
	public virtual int RemoveMatches(){ //funciton to removed matched objects. 
		int numRemoved = 0; //declars integer value and sets to zero. 

		for(int x = 0; x < gameManager.gridWidth; x++){ //for each position in grid width;
			for(int y = 0; y < gameManager.gridHeight ; y++){ //and each position in grid height, 
				if(x < gameManager.gridWidth - 2){ 
				//Checks if this x is one of the last elements in the row, since the
				//match check needs to be performed through tthe first element of the grid row (left ot right)

					int horizonMatchLength = GetHorizontalMatchLength(x, y); //checks length of horizontal match and saves it as local integer. 

					if(horizonMatchLength > 2){ //if the match length is greater than 2,

						for(int i = x; i < x + horizonMatchLength; i++){ //for each token in the match, 
							GameObject token = gameManager.gridArray[i, y];  //saves object as a token gameObject. 
							Destroy(token); //and DESTROYS it. 

							gameManager.gridArray[i, y] = null; //sets the token's position as null
							numRemoved++; //adds the amount of tokens removed from grid. 
						}
					}
				}
			}
		}
		
		return numRemoved;//returns how many tokens were removed. 
	}
}
