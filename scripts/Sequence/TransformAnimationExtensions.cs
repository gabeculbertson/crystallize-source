using UnityEngine;
using System.Collections;

namespace Unity.Sequence {
	public static class TransformAnimationExtensions {

		public static Sequence RotateToFace(this Transform transform, Transform target, float speed = 360f){
			return new Sequence (RotateToFaceSequence (transform, target, speed));
		}

		static IEnumerator RotateToFaceSequence(Transform transform, Transform target, float speed){
			Vector3 forward = target.position - transform.position;
			float angle = Vector3.Angle (transform.forward, forward);
			Quaternion inital = transform.rotation;
			Quaternion final = Quaternion.LookRotation (forward, Vector3.up);
			for (float t = 0; t < 1f; t += (speed / angle) * Time.deltaTime) {
				transform.rotation = Quaternion.Lerp(inital, final, t);

				yield return null;
			}
			transform.rotation = final;
		}

	}
}
