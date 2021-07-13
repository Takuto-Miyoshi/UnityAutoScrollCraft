using System.Collections.Generic;
using AutoScrollCraft.Enums;
using AutoScrollCraft.Scene;
using AutoScrollCraft.Sound;
using ExtentionMethod;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AutoScrollCraft.UI {
	public class Title : MonoBehaviour {
		[SerializeField] private List<RectTransform> titleMenus = new List<RectTransform> ();
		[SerializeField] private int currentSelect;
		private const float AnimationSpeed = 3.0f;  // アニメーションの速度
		private const float MoveScale = 100.0f; // 動くアニメーションの移動量

		// メニュー番号
		private const int GameStart = 0;
		private const int ScoreBoard = 1;
		private const int Option = 2;
		private const int QuitGame = 3;

		private void Update () {
			var value = Mathf.Abs ( Mathf.Sin ( Time.time * AnimationSpeed ) ) * MoveScale;
			titleMenus[currentSelect].localPosition = titleMenus[currentSelect].localPosition.SetX ( value );
		}

		public void OnMove ( BaseEventData data ) {
			var previous = currentSelect;

			// メニューの移動
			var axis = (data as AxisEventData).moveVector.y;
			currentSelect = UIFunctions.RevisionValue ( currentSelect - (int)axis, titleMenus.Count - 1 );
			// 選択項目が変われば再生
			if (previous != currentSelect) SoundManager.Instance.Play ( SE.Cursor );
			// 表示位置を戻す
			titleMenus.ForEach ( x => x.localPosition.Set ( 0.0f, x.localPosition.y, 0.0f ) );
		}

		public void OnSubmit ( BaseEventData data ) {
			SoundManager.Instance.Play ( SE.Submit );

			// 選択項目へ
			switch (currentSelect) {
				case GameStart: SceneManager.LoadScene ( SceneList.MainGame ); break;
				case ScoreBoard: TitleManager.Instance.ChangeScreen ( TitleManager.ScoreBoardScreen ); break;
				case Option: TitleManager.Instance.ChangeScreen ( TitleManager.OptionScreen ); break;
				case QuitGame: Application.Quit (); break;
				default: break;
			}
		}
	}
}