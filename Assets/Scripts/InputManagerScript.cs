using UnityEngine;
using System.Collections;

public class InputManagerScript : MonoBehaviour {

	protected GameManagerScript gameManager; //reference to gamemanager
	protected MoveTokensScript moveManager;	//reference to moveManager script. 
	protected GameObject selected = null;	//declares a selected gameObject by the player and sets it to null. 

	public virtual void Start () {
		moveManager = GetComponent<MoveTokensScript>(); //assigns movemanager script to reference. 
		gameManager = GetComponent<GameManagerScript>();	//assigns gameManager script to reference. 
	}
		
	public virtual void SelectToken(){	//function to select a token :)
		if(Input.GetMouseButtonDown(0)){ //if left mouse button is clicked, 
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //creates vector3 and sets it to the position in world space of the mouse. 
			
			Collider2D collider = Physics2D.OverlapPoint(mousePos); //checks if there is a collider where mouse is and saves that collider. 

			if(collider != null){ //if there is a collider:
				if(selected == null){ //and selected object reference is empty
					selected = collider.gameObject; //fill it with the collider at mousepoint. 
				} else { //other wise saves position of the selected object and the collider object. 
					Vector2 pos1 = gameManager.GetPositionOfTokenInGrid(selected);
					Vector2 pos2 = gameManager.GetPositionOfTokenInGrid(collider.gameObject);

					if(Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y) ==1){ //if absolute value of the difference between the two vectors x and y positions is one, 
						moveManager.SetupTokenExchange(selected, pos1, collider.gameObject, pos2, true); //switches the two object's positions, and states that they are reversible.
					}
					selected = null; //empties selected game object. 
				}
			}
		}

	}

}
