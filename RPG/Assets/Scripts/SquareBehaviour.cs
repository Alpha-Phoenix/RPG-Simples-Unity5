using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MotionDebug {

	public enum Direction {
		UP    = 1,
		RIGHT = 2,
		DOWN  = 3,
		LEFT  = 4
	}

	public enum AnimationState {
		WAITING = 1,
		STOPPED = 1,
		WALKING = 2,
		RUNNING = 3
	}

	public class SquareBehaviour : MonoBehaviour {

		private Direction direction;
		private Vector3 destiny;
		private bool doingSomething;
		private float waitingTime;
		private AnimationState animState;
		private int tileSize;
		private float velocity;
		private float delayTime;
		private Animator animator;

		private KeyCode[] moveKeys = {
			KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D
		};

		private Dictionary<KeyCode, float> keyRecord;

		void Start () {
			direction = Direction.UP;
			destiny = transform.position;
			doingSomething = false;
			waitingTime = 0;
			animState = AnimationState.STOPPED;
			tileSize = 1;
			velocity = 3f;
			delayTime = 0.15f;
			animator = GetComponent<Animator> ();

			keyRecord = new Dictionary<KeyCode, float> ();

			foreach (var key in moveKeys)
				keyRecord.Add (key, 0f);
		}

		void Update () {
			UpdateKeyRecords ();
			GetUserInput ();
			Move ();
			UpdateWaitingTime ();

			animator.SetInteger ("PlayerState", (int) animState);
			animator.SetInteger ("PlayerDirection", (int) direction);
		}

		void GetUserInput() {
			foreach (var key in keyRecord) {
				if (key.Value == delayTime) {
					MoveTo (KeyToDirection (key.Key), Input.GetKeyDown(KeyCode.LeftShift));
					break;
				}
			}
		}

		Direction KeyToDirection(KeyCode key) {
			if (key == KeyCode.W)
				return Direction.UP;
			if (key == KeyCode.A)
				return Direction.LEFT;
			if (key == KeyCode.S)
				return Direction.DOWN;
			if (key == KeyCode.D)
				return Direction.RIGHT;

			Debug.LogError ("INVALID KEY");
			return Direction.UP;
		}

		void UpdateKeyRecords () {
			foreach (var key in moveKeys) {
				if (Input.GetKey (key))
					keyRecord [key] += Time.deltaTime;
				else
					keyRecord [key] -= 0.001f;

				if (keyRecord [key] < 0)
					keyRecord [key] = 0;
				else if (keyRecord [key] > delayTime)
					keyRecord [key] = delayTime;
			}
		}

		void UpdateWaitingTime () {
			if (waitingTime > 0) {
				waitingTime -= Time.deltaTime;
				animState = AnimationState.WAITING;

				if (animState <= 0) {
					animState = 0;
					animState = AnimationState.STOPPED;
				}
			}
		}

		public void LookAt (Direction direction) {
			if (animState == AnimationState.STOPPED)
				this.direction = direction;
		}

		public bool CanMoveTo (Direction direction) {
			return true;
		}


		public void Wait(float time) {
			if (time <= 0)
				return;
			
			if (animState == AnimationState.STOPPED) {
				waitingTime = time;
				animState = AnimationState.WAITING;
				doingSomething = true;
			}
		}

		public void MoveTo (Direction direction, bool run) {
			if (animState == AnimationState.STOPPED && CanMoveTo(direction)) {
				doingSomething = true;
				animState = run ? AnimationState.RUNNING : AnimationState.WALKING;

				destiny = transform.position;

				switch (direction) {

				case Direction.UP:
					destiny.y += tileSize;
					break;
				case Direction.DOWN:
					destiny.y -= tileSize;
					break;
				case Direction.LEFT:
					destiny.x -= tileSize;
					break;
				case Direction.RIGHT:
					destiny.x += tileSize;
					break;

				}

				Move ();
			}
		}

		public bool IsDoingSomething () {
			if (transform.position != destiny || animState == AnimationState.WAITING)
				return true;
			
			return false;
		}

		private void Move () {
			Vector3 paraPercorrer = destiny - transform.position;
			Vector3 passo = paraPercorrer.normalized * velocity * Time.deltaTime;

			if (paraPercorrer.magnitude <= passo.magnitude)
				transform.position = destiny;
			else 
				transform.position += passo;

			if (passo.x < 0 && Mathf.Abs(passo.x) > Mathf.Abs(passo.y)) direction = Direction.LEFT;
			if (passo.x > 0 && Mathf.Abs(passo.x) > Mathf.Abs(passo.y)) direction = Direction.RIGHT;
			if (passo.y < 0 && Mathf.Abs(passo.x) <= Mathf.Abs(passo.y)) direction = Direction.DOWN;
			if (passo.y > 0 && Mathf.Abs(passo.x) <= Mathf.Abs(passo.y)) direction = Direction.UP;

			doingSomething = transform.position == destiny ? false : true;
			animState = doingSomething ? animState : AnimationState.STOPPED;
		}
	}

}