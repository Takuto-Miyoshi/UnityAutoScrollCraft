using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace AutoScrollCraft.UI {
	public class GameOverMenu : MonoBehaviour {
		[SerializeField] private RectTransform[] menuList;
		[SerializeField] private RectTransform cursor;
		private int currentSelect = 0;
		// メニュー番号
		private const int PlayAgain = 0;
		private const int BackToTitle = 1;

		private void Start () {
			menuList[PlayAgain].localPosition = new Vector3 ( 0, 0, 0 );
			menuList[BackToTitle].localPosition = new Vector3 ( 0, -140, 0 );
			cursor.localPosition = menuList[currentSelect].localPosition;
		}

		public void OnMove ( BaseEventData data ) {
			// カーソル移動
			var axis = (data as AxisEventData).moveVector;
			currentSelect -= (int)axis.y;
			if (currentSelect < 0) currentSelect = 0;
			if (currentSelect >= menuList.Length) currentSelect = menuList.Length - 1;
			cursor.localPosition = menuList[currentSelect].localPosition;
		}

		public void OnSubmit ( BaseEventData data ) {
			switch (currentSelect) {
				case PlayAgain: SceneManager.LoadScene ( "MainGame" ); break;
				case BackToTitle: SceneManager.LoadScene ( "Title" ); break;
				default: break;
			}
		}
	}
}
