using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace AutoScrollCraft.UI {
	public class Title : MonoBehaviour {
		[SerializeField] private RectTransform[] titleMenus;
		private int currentSelect;
		// メニュー番号
		private const int GameStart = 0;
		private const int ScoreBoard = 1;
		private const int QuitGame = 2;

		private void Awake () {
		}

		private void Update () {
			var p = titleMenus[currentSelect].localPosition;
			p.x = Mathf.Abs ( Mathf.Sin ( Time.time * 3.0f ) ) * 100;
			titleMenus[currentSelect].localPosition = p;
		}

		public void OnMove ( BaseEventData data ) {
			// メニューの移動
			var axis = (data as AxisEventData).moveVector;
			currentSelect -= (int)axis.y;
			if (currentSelect < 0) currentSelect = 0;
			if (currentSelect >= titleMenus.Length) currentSelect = titleMenus.Length - 1;
			foreach (var o in titleMenus) {
				o.localPosition = new Vector3 ( 0.0f, o.localPosition.y, 0.0f );
			}
		}

		public void OnSubmit ( BaseEventData data ) {
			// 選択項目へ
			switch (currentSelect) {
				case GameStart:
					SceneManager.LoadScene ( "MainGame" );
					break;
				case ScoreBoard:
					TitleManager.Instance.ChangeScreen ( TitleManager.ScoreBoardScreen );
					break;
				case QuitGame:
					Application.Quit ();
					break;
				default:
					break;
			}
		}
	}
}