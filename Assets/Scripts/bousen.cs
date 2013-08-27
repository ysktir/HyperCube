using UnityEngine;
using System.Collections;

public class bousen : MonoBehaviour {
	
	private bool retryOrNot = true;
	
	void Update () {
		
		if ( Input.GetAxis("Horizontal") < 0 ) {
			
			retryOrNot = true;
			
		} else if ( Input.GetAxis("Horizontal") > 0 ) {
			
			retryOrNot = false;
			
		}
		
		
		if ( retryOrNot && transform.position.x > 0.07 ) {
			
			// transform.position = transform.position - new Vector3(0.02f, 0, 0);
			
		} else if ( retryOrNot == false && transform.position.x < 0.29 ) {
			
			// transform.position = transform.position + new Vector3(0.02f, 0, 0);
			
		}
			
	}
}
