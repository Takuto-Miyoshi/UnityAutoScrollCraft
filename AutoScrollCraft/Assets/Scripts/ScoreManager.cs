using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager> {
	private const string Ranking = "Rank";  // 保存用の名前
	private const int RankingMax = 10; // ランキングの上限

	/// <summary>
	/// ランキングにスコアを登録し更新する
	/// </summary>
	/// <param name="score">登録するスコア</param>
	public void UpdateRanking ( int score ) {
		List<int> scoreList = GetRanking ().ToList ();

		// 今回のスコアを追加して並べ替え
		scoreList.Add ( score );
		scoreList.Sort ();
		scoreList.Reverse ();

		// 10位以内のスコアを保存する
		for (int i = 0; i < RankingMax; i++) {
			PlayerPrefs.SetInt ( Ranking + (i + 1).ToString (), scoreList[i] );
		}
	}

	/// <summary>
	/// ランキングのスコアを取得する
	/// </summary>
	/// <returns>取得したスコアの配列</returns>
	public int[] GetRanking () {
		List<int> scoreList = new List<int> ();
		for (int i = 0; i < RankingMax; i++) {
			scoreList.Add ( PlayerPrefs.GetInt ( Ranking + (i + 1).ToString (), 0 ) );
		}

		return scoreList.ToArray ();
	}
}
