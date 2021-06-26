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
		private string currentScheme = keyboard;
		private bool wait = false;
		private int currentSelect = 0;
		private const float Interval = 0.5f;

		private void Awake () {
			image = GetComponent<Image> ();
			defaultWidth = image.rectTransform.sizeDelta.x;
		}

		private void Update () {
			var s = playerInput.currentControlScheme;
			if (s != currentScheme) {
				currentScheme = s;
				ResetAnimationValue ();

				if (gamepadTexSmaller == true) {
					var size = image.rectTransform.sizeDelta;
					size.x = (currentScheme == gamepad) ? defaultWidth / WidthSmallerScale : defaultWidth;
					image.rectTransform.sizeDelta = size;
				}
			}

			Animation ();
		}

		private async void Animation () {
			if (wait == true) return;

			wait = true;
			var spriteList = (currentScheme == gamepad) ? gamepadTex : keyboardTex;
			image.sprite = spriteList[currentSelect];
			currentSelect = UIFunctions.RevisionValue ( currentSelect + 1, spriteList.Length - 1, UIFunctions.RevisionMode.Loop );
			await UniTask.Delay ( System.TimeSpan.FromSeconds ( Interval ) );
			wait = false;
		}

		private void ResetAnimationValue () {
			wait = false;
			currentSelect = 0;
		}
	}
}
