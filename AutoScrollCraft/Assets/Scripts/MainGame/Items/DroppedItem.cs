using UnityEngine;

namespace AutoScrollCraft.Items {
	public class DroppedItem : MonoBehaviour {
		[SerializeField] private Enums.Items item;
		public Enums.Items Item { get => item; set => item = value; }

		private void Update () {
			if (transform.position.y < -10) {
				Destroy ( gameObject );
			}
		}

		private void OnBecameInvisible () {
			Destroy ( gameObject );
		}
	}
}
