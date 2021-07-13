using ExtentionMethod;
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
			var value = Mathf.Abs ( Mathf.Sin ( Time.realtimeSinceStartup * speed ) ) * jumpScale;
			rect.localPosition = rect.localPosition.SetY ( value );
		}
	}
}
