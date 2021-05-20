using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoScrollCraft.Actors.AI {
	public class Cow : NPCBase {
		[SerializeField] SearchObject attackArea;
		bool attacking;
		protected override void Update () {
			base.Update ();

			if (CanBeAction == true) {
				if (attacking == true && attackArea.Detected == true) {
					AI.Rush ( this, attackArea.Target.transform.position );
				}
				else {
					AI.Wandering ( this );
				}
			}

			IsDead ();
		}

		public override void TakeDamageProc () {
			var d = DamageSource - transform.position;
			transform.rotation = Quaternion.LookRotation ( d, Vector3.up );
			if (attacking == false) UpdateTimer = 1.0f;
			attacking = true;
		}

		protected override void OnCollisionEnter ( Collision collision ) {
			base.OnCollisionEnter ( collision );
			var o = collision.gameObject;
			if (o.tag == "Player" && attacking == true) {
				o.GetComponent<Status> ().Hp -= Status.AttackPower;
				attacking = false;
			}
		}
	}
}
