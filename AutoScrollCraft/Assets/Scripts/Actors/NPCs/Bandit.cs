using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoScrollCraft.Actors.AI {
	public class Bandit : NPCBase {
		[SerializeField] SearchObject searchArea;
		[SerializeField] SearchObject attackArea;
		protected override void Update () {
			base.Update ();

			if (CanBeAction == true) {
				if (searchArea.Detected == true || attackArea.Detected == true) {
					UpdateInterval /= 2;

					if (attackArea.Detected == true) {
						AI.Rush ( this, attackArea.Target.transform.position );
					}
					else {
						AI.Chase ( this, searchArea.Target.transform.position );
					}

					UpdateInterval *= 2;
				}
				else {
					AI.Wandering ( this );
				}
			}

			IsDead ();
		}

		protected override void OnCollisionEnter ( Collision collision ) {
			base.OnCollisionEnter ( collision );
			var o = collision.gameObject;
			if (o.tag == "Player") {
				o.GetComponent<Status> ().Hp -= Status.AttackPower;
			}
		}
	}
}
