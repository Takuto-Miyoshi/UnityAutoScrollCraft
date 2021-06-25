using AutoScrollCraft.Actors;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	private Text text;
	[SerializeField] private Player player;
	private void Awake () {
		text = GetComponent<Text> ();
	}

	private void FixedUpdate () {
		// ゲームオーバー時は非表示にする
		text.text = (player.IsGameOver == true) ? "" : "SCORE : " + player.Status.Score;
	}
}
