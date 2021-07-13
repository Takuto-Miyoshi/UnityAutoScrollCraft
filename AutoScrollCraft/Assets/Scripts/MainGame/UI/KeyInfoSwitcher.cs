using AutoScrollCraft.Option;
using UnityEngine;

namespace AutoScrollCraft.UI {
	public class KeyInfoSwitcher : MonoBehaviour {
		private void Awake () {
			// キーTipsを表示するかをセーブデータより設定する
			var boolStr = PlayerPrefs.GetString ( CheckBox.Kind.KeyInfo.ToString () );
			gameObject.SetActive ( UIFunctions.StringToBool ( boolStr ) );
		}
	}
}
