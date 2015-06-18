using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class MathExtensions  {

	public static int Squared(this int i){
		return i * i;
	}

	public static Vector3 ToVector3XZ(this Vector2 v){
		return new Vector3 (v.x, 0, v.y);
	}

	public static Vector3? GetClosest(this Vector3 origin, IEnumerable<Vector3> targets){
		Vector3? closest = null;
		float shortest = float.MaxValue;
		foreach (var v in targets) {
			var d = Vector3.SqrMagnitude(v - origin);
			if(d < shortest){
				closest = v;
				shortest = d;
			}
		}
		return closest;
	}

	public static Vector3? GetClosest(this Vector3 origin, IEnumerable<Vector3> targets, Func<Vector3, Vector3, bool> whereClause){
		Vector3? closest = null;
		float shortest = float.MaxValue;
		foreach (var v in targets) {
			if(!whereClause(origin, v)){
				continue;
			}

			var d = Vector3.SqrMagnitude(v - origin);
			if(d < shortest){
				closest = v;
				shortest = d;
			}
		}
		return closest;
	}

}
