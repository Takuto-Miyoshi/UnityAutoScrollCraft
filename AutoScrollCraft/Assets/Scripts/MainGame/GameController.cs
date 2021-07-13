using AutoScrollCraft.Sound;
using UnityEngine;

public class GameController : Singleton<GameController> {
	[SerializeField] private GameObject defaultCanvas;
	[SerializeField] private GameObject gameOverScreen;

	public override void Awake () {
		base.Awake ();

		defaultCanvas.SetActive ( true );
		gameOverScreen.SetActive ( false );
	}

	private void Start () {
		SoundManager.Instance.PlayMainGameBGM ();
	}

	public void ShowGameOverScreen () {
		gameOverScreen.SetActive ( true );
	}
}
