using AutoScrollCraft.Option;
using UnityEngine;

namespace AutoScrollCraft.UI {
	public class KeyInfoSwitcher : MonoBehaviour {
		private void Awake () {
			var s = PlayerPrefs.GetString ( CheckBox.Type.KeyInfo.ToString () );
			gameObject.SetActive ( UIFunctions.StringToBool ( s ) );
		}
	}
}
