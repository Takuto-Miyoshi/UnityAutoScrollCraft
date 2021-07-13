using System;
using Cysharp.Threading.Tasks;
using ExtentionMethod;
using UnityEngine;

public class LogoAnimation : MonoBehaviour {
	[SerializeField] private RectTransform[] logoList;
	private const float AnimationSpeed = 6.0f;  // 動くスピード
	private const float MoveScale = 30.0f;  // 動くアニメーションの規模
	private int target = 0;
	private bool upperFlag = false;
	private bool wait = false;

	private async void Update () {
		if (wait == true) return;

		// 文字を跳ねさせる
		var value = Mathf.Abs ( Mathf.Cos ( Time.time * AnimationSpeed ) ) * MoveScale;
		logoList[target].localPosition = logoList[target].localPosition.SetY ( value );
		if (value > 15.0f) upperFlag = true;

		// 上に移動したかつ下に来たら次へ
		if (value <= 1 && upperFlag == true) {
			upperFlag = false;
			target++;
			// 最後の文字になったらリセット&ディレイ
			if (target >= logoList.Length) {
				target = 0;
				wait = true;
				await UniTask.Delay ( TimeSpan.FromSeconds ( 10.0f ) );
				wait = false;
			}
		}
	}
}
