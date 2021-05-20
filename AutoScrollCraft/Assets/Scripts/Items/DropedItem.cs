using System.Collections;
using System.Collections.Generic;
using AutoScrollCraft;
using UnityEngine;

namespace AutoScrollCraft.Items {
	public class DropedItem : MonoBehaviour {
		[SerializeField] Enums.Items item;
		public Enums.Items Item {
			get { return item; }
			set { item = value; }
		}

		// Update is called once per frame
		void Update () {
			if (transform.position.y < -50) {
				Destroy ( gameObject );
			}
		}
	}
}
