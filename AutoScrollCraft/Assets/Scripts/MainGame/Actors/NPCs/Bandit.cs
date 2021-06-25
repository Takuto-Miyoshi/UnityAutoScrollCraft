using UnityEngine;

namespace AutoScrollCraft.Actors.AI {
	public class Bandit : NPCBase {
		[SerializeField] private SearchObject searchArea;
		[SerializeField] private SearchObject attackArea;
		protected void Update () {
			if (CanBeAction == true) {
				if (attackArea.Detected == true) {
					AI.Rush ( this, attackArea.Target.transform.position );
				}
				else if (searchArea.Detected == true) {
					AI.Chase ( this, searchArea.Target.transform.position );
				}
				else {
					AI.Wandering ( this );
				}

				AfterActionDelay ();
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
