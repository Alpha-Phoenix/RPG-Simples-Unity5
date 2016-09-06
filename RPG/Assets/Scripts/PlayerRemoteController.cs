using UnityEngine;
using System.Collections;

namespace MotionDebug {

	public class PlayerRemoteController : MonoBehaviour {

		public SquareBehaviour player;


		// Use this for initialization
		void Start () {
			/*
while (player.IsDoingSomething ())
				;
			player.LookAt (Direction.UP);

			while (player.IsDoingSomething ())
				;
			player.Wait (3);

			while (player.IsDoingSomething ())
				;
			player.LookAt (Direction.LEFT);

			while (player.IsDoingSomething ())
				;
			player.Wait (3);

			while (player.IsDoingSomething ())
				;
			player.LookAt (Direction.RIGHT);

			while (player.IsDoingSomething ())
				;
			player.Wait (3);

			while (player.IsDoingSomething ())
				;
			player.LookAt (Direction.DOWN);

			while (player.IsDoingSomething ())
				;
			player.Wait (3);

			WalkThroughPath (new Direction[] {
				Direction.DOWN,
				Direction.LEFT,
				Direction.UP,
				Direction.UP,
				Direction.RIGHT,
				Direction.RIGHT,
				Direction.DOWN,
				Direction.DOWN,
				Direction.LEFT,
				Direction.UP,
			});

			while (player.IsDoingSomething ())
				;
			player.Wait (3);

			RunThroughPath (new Direction[] {
				Direction.DOWN,
				Direction.LEFT,
				Direction.UP,
				Direction.UP,
				Direction.RIGHT,
				Direction.RIGHT,
				Direction.DOWN,
				Direction.DOWN,
				Direction.LEFT,
				Direction.UP,
			});
			*/
		}

		// Update is called once per frame
		void Update () {

		}

		void WalkThroughPath (Direction[] path) {

		}

		void RunThroughPath (Direction[] path) {

		}

		void Up () {
			player.MoveTo (Direction.UP, true);
		}

		void Down () {
			player.MoveTo (Direction.DOWN, false);
		}

		void Left () {
			player.MoveTo (Direction.LEFT, false);
		}

		void Right () {
			player.MoveTo (Direction.RIGHT, false);
		}
	}
}