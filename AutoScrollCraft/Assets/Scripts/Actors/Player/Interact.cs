using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

// インタラクトの判定をColliderでやる場合に必要になる
// 未使用

namespace AutoScrollCraft.Actors {
	public class Interact : MonoBehaviour {
		[SerializeField] GameObject target;
		public GameObject Target {
			get { return target; }
		}

		// Start is called before the first frame update
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		void OnTriggerEnter ( Collider collider ) {
			if (collider.tag == "Object") {
				target = collider.gameObject;
			}
		}

		void OnTriggerExit ( Collider collider ) {
			if (target == collider.gameObject) {
				target = null;
			}
		}
	}
}
