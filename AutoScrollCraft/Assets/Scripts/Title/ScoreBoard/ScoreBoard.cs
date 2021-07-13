using AutoScrollCraft.Enums;
using AutoScrollCraft.Sound;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class ScoreBoard : MonoBehaviour {
		[SerializeField] private Text[] scoreList;

		private void Awake () {
			var ranking = ScoreManager.Instance.GetRanking ();

			for (int i = 0; i < scoreList.Length; i++) {
				scoreList[i].text = ranking[i].ToString ();
			}
		}

		public void OnCancel ( BaseEventData data ) {
			SoundManager.Instance.Play ( SE.Cancel );
			TitleManager.Instance.ChangeScreen ( TitleManager.TitleScreen );
		}
	}
}
