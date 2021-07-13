using ExtentionMethod;
using UnityEngine;

namespace AutoScrollCraft.UI {
	public class FollowPosition : MonoBehaviour {
		[SerializeField] GameObject followTarget;
		RectTransform rectTransform;
		private const float AdjustmentPosZ = -500.0f;

		private void Awake () {
			rectTransform = GetComponent<RectTransform> ();
		}

		void Update () {
			// ターゲットと同じ位置にする
			rectTransform.position = followTarget.transform.position;

			rectTransform.localPosition = rectTransform.localPosition.AddZ ( AdjustmentPosZ );
		}
	}
}
