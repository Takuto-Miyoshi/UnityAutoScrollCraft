using UnityEngine;
using UnityEngine.EventSystems;

public class TitleManager : Singleton<TitleManager> {
	[SerializeField] private GameObject[] screens;

	// 画面番号
	public const int TitleScreen = 0;
	public const int ScoreBoardScreen = 1;

	public void ChangeScreen ( int nextScreen ) {
		foreach (var o in screens) o.SetActive ( false );
		screens[nextScreen].SetActive ( true );
		EventSystem.current.SetSelectedGameObject ( screens[nextScreen] );
	}
}
