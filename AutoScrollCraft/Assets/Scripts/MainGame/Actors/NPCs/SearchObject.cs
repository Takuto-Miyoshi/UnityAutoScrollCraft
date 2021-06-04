using UnityEngine;

namespace AutoScrollCraft.Actors.AI {
	public class SearchObject : MonoBehaviour {
		[SerializeField] private string targetTag;
		[SerializeField] private float viewingAngle;
		private bool detected;
		public bool Detected { get => detected; }
		private GameObject target;
		public GameObject Target { get => target; }

		private void OnTriggerStay ( Collider collider ) {
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

		private void OnTriggerExit ( Collider collider ) {
			if (collider.tag != targetTag) return;
			detected = false;
			target = null;
		}
	}
}
