using UnityEngine;

namespace AutoScrollCraft.UI {
	public class JumpingAnimation : MonoBehaviour {
		private RectTransform rect;
		[SerializeField] private float speed;
		[SerializeField] private float jumpScale;

		private void Awake () {
			rect = GetComponent<RectTransform> ();
		}

		private void Update () {
			var p = rect.localPosition;
			p.y = Mathf.Abs ( Mathf.Sin ( Time.realtimeSinceStartup * speed ) ) * jumpScale;
			rect.localPosition = p;
		}
	}
}
