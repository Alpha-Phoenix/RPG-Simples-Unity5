using UnityEngine;
using System.Collections;

//[RequireComponent (typeof (ICharacterMovementBehaviour))]
public class CharacterMovementController : MonoBehaviour {

	private ICharacterMovementBehaviour movementBehaviour;

	// Use this for initialization
	void Start () {
		movementBehaviour = new CharacterSmoothMovementBehaviour (this.gameObject, 1, new Rect(0, 0, 1200/24, 1200/24));
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 behaviuorDestination = movementBehaviour.GetDestination ();

		if (Input.GetKey (KeyCode.W)) {
			behaviuorDestination.y += GameGlobalConfigurations.TILE_SIZE;
		} else if (Input.GetKey (KeyCode.A)) {
			behaviuorDestination.x -= GameGlobalConfigurations.TILE_SIZE;
		} else if (Input.GetKey (KeyCode.S)) {
			behaviuorDestination.y -= GameGlobalConfigurations.TILE_SIZE;
		} else if (Input.GetKey (KeyCode.D)) {
			behaviuorDestination.x += GameGlobalConfigurations.TILE_SIZE;
		}
			
		movementBehaviour.SetDestination (behaviuorDestination, 5);
	}
}
