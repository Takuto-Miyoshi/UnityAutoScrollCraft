using System.Collections.Generic;
using AutoScrollCraft.UI;
using ExtentionMethod;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AutoScrollCraft.Option {
	public class OptionManager : Singleton<OptionManager> {
		[SerializeField] private List<OptionBase> optionList = new List<OptionBase> ();
		private int currentSelect = 0;
		private const float DeActiveX = 300.0f;
		private const float ActiveX = 100.0f;
		private enum ReturnTo {
			Title,
			Pause,
		};
		[SerializeField] private ReturnTo returnTo;
		[SerializeField] private EventSystem eventSystem;
		[SerializeField] private GameObject pauseCase;

		public override void Awake () {
			base.Awake ();

			PositionAdjustment ( currentSelect, -1 );
		}

		public void OnMove ( BaseEventData data ) {
			var axis = (data as AxisEventData).moveVector;
			// 選択項目の切り替え(y)
			if ((int)axis.y != 0) {
				var before = currentSelect;
				currentSelect = UIFunctions.RevisionValue ( currentSelect - (int)axis.y, optionList.Count - 1 );
				PositionAdjustment ( currentSelect, before );
			}
			// 値の変更(x)
			else if ((int)axis.x != 0) {
				optionList[currentSelect].AxisAction ( (int)axis.x );
			}
		}

		public void OnSubmit ( BaseEventData data ) {
			optionList[currentSelect].CallAction ();
		}

		public void OnCancel ( BaseEventData data ) {
			switch (returnTo) {
				case ReturnTo.Title:
					TitleManager.Instance.ChangeScreen ( TitleManager.TitleScreen );
					break;
				case ReturnTo.Pause:
					eventSystem.SetSelectedGameObject ( pauseCase );
					gameObject.SetActive ( false );
					break;
				default: break;
			}
		}

		/// <summary>
		/// 選択中の項目の位置を調整する @n 負の数で位置調整をスキップ
		/// </summary>
		/// <param name="current">現在選択中の要素番号</param>
		/// <param name="before">前回選択していた要素番号</param>
		private void PositionAdjustment ( int current, int before ) {
			if (current == before) return;

			// 前回選択していた要素の位置を戻す
			if (before >= 0) {
				optionList[before].transform.localPosition = optionList[before].transform.localPosition.SetX ( DeActiveX );
				optionList[before].Operatable = false;
			}

			// 選択中の項目を飛び出させる
			if (current >= 0) {
				optionList[current].transform.localPosition = optionList[current].transform.localPosition.AddX ( ActiveX );
				optionList[current].Operatable = true;
			}
		}
	}
}
