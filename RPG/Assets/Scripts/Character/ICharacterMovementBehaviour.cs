using UnityEngine;
using System.Collections;

public abstract class ICharacterMovementBehaviour : MonoBehaviour {
	public abstract Vector3 GetDestination ();
	public 	abstract bool SetDestination (Vector3 newDestination, float speed);

	public abstract Vector3 GetPosition ();
	public abstract bool IsMoving ();

	public abstract float GetSpeed ();
	public abstract bool SetSpeed (float velocity);

}
