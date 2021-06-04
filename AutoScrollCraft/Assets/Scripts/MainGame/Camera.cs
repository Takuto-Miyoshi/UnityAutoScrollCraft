using UnityEngine;

namespace AutoScrollCraft {
	public class Camera : MonoBehaviour {
		[SerializeField] private float speed;

		private void FixedUpdate () {
			// 右へスクロール
			var x = speed;
			transform.Translate ( x, 0, 0 );
		}
	}
}
