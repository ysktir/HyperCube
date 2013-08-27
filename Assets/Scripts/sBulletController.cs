using UnityEngine;
using System.Collections;

public class sBulletController : MonoBehaviour {
	

	void Update () {
		
		//  一定のスピードを保つ.
		if ( rigidbody.velocity.z < 70.0f ) {
			
			rigidbody.velocity = new Vector3( 0, 0, 80.0f);
		
		}
		
		// 一定距離以上を越えた弾丸は破棄.
		if ( transform.position.z > 230.0f ) {
			
			Destroy(gameObject);
			
		}
		
		
	}
}
