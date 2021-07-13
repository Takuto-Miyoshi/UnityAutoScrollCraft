using AutoScrollCraft.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AutoScrollCraft.Option {
	public class CheckBox : OptionBase {
		// オプションの種類
		public enum Kind {
			KeyInfo,
		};
		[SerializeField] private Kind optionKind;
		private bool current;
		[SerializeField] private Image check;
		[SerializeField] private bool defaultValue;

		private void Start () {
			// セーブデータから読み込む
			var s = PlayerPrefs.GetString ( optionKind.ToString (), defaultValue.ToString () );
			current = UIFunctions.StringToBool ( s );
			check.gameObject.SetActive ( current );
		}

		public override void CallAction () {
			base.CallAction ();

			current = !current;
			check.gameObject.SetActive ( current );
			PlayerPrefs.SetString ( optionKind.ToString (), current.ToString () );
		}
	}
}
