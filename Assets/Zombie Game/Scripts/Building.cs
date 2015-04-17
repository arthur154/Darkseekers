using UnityEngine;
using System.Collections;

public enum buildingType {
	bottomLeft,
	bottom,
	bottomRight,
	left,
	middle,
	right,
	topLeft,
	top,
	topRight
}

public enum wallType {
	left,
	bottom,
	right,
	top
}

public class Building : MonoBehaviour {
	
	public City city;
	public Vector3 origin;
	public Vector2 dimensions;
	public buildingType type;
	public Texture2D texture;
//	public GameObject window;
		
//	public static void newBuilding(City c, Building b, float buildingScale, Vector3 orig, Vector2 dim, buildingType typ, Texture2D tile, GameObject wind) {
//		b.city = c;
//		b.origin = new Vector3(orig.x + (((1-buildingScale)*dim.x)/2), orig.y, orig.z + (((1-buildingScale)*dim.x)/2));
//		b.dimensions = new Vector2(dim.x*buildingScale, dim.y);
//		b.type = typ;
//		b.texture = tile;
//		b.window = wind;
//		b.initialize();
//	}

	public static void newBuilding(City c, Building b, float buildingScale, Vector3 orig, Vector2 dim, buildingType typ, Texture2D tile) {
		b.city = c;
		b.origin = new Vector3(orig.x + (((1-buildingScale)*dim.x)/2), orig.y, orig.z + (((1-buildingScale)*dim.x)/2));
		b.dimensions = new Vector2(dim.x*buildingScale, dim.y);
		b.type = typ;
		b.texture = tile;
		b.initialize();
	}
	
	void initialize() {
		
		createWall(wallType.left);
		createWall(wallType.bottom);
		createWall(wallType.right);
		createWall(wallType.top);
		
		GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Plane);
		roof.name = "roof";
		roof.transform.localScale = new Vector3(this.dimensions.x*0.1f, 0f, this.dimensions.x*0.1f);
		roof.transform.Translate(new Vector3(this.origin.x + (0.5f * this.dimensions.x), this.origin.y + (1f * this.dimensions.y), this.origin.z + (0.5f * this.dimensions.x)));
		roof.transform.parent = this.transform;
		roof.layer = LayerMask.NameToLayer("Obstacles");
	}
	
	void createWall(wallType w) {
		
		GameObject container = new GameObject();
		GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Plane);
		wall.transform.localScale = new Vector3(this.dimensions.x*0.1f, 0f, this.dimensions.y*0.1f);
		wall.transform.parent = container.transform;
		addLedge(container);
//		addWindows(container);
		
		if (w == wallType.left) {
			
//			if (this.type == buildingType.bottomLeft ||
//				this.type == buildingType.left ||
//				this.type == buildingType.topLeft ||
//				this.type == buildingType.right) {
//				addWindows(container);
//			}
			
			container.name = "LeftContainer";
			wall.name = "LeftWall";
			
			container.transform.Translate(new Vector3(this.origin.x, this.origin.y + (0.5f * this.dimensions.y), this.origin.z + (0.5f * this.dimensions.x)));
			container.transform.Rotate(Vector3.up, 90f);
			container.transform.Rotate(Vector3.right, -90f);	
		} else if (w == wallType.bottom) {
			
//			if (this.type == buildingType.bottomLeft ||
//				this.type == buildingType.bottom ||
//				this.type == buildingType.bottomRight ||
//				this.type == buildingType.top) {
//				addWindows(container);
//			}
			
			container.name = "BottomContainer";
			wall.name = "BottomWall";
			
			container.transform.Translate(new Vector3(this.origin.x + (0.5f * this.dimensions.x), this.origin.y + (0.5f * this.dimensions.y), this.origin.z));
			container.transform.Rotate(Vector3.right, -90f);
		} else if (w == wallType.right) {
			
//			if (this.type == buildingType.topRight ||
//				this.type == buildingType.right ||
//				this.type == buildingType.bottomRight ||
//				this.type == buildingType.left) {
//				addWindows(container);
//			}
			
			container.name = "RightContainer";
			wall.name = "RightWall";
			
			container.transform.Translate(new Vector3(this.origin.x + this.dimensions.x, this.origin.y + (0.5f * this.dimensions.y), this.origin.z + (0.5f * this.dimensions.x)));
			container.transform.Rotate(Vector3.up, -90f);
			container.transform.Rotate(Vector3.right, -90f);
		} else if (w == wallType.top) {
			
//			if (this.type == buildingType.topLeft ||
//				this.type == buildingType.top ||
//				this.type == buildingType.topRight ||
//				this.type == buildingType.bottom) {
//				addWindows(container);
//			}
			
			container.name = "TopContainer";
			wall.name = "TopWall";
			
			container.transform.Translate(new Vector3(this.origin.x + (0.5f * this.dimensions.x), this.origin.y + (0.5f * this.dimensions.y), this.origin.z + this.dimensions.x));
			container.transform.Rotate(Vector3.up, 180f);
			container.transform.Rotate(Vector3.right, -90f);
		}
		
		wall.renderer.material.mainTexture = this.texture;
		Vector2 wallScale = new Vector2(1, dimensions.y/6.5f);
		wall.renderer.material.mainTextureScale = wallScale;
		wall.transform.parent = container.transform;
		container.transform.parent = this.transform;
	}
	
//	void addWindows(GameObject container) {
//		float windowWidth = (this.dimensions.x * 0.6f)/5f;
//		float spaceWidth = (this.dimensions.x * 0.4f)/6f;
//		Vector3 origin = new Vector3(-(this.dimensions.x/2) + (spaceWidth)+(windowWidth/2), 0.1f, -(this.dimensions.y/2) + (city.storyHeight/2));
//		Vector3 nextPos = origin;
//		int numStories = (int) (this.dimensions.y / this.city.storyHeight);
//		int i = 0;
//		int j = 0;
//		while (i < numStories) {
//			while (j < 5) {
//				GameObject window = (GameObject)Instantiate(this.window, nextPos, Quaternion.identity);
//				window.transform.localScale = new Vector3(windowWidth*0.1f, window.transform.localScale.y, window.transform.localScale.z);
//				window.transform.parent = container.transform;
//				nextPos.x += (spaceWidth)+(windowWidth);
//				j++;
//			}
//			nextPos.x = origin.x;
//			nextPos.z += city.storyHeight;
//			j = 0;
//			i++;
//		}
//	}
	
	void addLedge(GameObject container) {
		GameObject ledge = GameObject.CreatePrimitive(PrimitiveType.Cube);
		ledge.name = "ledge";
		ledge.transform.Translate(new Vector3(-0.5f, 0.5f, (this.dimensions.y/2)+(1.5f/2)));
		ledge.transform.parent = container.transform;
		ledge.transform.localScale = new Vector3(this.dimensions.x + 1f, 1f, 1.5f);
		ledge.layer = LayerMask.NameToLayer("Obstacles");
	}
}
