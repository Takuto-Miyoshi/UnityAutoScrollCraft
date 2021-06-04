using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AutoScrollCraft.Actors.AI {
	public class Bandit : NPCBase {
		[SerializeField] private SearchObject searchArea;
		[SerializeField] private SearchObject attackArea;
		protected override async void Update () {
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

				CanBeAction = false;
				await UniTask.Delay ( TimeSpan.FromSeconds ( UpdateInterval ) );
				CanBeAction = true;
			}
		}

		protected override void OnCollisionEnter ( Collision collision ) {
			base.OnCollisionEnter ( collision );
			var o = collision.gameObject;
			if (o.tag == "Player") {
				o.GetComponent<Player> ().TakeDamageProc ( Status.AttackPower );
			}
		}
	}
}
