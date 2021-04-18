using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Camera : MonoBehaviour {
	[SerializeField] float speed;

	// Start is called before the first frame update
	void Start () {

	}

	void FixedUpdate () {
		// 右へスクロール
		var x = speed;
		transform.Translate ( x, 0, 0 );
	}
}
