using UnityEngine;
using System.Collections;

public class eBulletController : MonoBehaviour {
	
	// 爆発用のprefabを格納するための変数宣言.
	public GameObject bbSimpleDetonator;
	
	// Use this for initialization
	void Start () {
		
		// 爆発プレハブをロード.
		bbSimpleDetonator = (GameObject) Resources.Load("MyDetonatorTiny");
	
	}
	
	// Update is called once per frame
	void Update () {
		
		// playerを通過した弾丸は破棄.
		if (transform.position.z < -5.0) {
			
			Destroy(gameObject);
			
		}
		
		if ( rigidbody.velocity.z > -35 ) {
		
			Destroy(gameObject);
			
		}
		
	}
	
	void OnTriggerEnter (Collider other) {
		
		// playerに当たった弾丸は破棄する.
		if ( other.gameObject.tag == "Player" ) {
			
			Vector3 struckPosition = transform.position + new Vector3(0, 0, 2.3f);
			Instantiate( bbSimpleDetonator, struckPosition, Quaternion.identity );
			
			// 自分自身を破棄する.
			Destroy(gameObject);
	
		} else if ( other.gameObject.tag == "Bullet" ) {
			
			// 自分自身を破棄する.
			Destroy(gameObject);
			// playerのbulletも破棄する.
			Destroy(other.gameObject);
			
			// 爆発プレハブをインスタンス化.
			Instantiate( bbSimpleDetonator, transform.position, Quaternion.identity );
			
		} else if ( other.gameObject.tag == "Sbullet" ) {
			
			// 自分自身を破棄する.
			Destroy(gameObject);
			
		}

	}

}
