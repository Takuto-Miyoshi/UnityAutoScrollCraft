using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
	[SerializeField] float speed;
	Vector2 axis;
	Vector3 latestPos;
	NavMeshAgent nav;

	// Start is called before the first frame update
	void Start () {
		latestPos = transform.position;
		nav = GetComponent<NavMeshAgent> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		// 移動
		if (axis != Vector2.zero) {
			var x = axis.x * speed;
			var z = axis.y * speed;
			nav.Move ( new Vector3 ( x, 0, z ) );
		}

		// 移動している方向を向く
		var diff = transform.position - latestPos;
		latestPos = transform.position;
		if (diff.magnitude > 0.01f) {
			var rot = Quaternion.LookRotation ( diff );
			rot.x = 0;
			rot.z = 0;
			transform.rotation = rot;
		}
	}

	// 入力値の更新
	public void Move ( InputAction.CallbackContext context ) {
		axis = context.ReadValue<Vector2> ();
	}
}
