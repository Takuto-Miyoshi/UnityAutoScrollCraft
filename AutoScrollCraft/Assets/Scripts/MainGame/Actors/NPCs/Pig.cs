using UnityEngine;

namespace AutoScrollCraft.Actors.AI {
	public class Pig : NPCBase {
		protected void Update () {
			if (CanBeAction == true) {
				AI.Wandering ( this );

				AfterActionDelay ();
			}
		}

		public override bool TakeDamageProc ( int damage, Vector3 damageFrom ) {
			AI.Escape ( this );
			var b = base.TakeDamageProc ( damage, damageFrom );
			return b;
		}
	}
}
