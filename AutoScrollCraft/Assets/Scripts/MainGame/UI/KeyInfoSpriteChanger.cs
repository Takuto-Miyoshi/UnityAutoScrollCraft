using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class KeyInfoSpriteChanger : MonoBehaviour {
		[SerializeField] private PlayerInput playerInput;
		private Image image;
		[SerializeField] private Sprite[] keyboardTex;
		[SerializeField] private Sprite[] gamepadTex;
		[SerializeField] private bool gamepadTexSmaller;
		private float defaultWidth;
		private const int WidthSmallerScale = 3;

		private const string keyboard = "Keyboard";
		private const string gamepad = "Gamepad";
		private string usingScheme = keyboard;
		private bool wait = false;  // await管理用
		private int currentSelect = 0;
		private const float Interval = 0.5f;

		private void Awake () {
			image = GetComponent<Image> ();
			defaultWidth = image.rectTransform.sizeDelta.x;
		}

		private void Update () {
			var currentScheme = playerInput.currentControlScheme;
			if (currentScheme != usingScheme) {
				usingScheme = currentScheme;
				ResetAnimationValue ();

				if (gamepadTexSmaller == true) {
					var size = image.rectTransform.sizeDelta;
					size.x = (usingScheme == gamepad) ? defaultWidth / WidthSmallerScale : defaultWidth;
					image.rectTransform.sizeDelta = size;
				}
			}

			Animation ();
		}

		private async void Animation () {
			if (wait == true) return;

			wait = true;

			// 現在のスキームに応じてテクスチャを読み込み
			var spriteList = (usingScheme == gamepad) ? gamepadTex : keyboardTex;
			image.sprite = spriteList[currentSelect];
			currentSelect = UIFunctions.RevisionValue ( currentSelect + 1, spriteList.Length - 1, UIFunctions.RevisionMode.Loop );
			await UniTask.Delay ( System.TimeSpan.FromSeconds ( Interval ) );

			wait = false;
		}

		/// <summary>
		/// アニメーションの状態をリセットする
		/// </summary>
		private void ResetAnimationValue () {
			wait = false;
			currentSelect = 0;
		}
	}
}
