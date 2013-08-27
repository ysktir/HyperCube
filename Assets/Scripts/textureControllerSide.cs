
using UnityEngine;
using System.Collections;

public class textureControllerSide : MonoBehaviour {

	public float scrollSpeed = 2.0f;
	public bool stopScroll = false;
	
	
	void Update () {

		float offset = Time.deltaTime * -scrollSpeed;
		renderer.material.mainTextureOffset = new Vector2(renderer.material.mainTextureOffset.x - offset, 0);
		
		if ( stopScroll == true ) {
			
			if ( scrollSpeed >= 0.1f ) { 
				scrollSpeed -= 0.03f;
			}
			
		}

	}
}

