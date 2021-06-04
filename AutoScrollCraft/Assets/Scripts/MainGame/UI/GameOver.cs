using System;
using AutoScrollCraft.Actors;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
	private enum Phase {
		Wait,
		OpenBackGround,
		ShowGameOver,
		ScoreAnimation,
		Result
	}
	private Phase phase = Phase.OpenBackGround;
	private enum ScorePhase {
		Wait,
		Distance,
		Kill,
		Craft,
		Result
	}
	private ScorePhase scorePhase = ScorePhase.Distance;
	[SerializeField] private RectTransform backGround;
	[SerializeField] private RectTransform gameOver;
	[SerializeField] private Text generalScore;
	[SerializeField] private Text anyScore;
	private float generalScoreNum;
	private float anyScoreNum;
	private string anyScoreStr;
	[SerializeField] private Player player;
	[SerializeField] private GameObject menu;
	private void Awake () {
		backGround.localPosition = new Vector3 ( 0, -550, 0 );
		backGround.sizeDelta = Vector2.zero;
		gameOver.localPosition = Vector3.zero;
		gameOver.gameObject.SetActive ( false );
		generalScore.rectTransform.localPosition = new Vector2 ( 0, -200 );
		generalScore.gameObject.SetActive ( false );
		anyScore.rectTransform.localPosition = Vector2.zero;
		anyScore.gameObject.SetActive ( false );
		ScoreManager.Instance.UpdateRanking ( player.Status.Score );
		menu.SetActive ( false );
	}

	private void FixedUpdate () {
		switch (phase) {
			case Phase.Wait: break;
			case Phase.OpenBackGround: OpenBackGround (); break;
			case Phase.ShowGameOver: ShowGameOver (); break;
			case Phase.ScoreAnimation: ScoreAnimation (); break;
			case Phase.Result: GameOverAnimation (); break;
			default: break;
		}
	}

	/// <summary>
	/// ディレイを付けてアニメーションフェーズを変更する
	/// </summary>
	/// <param name="next">次のフェーズ</param>
	private async void ChangePhase ( Phase next ) {
		phase = Phase.Wait;
		await UniTask.Delay ( TimeSpan.FromSeconds ( 0.5f ) );
		phase = next;
		menu.SetActive ( phase == Phase.Result );
	}

	// ゲームオーバーウィンドウを開く
	private void OpenBackGround () {
		// 60回で目的の位置まで移動させる
		var p = backGround.localPosition;
		p.y += 550.0f / 60;
		var s = backGround.sizeDelta;
		s.x += 1700.0f / 60;
		s.y += 900.0f / 60;
		// 目的の位置まで来たら位置を整える
		if (s.x > 1700.0f) {
			p.y = 0.0f;
			s = new Vector2 ( 1700, 900 );
			gameOver.gameObject.SetActive ( true );
			ChangePhase ( Phase.ShowGameOver );
		}
		backGround.localPosition = p;
		backGround.sizeDelta = s;
	}

	// ゲームオーバーテキストを表示する
	private void ShowGameOver () {
		var p = gameOver.localPosition;
		p.y += 300.0f / 30;
		if (p.y > 300.0f) {
			p.y = 300.0f;
			anyScoreNum = player.DistanceScore;
			generalScore.gameObject.SetActive ( true );
			anyScore.gameObject.SetActive ( true );
			ChangePhase ( Phase.ScoreAnimation );
		}
		gameOver.localPosition = p;
	}

	// Sin波でゆらゆらする
	private void GameOverAnimation () {
		var p = gameOver.localPosition;
		p.y += Mathf.Sin ( Time.time * 3.0f );
		gameOver.localPosition = p;
	}

	// スコアをアニメーションで表示する
	private void ScoreAnimation () {
		GameOverAnimation ();
		switch (scorePhase) {
			case ScorePhase.Wait: break;
			case ScorePhase.Distance:
				anyScoreStr = "距離スコア";
				GeneralScoreAnim ( player.DistanceScore, player.DistanceScore, ScorePhase.Kill );
				break;
			case ScorePhase.Kill:
				anyScoreStr = "討伐スコア";
				GeneralScoreAnim ( player.KillScore, player.DistanceScore + player.KillScore, ScorePhase.Craft );
				break;
			case ScorePhase.Craft:
				anyScoreStr = "クラフトスコア";
				GeneralScoreAnim ( player.CraftScore, player.Status.Score, ScorePhase.Result, false );
				break;
			case ScorePhase.Result:
				generalScoreNum = player.Status.Score;
				ScoreResult ();
				break;
			default: break;
		}

		generalScore.text = "スコア : " + ((int)generalScoreNum).ToString ();
		anyScore.text = anyScoreStr + " : " + ((int)anyScoreNum).ToString ();
	}

	/// <summary>
	/// スコアアニメーションの汎用関数
	/// </summary>
	/// <param name="targetScore">元となるスコア</param>
	/// <param name="totalScore">元スコアを加算後の合計点</param>
	/// <param name="next">次のフェーズ</param>
	/// <param name="setActive">演出後にスコアを非表示にするか</param>
	private async void GeneralScoreAnim ( int targetScore, int totalScore, ScorePhase next, bool setActive = true ) {
		var s = (float)targetScore / 60;
		anyScoreNum -= s;
		generalScoreNum += s;
		if (anyScoreNum <= 0) {
			anyScoreNum = 0;
			generalScoreNum = totalScore;
			// 演出
			scorePhase = ScorePhase.Wait;
			await UniTask.Delay ( TimeSpan.FromSeconds ( 0.5f ) );
			anyScore.gameObject.SetActive ( false );
			await UniTask.Delay ( TimeSpan.FromSeconds ( 0.5f ) );
			anyScoreNum = next switch
			{
				ScorePhase.Kill => player.KillScore,
				ScorePhase.Craft => player.CraftScore,
				_ => 0,
			};
			scorePhase = next;
			anyScore.gameObject.SetActive ( setActive );
		}
	}

	// スコアアニメーションの後処理
	private void ScoreResult () {
		var p = generalScore.rectTransform.localPosition;
		p.y += 200.0f / 30;
		if (p.y > 0.0f) {
			p.y = 0.0f;
			ChangePhase ( Phase.Result );
		}
		generalScore.rectTransform.localPosition = p;
	}
}
