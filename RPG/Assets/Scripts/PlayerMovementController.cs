using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {

	public PlayerMovementBehaviour behaviour;

	// Use this for initialization
	void Awake () {
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 behaviuorDestination = behaviour.GetDestination ();

		//behaviuorDestination.x += GameGlobalConfigurations.TILE_SIZE;

		if (Input.GetKey (KeyCode.W)) {
			behaviuorDestination.y += GameGlobalConfigurations.TILE_SIZE;
		} else if (Input.GetKey (KeyCode.A)) {
			behaviuorDestination.x -= GameGlobalConfigurations.TILE_SIZE;
		} else if (Input.GetKey (KeyCode.S)) {
			behaviuorDestination.y -= GameGlobalConfigurations.TILE_SIZE;
		} else if (Input.GetKey (KeyCode.D)) {
			behaviuorDestination.x += GameGlobalConfigurations.TILE_SIZE;
		}

		behaviour.SetDestination (behaviuorDestination, Input.GetKey (KeyCode.LeftShift));

	}
}
