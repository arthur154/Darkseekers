//using UnityEngine;
//using System.Collections;
//
//public class GenerateBuilding : MonoBehaviour {
//	
//	public static float minHeight = 15.0f;
//	public static float maxHeight = 70.0f;
//	
//	public enum buildingType {
//		bottomLeft,
//		bottom,
//		bottomRight,
//		left,
//		middle,
//		right,
//		topLeft,
//		top,
//		topRight
//	}
//	
//	public static GameObject instantiateBuilding(GameObject parent, Vector3 startingPosition, float x_scale, float z_scale, Texture2D texture) {
//		float height = Random.Range(minHeight, maxHeight);
//		
//		GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);
//		
//		startingPosition.x += (x_scale/2);
//		startingPosition.y += (height/2);
//		startingPosition.z += (z_scale/2);
//		building.transform.position = startingPosition;
//		Vector3 scale = new Vector3(x_scale, height, z_scale);
//		building.transform.localScale = scale;
//		
////		building.renderer.material.mainTexture = texture;
////		Vector2 buildingScale = building.renderer.material.mainTextureScale;
////		buildingScale = new Vector2(building.transform.localScale.x, building.transform.localScale.y);
////		building.renderer.material.mainTextureScale = buildingScale*0.25f;
//		
//		building.transform.parent = parent.transform;
//		
//		return building;
//	}
//	
//}
//;