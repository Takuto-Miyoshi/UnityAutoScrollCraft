using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component {
	private static T instance;
	public static T Instance {
		get {
			// なければ探す、それでもなければ作る
			if (instance == null) {
				instance = FindObjectOfType<T> ();

				if (instance == null) {
					var o = new GameObject ();
					o.name = typeof ( T ).Name;
					instance = o.AddComponent<T> ();
				}
			}

			return instance;
		}
	}

	public virtual void Awake () {
		if (instance == null) {
			instance = this as T;
		}
	}
}
