using UnityEngine;
using System.Collections;

public class bulletController : MonoBehaviour {
	
	void Update () {
		
		// 一定距離以上を越えた弾丸は破棄.
		if (transform.position.z > 130.0) {
			
			Destroy(gameObject);
			
		}
		
		if ( rigidbody.velocity.z < 70 ) {
		
			Destroy(gameObject);
			
		}
			
	}

}
