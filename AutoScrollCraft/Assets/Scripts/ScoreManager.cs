using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager> {
	private const string Ranking = "Rank";

	/// <summary>
	/// ランキングにスコアを登録し更新する
	/// </summary>
	/// <param name="score">登録するスコア</param>
	public void UpdateRanking ( int score ) {
		List<int> scoreList = new List<int> ();
		// スコアを取得
		for (int i = 1; i < 11; i++) {
			scoreList.Add ( PlayerPrefs.GetInt ( Ranking + i.ToString () ) );
		}
		// 並べ替え
		scoreList.Add ( score );
		scoreList.Sort ();
		scoreList.Reverse ();

		// 10位以内のスコアを保存する
		for (int i = 1; i < 11; i++) {
			PlayerPrefs.SetInt ( Ranking + i.ToString (), scoreList[i - 1] );
		}
	}

	/// <summary>
	/// ランキングのスコアを取得する
	/// </summary>
	/// <returns>取得したスコアの配列</returns>
	public int[] GetRanking () {
		List<int> scoreList = new List<int> ();
		for (int i = 1; i < 11; i++) {
			scoreList.Add ( PlayerPrefs.GetInt ( Ranking + i.ToString () ) );
		}
		return scoreList.ToArray ();
	}
}
