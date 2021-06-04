using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace AutoScrollCraft.World {
	public class FieldGenelator : MonoBehaviour {
		[SerializeField] private Terrain terrain;
		private UnityEngine.Camera mainCamera;
		[SerializeField] private GameObject[] appearFieldObjectList;
		[SerializeField] private GameObject[] appearNPCList;
		private float basePosition;
		private const float DefaultSizeZ = 20.0f;
		private const float DefaultSizeX = 300.0f;
		private const float UpdatePositionX = 200.0f; // これだけ移動したら更新する
		private const float MovementAmount = 100.0f; // 移動させる量

		private void Awake () {
			mainCamera = UnityEngine.Camera.main;
			terrain.terrainData.size = new Vector3 ( DefaultSizeX, 1, DefaultSizeZ );
			terrain.transform.position = Vector3.zero;
			// オブジェクトを配置する
			DeploymentObject ( 1, (int)DefaultSizeX, appearFieldObjectList );
			DeploymentObject ( 1, (int)DefaultSizeX, appearNPCList, 5.0f, 15.0f );
		}

		private void Start () {

		}

		private void FixedUpdate () {
			// カメラの場所に応じてベースの地形を移動させる
			if (mainCamera.transform.position.x > basePosition + UpdatePositionX) {
				// 移動させる
				var p = terrain.transform.position;
				p.x = basePosition + MovementAmount;
				terrain.transform.position = p;
				basePosition = p.x;

				// 足場のなくなったオブジェクトを消す
				var v = FindObjectsOfType<GameObject> ().ToList ()
				.Where ( o => o.tag == "Object" || o.tag == "NPC" )
				.Where ( o => o.transform.position.x < basePosition );
				foreach (var o in v) Destroy ( o );

				// 移動させた先にオブジェクトを配置する
				var b = (int)p.x + (int)UpdatePositionX;
				var e = b + (int)MovementAmount;
				DeploymentObject ( b, e, appearFieldObjectList );
				// 進行度に応じてレートを上げる
				var r = (15.0f - basePosition / 1000 < 5.1f) ? 5.1f : 15.0f - basePosition / 1000;
				DeploymentObject ( b, e, appearNPCList, 5.0f, r );
			}
		}

		/// <summary>
		/// オブジェクトを配置する
		/// </summary>
		/// <param name="begin">配置しはじめるX座標</param>
		/// <param name="end">ここまで配置する</param>
		/// <param name="appearObjectList">配置するオブジェクトリスト</param>
		/// <param name="deploySpaceMin">配置間隔の最小値</param>
		/// <param name="deploySpaceMax">配置間隔の最大値</param>
		private void DeploymentObject ( int begin, int end, GameObject[] appearObjectList, float deploySpaceMin = 1.0f, float deploySpaceMax = 4.0f ) {
			for (float x = begin; x < end; x += Random.Range ( deploySpaceMin, deploySpaceMax )) {
				var z = Random.Range ( 1, DefaultSizeZ - 1 );
				var p = new Vector3 ( x, 0.5f, z );
				var o = appearObjectList[Random.Range ( 0, appearObjectList.Length )];
				Instantiate ( o, p, Quaternion.identity );
			}
		}
	}
}
