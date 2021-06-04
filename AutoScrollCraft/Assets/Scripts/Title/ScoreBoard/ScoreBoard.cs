using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class ScoreBoard : MonoBehaviour {
		[SerializeField] private Text[] scoreList;

		private void Awake () {
			var s = ScoreManager.Instance.GetRanking ();
			for (int i = 0; i < scoreList.Length; i++) {
				scoreList[i].text = s[i].ToString ();
			}
		}

		private void Start () {

		}

		public void OnCancel ( BaseEventData data ) {
			TitleManager.Instance.ChangeScreen ( TitleManager.TitleScreen );
		}
	}
}
