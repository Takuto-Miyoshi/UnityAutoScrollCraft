using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class UIFunctions : MonoBehaviour {
		public enum RevisionMode {
			Limit,  // 値の限界で止まる
			Loop,   // 値の限界で値の位置を反転させる
		};

		private const int BiggerLeft = 1;
		private const int BiggerRight = -1;
		private const int Equal = 0;

		private const int FadeIn = 1;
		private const int FadeOut = -1;

		/// <summary>
		/// 値を補正する
		/// </summary>
		/// <param name="value">補正する値</param>
		/// <param name="maxValue">値の上限</param>
		/// <param name="mode">上限、下限を超えたときの挙動</param>
		/// <returns>補正後の値</returns>
		public static int RevisionValue ( int value, int maxValue, RevisionMode mode = RevisionMode.Limit ) {
			var r = value.CompareTo ( maxValue );
			// valueとmaxValueの比較結果と値をもとに補正
			switch (r) {
				case BiggerRight:
					if (value < 0) {
						switch (mode) {
							case RevisionMode.Limit: return 0;
							case RevisionMode.Loop:
								var t = value;
								// 一回の補正で自然数にならないかもしれないので
								while (t + maxValue + 1 <= maxValue) {
									t += maxValue + 1;
								}
								return t;
						}
					}
					return value;
				case Equal: return value;
				case BiggerLeft:
					switch (mode) {
						case RevisionMode.Limit: return maxValue;
						case RevisionMode.Loop:
							var t = value;
							while (t - (maxValue + 1) >= 0) {
								t -= maxValue + 1;
							}
							return t;
						default: return 0;
					}
				default: return 0;
			}
		}

		/// <summary>
		/// フェードイン→フェードアウトさせる
		/// </summary>
		/// <param name="target">フェードさせる対象</param>
		/// <param name="endToDeactivate">フェード完了後にDeActiveに設定するか</param>
		/// <param name="waitTime">フェードイン後の待ち時間</param>
		public static async void FloatingShow ( Image target, bool endToDeactivate, bool onlyFadeIn = false, float waitTime = 3.0f ) {
			var m = FadeIn;
			var c = target.color;
			c.a = 0.0f;
			target.color = c;
			// フェード
			while (true) {
				if (target == null) return;

				c = target.color;
				c.a += Time.deltaTime * m;
				target.color = c;
				// モード切り替え、終了条件
				if ((m == FadeIn && c.a > 1.0f) || (m == FadeOut && c.a < 0.0f)) {
					if (m == FadeOut || onlyFadeIn == true) {
						target.gameObject.SetActive ( endToDeactivate );
						return;
					}
					m = FadeOut;
					await UniTask.Delay ( System.TimeSpan.FromSeconds ( waitTime ) );
				}
				await UniTask.NextFrame ();
			}
		}

		// "true"ならtrue、それ以外はfalse
		public static bool StringToBool ( string str ) {
			return str == true.ToString ();
		}
	}
}