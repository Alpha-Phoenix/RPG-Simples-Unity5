using UnityEngine;
using System.Collections;

public static class Utils {
	public static bool IsInside (Rect r1, Rect r2) {
		if (r1.xMin < r2.xMin ||
			r1.xMax > r2.xMax ||
		    r1.yMin < r2.yMin ||
		    r1.yMax > r2.yMax) {
			return false;
		} else {
			return true;
		}
	}
}
