using System.Collections.Generic;
using AutoScrollCraft.Enums;
using AutoScrollCraft.Sound;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AutoScrollCraft.UI {
	public class TitleManager : Singleton<TitleManager> {
		[SerializeField] private List<GameObject> screens = new List<GameObject> ();

		// 画面番号
		public const int TitleScreen = 0;
		public const int ScoreBoardScreen = 1;
		public const int OptionScreen = 2;

		private async void Start () {
			SoundManager.Instance.Play ( BGM.Title );
			// 他のオブジェクトの読み込みを待つ
			await UniTask.Delay ( 10 );
			ChangeScreen ( TitleScreen );
		}

		// タイトルシーン内で画面を切り替える
		public void ChangeScreen ( int nextScreen ) {
			screens.ForEach ( e => e.SetActive ( false ) );
			screens[nextScreen].SetActive ( true );
			EventSystem.current.SetSelectedGameObject ( screens[nextScreen] );
		}
	}
}
