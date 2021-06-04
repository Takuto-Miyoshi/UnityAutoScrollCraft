using AutoScrollCraft.Enums;
using UnityEngine;

namespace AutoScrollCraft.Items {
	public class DroppedItem : MonoBehaviour {
		[SerializeField] private Enums.Items item;
		public Enums.Items Item { get => item; set => item = value; }

		// Update is called once per frame
		private void Update () {
			if (transform.position.y < -50) {
				Destroy ( gameObject );
			}
		}

		private void OnBecameInvisible () {
			Destroy ( gameObject, 3.0f );
		}
	}
}
