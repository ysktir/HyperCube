
using UnityEngine;
using System.Collections;

public class textureControllerTop: MonoBehaviour {

	public float scrollSpeed = 8.0f;
	public bool stopScroll = false;
	
	
	void Update () {

		float offset = Time.deltaTime * -scrollSpeed;
		renderer.material.mainTextureOffset = new Vector2(0, renderer.material.mainTextureOffset.y - offset);
		
		if ( stopScroll == true ) {
			
			if ( scrollSpeed >= 0.1f ) { 
				scrollSpeed -= 0.03f;
			}
			
		}
		
	}
}
