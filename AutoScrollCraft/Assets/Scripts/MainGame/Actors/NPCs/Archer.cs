using UnityEngine;

namespace AutoScrollCraft.Actors.AI {
	public class Archer : NPCBase {
		[SerializeField] private SearchObject searchArea;
		[SerializeField] private GameObject arrow;
		protected void Update () {
			if (CanBeAction == true) {
				if (searchArea.Detected == true) {
					AI.ShootProjectile ( this, searchArea.Target.transform.position, arrow, 0.3f );
				}
				else {
					AI.Wandering ( this );
				}

				AfterActionDelay ();
			}
		}
	}
}
