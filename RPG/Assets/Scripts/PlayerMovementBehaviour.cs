using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovementBehaviour : MonoBehaviour {

	public enum PlayerDirection {
		UP    = 1,
		RIGHT = 2,
		DOWN  = 3,
		LEFT  = 4
	}

	public enum PlayerState {
		STOPPED = 1,
		WALKING = 2,
		RUNNING = 3
	}

	public KeyCode[] moveKeys = {
		KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D
	};

	public PlayerDirection direction;
	public PlayerState state;
	public Vector3 destino;
	public float velocity = 3.0f;
	public int tileHeight;
	public int tileWidth;
	public float delayTime;
	public Animator animator;

	public Dictionary<KeyCode, float> keyRecord;

	// Use this for initialization
	void Start () {
		destino = transform.position;
		tileWidth = 1;
		tileHeight = 1;
		direction = PlayerDirection.UP;
		state = PlayerState.STOPPED;
		keyRecord = new Dictionary<KeyCode, float> ();
		delayTime = 0.15f;
		animator = GetComponent<Animator> ();

		foreach (var key in moveKeys)
			keyRecord.Add (key, 0f);
	}

	void UpdateKeyRecords () {
		foreach (var key in moveKeys) {
			if (Input.GetKey (key))
				keyRecord [key] += Time.deltaTime;
			else
				keyRecord [key] -= Time.deltaTime;

			if (keyRecord [key] < 0)
				keyRecord [key] = 0;
			else if (keyRecord [key] > delayTime)
				keyRecord [key] = delayTime;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		UpdateKeyRecords ();
		GetUserInput ();
		Move ();
		UpdatePlayerAnimationState ();
	}

	void UpdatePlayerAnimationState () {
		if (TryingToMove ()) {
			if (IsMoving ())
				state = Input.GetKey (KeyCode.LeftShift) ? PlayerState.RUNNING : PlayerState.WALKING;
			else
				state = PlayerState.STOPPED;
		} else 
			state = PlayerState.STOPPED;
		animator.SetInteger ("PlayerState", (int) state);
		animator.SetInteger ("PlayerDirection", (int) direction);
	}

	void GetUserInput() {
		if (IsMoving ())
			return;

		if (Input.GetKey(KeyCode.W)) {
			if (direction == PlayerDirection.UP) {
				if (keyRecord [KeyCode.W] == delayTime)
					destino.y += tileHeight;
			} else
				direction = PlayerDirection.UP;
		}

		else if (Input.GetKey(KeyCode.A)) {
			if (direction == PlayerDirection.LEFT) {
				if (keyRecord [KeyCode.A] == delayTime)
					destino.x -= tileWidth;
			}
			else
				direction = PlayerDirection.LEFT;
		}

		else if (Input.GetKey(KeyCode.S)) {
			if (direction == PlayerDirection.DOWN) {
				if (keyRecord [KeyCode.S] == delayTime)
					destino.y -= tileHeight;
			}
			else
				direction = PlayerDirection.DOWN;
		}

		else if (Input.GetKey(KeyCode.D)) {
			if (direction == PlayerDirection.RIGHT) {
				if (keyRecord [KeyCode.D] == delayTime)
					destino.x += tileWidth;
			}
			else
				direction = PlayerDirection.RIGHT;
		}
	}

	void Move() {
		Vector3 paraPercorrer = destino - transform.position;
		Vector3 passo = paraPercorrer.normalized * velocity * Time.deltaTime;
		if (paraPercorrer.magnitude <= passo.magnitude) {
			transform.position = destino;
		}
		else {
			transform.position += passo;
		}

		if (passo.x < 0 && Mathf.Abs(passo.x) > Mathf.Abs(passo.y)) direction = PlayerDirection.LEFT;
		if (passo.x > 0 && Mathf.Abs(passo.x) > Mathf.Abs(passo.y)) direction = PlayerDirection.RIGHT;
		if (passo.y < 0 && Mathf.Abs(passo.x) <= Mathf.Abs(passo.y)) direction = PlayerDirection.DOWN;
		if (passo.y > 0 && Mathf.Abs(passo.x) <= Mathf.Abs(passo.y)) direction = PlayerDirection.UP;
	}

	bool IsMoving() {
		return transform.position != destino;
	}

	bool TryingToMove () {
		foreach (var key in moveKeys)
			if (Input.GetKey (key))
				return true;
		return false;
	}
}
