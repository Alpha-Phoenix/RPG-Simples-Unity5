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
		RUNNING = WALKING // Ainda sem animação de correr
	}

	public KeyCode[] moveKeys = {
		KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D
	};

	public PlayerDirection direction; // Direção para a qual o player está olhando
	public PlayerState state;		  // Estado atual
	public Vector3 destiny;			  // Direção para a qual o player deve andar
	public float velocity = 3.0f;     // Velocidade de animação
	public float tileSize;            // Largura e altura de um tile em Unity units
	public float delayTime;			  // O player só se movimenta para uma determinada direção se o keyRecord dessa
									  // direção for igual ao delayTime
	public Animator animator;		  // Controlador de animações

	public Dictionary<KeyCode, float> keyRecord; // Armazena tempo em que as teclas estão pressionadas

	// Use this for initialization
	void Start () {
		destiny = transform.position;
		tileSize = 1f;

		// Inicia parado virado para cima
		direction = PlayerDirection.UP;
		state = PlayerState.STOPPED;

		keyRecord = new Dictionary<KeyCode, float> ();
		delayTime = 0.15f;
		animator = GetComponent<Animator> ();

		// Inicia as keyRecords com 0
		foreach (var key in moveKeys)
			keyRecord.Add (key, 0f);
	}

	// Aumenta ou diminui uma keyRecord se a key estiver pressionada
	void UpdateKeyRecords () {
		foreach (var key in moveKeys) {
			if (Input.GetKey (key))
				keyRecord [key] += Time.deltaTime;
			else {
				// Esse if é um truque para que não seja preciso esperar o tempo inteiro
				// para repetir um movimento anterior
				if (!(IsMoving() && direction == KeyToDirection(key)))
					keyRecord [key] -= 0.005f;
			}

			// Mantém a as keysRecords em um intervalo [0, delayTime]
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
		
		if (IsMoving ())
			state = Input.GetKey (KeyCode.LeftShift) ? PlayerState.RUNNING : PlayerState.WALKING;
		else {
			if (!TryingToMove())
				state = PlayerState.STOPPED;
		}
		animator.SetInteger ("PlayerState", (int) state);
		animator.SetInteger ("PlayerDirection", (int) direction);
	}

	PlayerDirection KeyToDirection(KeyCode key) {
		if (key == KeyCode.W)
			return PlayerDirection.UP;
		if (key == KeyCode.A)
			return PlayerDirection.LEFT;
		if (key == KeyCode.S)
			return PlayerDirection.DOWN;
		if (key == KeyCode.D)
			return PlayerDirection.RIGHT;

		Debug.LogError ("INVALID KEY");
		return PlayerDirection.UP;
	}

	void GetUserInput() {
		if (IsMoving ())
			return;

		foreach (var key in keyRecord) {
			// Condição para que possa se mover sem que seja necessário esperar o keyRecord atingir o delayTime
			if (key.Value == delayTime) {
				switch (key.Key) {
				case KeyCode.W:
					if (direction == KeyToDirection(key.Key) && keyRecord[key.Key] == delayTime)
						destiny.y += tileSize;
					else
						direction = KeyToDirection(key.Key);
					break;
				case KeyCode.A:
					if (direction == KeyToDirection(key.Key) && keyRecord[key.Key] == delayTime)
						destiny.x -= tileSize;
					else
						direction = KeyToDirection(key.Key);
					break;
				case KeyCode.S:
					if (direction == KeyToDirection(key.Key) && keyRecord[key.Key] == delayTime)
						destiny.y -= tileSize;
					else
						direction = KeyToDirection(key.Key);
					break;
				case KeyCode.D:
					if (direction == KeyToDirection(key.Key) && keyRecord[key.Key] == delayTime)
						destiny.x += tileSize;
					else
						direction = KeyToDirection(key.Key);
					break;
				}
				return;
			}
		}

		if (Input.GetKey(KeyCode.W)) {
			if (direction == PlayerDirection.UP) {
				if (keyRecord [KeyCode.W] == delayTime)
					destiny.y += tileSize;
			} else
				direction = PlayerDirection.UP;
		}

		else if (Input.GetKey(KeyCode.A)) {
			if (direction == PlayerDirection.LEFT) {
				if (keyRecord [KeyCode.A] == delayTime)
					destiny.x -= tileSize;
			}
			else
				direction = PlayerDirection.LEFT;
		}

		else if (Input.GetKey(KeyCode.S)) {
			if (direction == PlayerDirection.DOWN) {
				if (keyRecord [KeyCode.S] == delayTime)
					destiny.y -= tileSize;
			}
			else
				direction = PlayerDirection.DOWN;
		}

		else if (Input.GetKey(KeyCode.D)) {
			if (direction == PlayerDirection.RIGHT) {
				if (keyRecord [KeyCode.D] == delayTime)
					destiny.x += tileSize;
			}
			else
				direction = PlayerDirection.RIGHT;
		}
	}

	void Move() {
		Vector3 toAdd = destiny - transform.position;
		Vector3 step = toAdd.normalized * velocity * Time.deltaTime;
		if (toAdd.magnitude <= step.magnitude) {
			transform.position = destiny;
		}
		else {
			transform.position += step;
		}

		if (step.x < 0 && Mathf.Abs(step.x) > Mathf.Abs(step.y)) direction = PlayerDirection.LEFT;
		if (step.x > 0 && Mathf.Abs(step.x) > Mathf.Abs(step.y)) direction = PlayerDirection.RIGHT;
		if (step.y < 0 && Mathf.Abs(step.x) <= Mathf.Abs(step.y)) direction = PlayerDirection.DOWN;
		if (step.y > 0 && Mathf.Abs(step.x) <= Mathf.Abs(step.y)) direction = PlayerDirection.UP;
	}

	bool IsMoving() {
		return transform.position != destiny;
	}

	bool TryingToMove () {
		foreach (var key in moveKeys)
			if (Input.GetKey (key))
				return true;
		return false;
	}


	void OnTriggerEnter2D (Collider2D col) {

		switch (direction) {
		case PlayerDirection.UP:
			transform.Translate (Vector3.down);
			destiny = transform.position;
			break;
		}

		Debug.Log ("TRIGGER");
	}
}
