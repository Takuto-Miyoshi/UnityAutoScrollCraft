using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AutoScrollCraft.UI {
	public class KeyInfoSpriteChanger : MonoBehaviour {
		[SerializeField] private PlayerInput playerInput;
		private Image image;
		[SerializeField] private Sprite keyBoardTex;
		[SerializeField] private Sprite gamepadTex;
		[SerializeField] private bool gamepadTexSmaller;
		private float defaultWidth;
		private const int WidthSmallerScale = 3;

		private const string keyboard = "Keyboard";
		private const string gamepad = "Gamepad";
		private string beforeScheme = keyboard;

		private void Awake () {
			image = GetComponent<Image> ();
			defaultWidth = image.rectTransform.sizeDelta.x;
		}

		private void Update () {
			var s = playerInput.currentControlScheme;
			if (s != beforeScheme) {
				image.sprite = s switch
				{
					gamepad => gamepadTex,
					keyboard => keyBoardTex,
					_ => keyBoardTex
				};

				if (gamepadTexSmaller == true) {
					var size = image.rectTransform.sizeDelta;
					size.x = (s == gamepad) ? defaultWidth / WidthSmallerScale : defaultWidth;
					image.rectTransform.sizeDelta = size;
				}

				beforeScheme = s;
			}
		}
	}
}
