using UnityEngine;

namespace AutoScrollCraft.UI {
	public class FollowPosition : MonoBehaviour {
		[SerializeField] GameObject followTarget;
		RectTransform rectTransform;
		private void Awake () {
			rectTransform = GetComponent<RectTransform> ();
		}

		void Update () {
			// ターゲットと同じ位置にした後、位置を調整する
			var p = followTarget.transform.position;
			p.y += 0;
			rectTransform.position = p;
			p = rectTransform.localPosition;
			p.z -= 500;
			rectTransform.localPosition = p;
		}
	}
}
