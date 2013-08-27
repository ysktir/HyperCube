using UnityEngine;
using System.Collections;

public class takeOff : MonoBehaviour {
	
	// 移動速度.
	private float movingSpeed;
	private title titleScript;
	
	
	void Start () {
		
		movingSpeed = 0.0f;
		
	}
	
	
	void Update () {
		
		titleScript = GameObject.Find("Camera").GetComponent<title>();
		
		if ( titleScript.takeOff == true ) {
			
			movingSpeed += 0.9f;
			transform.Translate(Vector3.up * movingSpeed * Time.deltaTime);
			
		}
		
	}
}
