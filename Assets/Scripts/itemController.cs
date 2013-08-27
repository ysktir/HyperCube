using UnityEngine;
using System.Collections;

public class itemController: MonoBehaviour {
	
	// 移動速度.
	public float movingSpeed = 15.0f;
	private float rotationSpeed = 17.0f;

	
	void Update () {
		
		transform.Translate(Vector3.back * movingSpeed * Time.deltaTime );
		
		// z軸を基準に回転させる.
		transform.rotation = Quaternion.AngleAxis( rotationSpeed, new Vector3( 0, 0, 1) );
		rotationSpeed += 2.0f;
		
		// playerを通過したアイテムは破棄.
		if (transform.position.z < -10.0) {
		
			Destroy(gameObject);
		
		}
	
	}
	
}
