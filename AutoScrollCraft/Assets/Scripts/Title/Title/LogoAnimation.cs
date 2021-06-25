using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LogoAnimation : MonoBehaviour {
	[SerializeField] private RectTransform[] logoList;
	private int target = 0;
	private bool upperFlag = false;
	private bool wait = false;

	private async void Update () {
		if (wait == true) return;

		// 文字を跳ねさせる
		var p = logoList[target].localPosition;
		p.y = Mathf.Abs ( Mathf.Cos ( Time.time * 6.0f ) ) * 30;
		if (p.y > 15.0f) upperFlag = true;
		logoList[target].localPosition = p;
		// 上に移動したかつ下に来たら次へ
		if ((int)p.y <= 1 && upperFlag == true) {
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
