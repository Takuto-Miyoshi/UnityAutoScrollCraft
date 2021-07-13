using System;
using AutoScrollCraft.Actors;
using Cysharp.Threading.Tasks;
using ExtentionMethod;
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
	private const float AwaitDeray = 0.5f;
	private const float AnimationSpeed = 3.0f;
	// ---------- ウィンドウ -------------
	private const float WindowMoveAmountY = 550.0f;
	private Vector2 windowSizeGoal = new Vector2 ( 1700.0f, 900.0f );

	// ---------- ゲームオーバーテキスト -------------
	private const float GameOverGoalY = 300.0f;

	// ---------- スコア -------------
	private const float GeneralScoreMoveAmountY = 200.0f;
	private const float GeneralScoreGoalY = 0.0f;

	private void Awake () {
		backGround.localPosition = new Vector3 ( 0, -550, 0 );
		backGround.sizeDelta = Vector2.zero;
		gameOver.localPosition = Vector3.zero;
		gameOver.gameObject.SetActive ( false );
		generalScore.rectTransform.localPosition = new Vector3 ( 0, -200, 0 );
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
		await UniTask.Delay ( TimeSpan.FromSeconds ( AwaitDeray ) );
		phase = next;
		menu.SetActive ( phase == Phase.Result );
	}

	// ゲームオーバーウィンドウを開く
	private void OpenBackGround () {
		// 60回で目的の位置まで移動させる
		var backGroundPos = backGround.localPosition;
		backGroundPos.y += WindowMoveAmountY / 60;
		var backGroundSize = backGround.sizeDelta;
		backGroundSize.x += windowSizeGoal.x / 60;
		backGroundSize.y += windowSizeGoal.y / 60;
		// 目的の位置まで来たら位置を整える
		if (backGroundSize.x > windowSizeGoal.x) {
			backGroundPos.y = 0.0f;
			backGroundSize = windowSizeGoal;
			gameOver.gameObject.SetActive ( true );
			ChangePhase ( Phase.ShowGameOver );
		}
		backGround.localPosition = backGroundPos;
		backGround.sizeDelta = backGroundSize;
	}

	// ゲームオーバーテキストを表示する
	private void ShowGameOver () {
		var gameOverPos = gameOver.localPosition;
		// 30回で目的の位置へ移動させる
		gameOverPos.y += GameOverGoalY / 30;
		if (gameOverPos.y > GameOverGoalY) {
			gameOverPos.y = GameOverGoalY;
			anyScoreNum = player.DistanceScore;
			generalScore.gameObject.SetActive ( true );
			anyScore.gameObject.SetActive ( true );
			ChangePhase ( Phase.ScoreAnimation );
		}
		gameOver.localPosition = gameOverPos;
	}

	// Sin波でゆらゆらする
	private void GameOverAnimation () {
		var value = Mathf.Sin ( Time.time * AnimationSpeed );
		gameOver.localPosition = gameOver.localPosition.AddY ( value );
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
		var scorePerFrame = (float)targetScore / 60;
		anyScoreNum -= scorePerFrame;
		generalScoreNum += scorePerFrame;
		if (anyScoreNum <= 0) {
			anyScoreNum = 0;
			generalScoreNum = totalScore;
			// 演出
			scorePhase = ScorePhase.Wait;
			await UniTask.Delay ( TimeSpan.FromSeconds ( AwaitDeray ) );
			anyScore.gameObject.SetActive ( false );
			await UniTask.Delay ( TimeSpan.FromSeconds ( AwaitDeray ) );
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
		var scorePos = generalScore.rectTransform.localPosition;
		// 30回で移動させる
		scorePos.y += GeneralScoreMoveAmountY / 30;
		if (scorePos.y > GeneralScoreGoalY) {
			scorePos.y = GeneralScoreGoalY;
			ChangePhase ( Phase.Result );
		}
		generalScore.rectTransform.localPosition = scorePos;
	}
}
