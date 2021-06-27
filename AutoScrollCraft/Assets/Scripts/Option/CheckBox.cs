using AutoScrollCraft.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AutoScrollCraft.Option {
	public class CheckBox : OptionBase {
		public enum Type {
			KeyInfo,
		};
		[SerializeField] private Type type;
		private bool current;
		[SerializeField] private Image check;
		[SerializeField] private bool defaultValue;

		private void Start () {
			// セーブデータから読み込む
			var s = PlayerPrefs.GetString ( type.ToString (), defaultValue.ToString () );
			current = UIFunctions.StringToBool ( s );
			check.gameObject.SetActive ( current );
		}

		public override void CallAction () {
			base.CallAction ();
			current = !current;
			check.gameObject.SetActive ( current );
			PlayerPrefs.SetString ( type.ToString (), current.ToString () );
		}
	}
}
