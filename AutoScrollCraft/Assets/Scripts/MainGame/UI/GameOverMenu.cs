using System.Linq;
using AutoScrollCraft.Scene;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AutoScrollCraft.UI {
	public class GameOverMenu : MonoBehaviour {
		[SerializeField] private RectTransform[] menuList;
		[SerializeField] private RectTransform cursor;
		private int currentSelect = 0;
		// メニュー番号
		private const int PlayAgain = 0;
		private const int BackToTitle = 1;

		private void Start () {
			menuList[PlayAgain].localPosition.Set ( 0, 0, 0 );
			menuList[BackToTitle].localPosition.Set ( 0, -140, 0 );
			cursor.localPosition = menuList[currentSelect].localPosition;
		}

		public void OnMove ( BaseEventData data ) {
			// カーソル移動
			var axis = (data as AxisEventData).moveVector;
			currentSelect = UIFunctions.RevisionValue ( currentSelect - (int)axis.y, menuList.Count () - 1 );
			cursor.localPosition = menuList[currentSelect].localPosition;
		}

		public void OnSubmit ( BaseEventData data ) {
			switch (currentSelect) {
				case PlayAgain: SceneManager.LoadScene ( SceneList.MainGame ); break;
				case BackToTitle: SceneManager.LoadScene ( SceneList.Title ); break;
				default: break;
			}
		}
	}
}
