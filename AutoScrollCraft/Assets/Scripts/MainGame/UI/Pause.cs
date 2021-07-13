using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class Pause : Singleton<Pause> {
		[SerializeField] private GameObject pauseMenu;
		[SerializeField] private GameObject option;
		[SerializeField] private GameObject status;
		[SerializeField] private EventSystem eventSystem;
		private bool pause;
		[SerializeField] private Text[] textList;
		[SerializeField] private Image cursor;
		private const int Continue = 0;
		private const int Option = 1;
		private const int Resume = 2;
		private const int BackToTitle = 3;
		private int currentSelect = 0;
		private float NormalTimeScale = 1.0f;
		private float PauseTimeScale = 0.0f;

		public override void Awake () {
			base.Awake ();

			TogglePause ( false );
		}

		/// <summary>
		/// ポーズ状態を反転させる
		/// </summary>
		/// <returns>変更後の状態</returns>
		public bool TogglePause () {
			pause = !pause;
			SetTimeScale ();
			ShowPauseMenu ();
			return pause;
		}

		/// <summary>
		/// ポーズ状態を設定する
		/// </summary>
		/// <param name="mode">設定する値</param>
		public void TogglePause ( bool mode ) {
			pause = mode;
			SetTimeScale ();
			ShowPauseMenu ();
		}

		/// <summary>
		/// ポーズの状態を元にTimeScaleを変更する
		/// </summary>
		private void SetTimeScale () {
			Time.timeScale = (pause == true) ? PauseTimeScale : NormalTimeScale;
		}

		private void ShowPauseMenu () {
			pauseMenu.SetActive ( pause );
			status.SetActive ( !pause );
		}

		public void OnMove ( BaseEventData data ) {
			if (pause == false) return;

			var axis = (data as AxisEventData).moveVector;
			currentSelect = UIFunctions.RevisionValue ( currentSelect - (int)axis.y, textList.Length - 1 );
			cursor.rectTransform.position = textList[currentSelect].rectTransform.position;
		}

		public void OnSubmit ( BaseEventData data ) {
			if (pause == false) return;

			switch (currentSelect) {
				case Continue: TogglePause ( false ); return;
				case Option:
					option.SetActive ( true );
					eventSystem.SetSelectedGameObject ( option );
					return;
				case Resume:
					TogglePause ( false );
					SceneManager.LoadScene ( "MainGame" );
					return;
				case BackToTitle:
					TogglePause ( false );
					SceneManager.LoadScene ( "Title" );
					return;

			}
		}
	}
}
