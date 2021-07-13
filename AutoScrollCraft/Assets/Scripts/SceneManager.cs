using UnityEngine;

namespace AutoScrollCraft.Scene {
	public enum SceneList {
		Title,
		MainGame,
	}

	public class SceneManager : MonoBehaviour {
		public static void LoadScene ( SceneList next ) {
			UnityEngine.SceneManagement.SceneManager.LoadScene ( next.ToString () );
		}
	}
}
