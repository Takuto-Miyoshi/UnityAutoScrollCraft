using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class UIFunctions : MonoBehaviour {
	// Start is called before the first frame update
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public enum RevisionMode {
		Limit,  // 値の限界で止まる
		Loop,   // 値の限界で最大値または0に戻る
	};

	static public int RevisionValue ( int value, int maxValue, RevisionMode mode = RevisionMode.Limit ) {
		var r = value.CompareTo ( maxValue );
		switch (r) {
			case -1:
				if (value < 0) {
					switch (mode) {
						case RevisionMode.Limit: return 0;
						case RevisionMode.Loop: return maxValue;
					}
				}
				return value;
			case 0: return value;
			case 1:
				return mode switch
				{
					RevisionMode.Limit => maxValue,
					RevisionMode.Loop => 0,
					_ => 0,
				};
			default: return 0;
		}
	}
}
