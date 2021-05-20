using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AutoScrollCraft.Actors.AI {
	public class SearchObject : MonoBehaviour {
		[SerializeField] string targetTag;
		[SerializeField] float viewingAngle;
		bool detected;
		public bool Detected { get => detected; }
		GameObject target;
		public GameObject Target { get => target; }

		void OnTriggerStay ( Collider collider ) {
			if (collider.tag == targetTag) {
				var d = collider.transform.position - transform.position;
				var a = Vector3.Angle ( transform.forward, d );
				if (a <= viewingAngle) {
					detected = true;
					target = collider.gameObject;
				}
				else {
					detected = false;
					target = null;
				}
			}
		}

		void OnTriggerExit ( Collider collider ) {
			if (collider.tag != targetTag) return;
			detected = false;
			target = null;
		}
	}
}
