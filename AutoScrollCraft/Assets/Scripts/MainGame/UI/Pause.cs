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

		public override void Awake () {
			base.Awake ();
			TogglePause ( false );
		}

		public bool TogglePause () {
			pause = !pause;
			Time.timeScale = (pause == true) ? 0.0f : 1.0f;
			ShowPauseMenu ();
			return pause;
		}

		public void TogglePause ( bool mode ) {
			pause = mode;
			Time.timeScale = (pause == true) ? 0.0f : 1.0f;
			ShowPauseMenu ();
		}

		private void ShowPauseMenu () {
			pauseMenu.SetActive ( pause );
			status.SetActive ( !pause );
		}

		public void OnMove ( BaseEventData data ) {
			if (pause == false) return;

			var axis = (data as AxisEventData).moveVector;
			currentSelect = UIFunctions.RevisionValue ( currentSelect - (int)axis.y, textList.Length );
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
