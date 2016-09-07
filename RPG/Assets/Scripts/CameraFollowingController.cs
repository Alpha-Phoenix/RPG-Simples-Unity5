using UnityEngine;
using System.Collections;

public class CameraFollowingController : MonoBehaviour {

	public Camera cam;
	public GameObject target;

	public SpriteRenderer mapSpriteRenderer;

	public Rect mapRect;
	public Rect camRect;

	// Use this for initialization
	void Start () {
		UpdateBounds ();
	}

	void UpdateBounds () {
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = cam.orthographicSize * 2;
		Bounds bounds = new Bounds(
			cam.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
		
		mapRect.xMin = mapSpriteRenderer.bounds.min.x;
		mapRect.yMin = mapSpriteRenderer.bounds.min.y;
		mapRect.xMax = mapSpriteRenderer.bounds.max.x;
		mapRect.yMax = mapSpriteRenderer.bounds.max.y;

		camRect.xMin = bounds.min.x;
		camRect.yMin = bounds.min.y;
		camRect.xMax = bounds.max.x;
		camRect.yMax = bounds.max.y;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateBounds ();

		if (camRect.xMin <= mapRect.xMin ||
			camRect.xMax >= mapRect.xMax ||
			camRect.yMin <= mapRect.yMin ||
			camRect.yMax >= mapRect.yMax) {
			Debug.Log ("N");
		} else {
			Vector3 currPos = target.transform.position;
			currPos.z = -1;
			cam.transform.position = currPos;
			;
		}
	}
}
