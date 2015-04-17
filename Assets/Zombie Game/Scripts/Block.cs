using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	
	public enum blockType {
		L,
		U,
		C,
		O
	}
	
	public enum fenceType {
		none,
		top,
		topleft,
		topright,
		right,
		left,
		bottom,
		bottomleft,
		bottomright
	}
	
	public City city;
	public Vector3 startingPosition;
	public Vector3 nextPosition;
	public blockType bType;
	public fenceType fType;
	public bool survival;
	private float fenceLength = 7.5f;
	private Vector3 smallFence = new Vector3 (0.68f, 0.68f, 0.68f);
	private float fenceHeight = 0.2f;
	
	public static Vector3 newBlock(Block b, City c, Vector3 origin, blockType bTyp, fenceType fTyp, bool surv) {
		b.city = c;
		b.startingPosition = origin;
		b.bType = bTyp;
		b.fType = fTyp;
		b.survival = surv;
		b.initialize();
		return b.nextPosition;
	}
	
	void initialize() {
		switch (bType) {
			case blockType.L:
				instantiateL();
				break;
			case blockType.U:
				instantiateU();
				break;
			case blockType.C:
				instantiateC();
				break;
			case blockType.O:
				instantiateO();
				break;
		}
	}
	
	// Instantiates an L configuration of roads, taking into account blockSize.
	// The + is placed at the starting position.
	//
	// If blockSize were 3, it would yield:
	//
	//        |
	//        |
	//        |
	//        +===
	//
	// Changes nextPosition to the right and bottom bound of the right-most tile on the BOTTOM row.
	//
	private void instantiateL() {
		
		// Initialize indexes and position vectors
		int currtileX = 0;
		int currtileY = 0;
		this.nextPosition = this.startingPosition;
		
		// Initialize temp gameobject
		GameObject temp = null;
		
		
		while (currtileY <= city.blockSize) {
			while (currtileX <= city.blockSize) {
				// If the current tile is in the first row or first column:
				if (currtileY == 0 || currtileX == 0) {
					if (currtileY == 0) {
						if (currtileX == 0) {
							// First tile: instantiate a cross-road, rotated 180 degrees because origin is at top right of
							// original object.
							temp = (GameObject) Instantiate(city.roadCross, this.nextPosition, Quaternion.AngleAxis(180, Vector3.up));
							temp.transform.parent = this.transform;

							if (this.fType == fenceType.right || this.fType == fenceType.topright) {
								GameObject fence = (GameObject) Instantiate(city.fence, new Vector3(this.nextPosition.x + (temp.transform.GetChild(0).renderer.bounds.size.x), this.nextPosition.y + this.fenceHeight, this.nextPosition.z + 16f), Quaternion.identity);
								fence.transform.localScale = new Vector3(fence.transform.localScale.x, fence.transform.localScale.y, this.fenceLength);
								fence.transform.Translate(new Vector3((fence.transform.GetChild(0).renderer.bounds.size.x), 0f, 0f));
								fence.transform.parent = temp.transform;
								fence.name = "largeRoadBlock";
							}
							
							if (this.fType == fenceType.top || this.fType == fenceType.topright) {
								GameObject fence = (GameObject) Instantiate(city.fence, new Vector3(this.nextPosition.x, this.nextPosition.y + this.fenceHeight, this.nextPosition.z), Quaternion.AngleAxis(90f, Vector3.up));
								fence.transform.localScale = new Vector3(fence.transform.localScale.x, fence.transform.localScale.y, this.fenceLength);
								fence.transform.Translate(new Vector3(-(temp.transform.GetChild(0).renderer.bounds.size.x)-(fence.transform.GetChild(0).renderer.bounds.size.z), 0f, 16.1f));
								fence.transform.parent = temp.transform;
								fence.name = "largeRoadBlock";
							}
							
						} else {
							// First row tiles: instantiate a horizontal road, rotated 180 degrees because origin is at top
							// right of original object.
							temp = (GameObject) Instantiate(city.roadStraight, this.nextPosition, Quaternion.AngleAxis(180, Vector3.up));
							temp.transform.parent = this.transform;
						}
						
						this.nextPosition.x += (temp.transform.GetChild(0).renderer.bounds.size.x);
						
						if (currtileX == city.blockSize) {
							
							if (this.fType == fenceType.left || this.fType == fenceType.topleft) {
								GameObject fence = (GameObject) Instantiate(city.fence, new Vector3(this.nextPosition.x, this.nextPosition.y + this.fenceHeight, this.nextPosition.z + 16f), Quaternion.identity);
								fence.transform.localScale = new Vector3(fence.transform.localScale.x, fence.transform.localScale.y, this.fenceLength);
								fence.transform.Translate(new Vector3(-(fence.transform.GetChild(0).renderer.bounds.size.x), 0f, 0f));
								fence.transform.parent = temp.transform;
								fence.name = "largeRoadBlock";
							}
							
							this.nextPosition.x = this.startingPosition.x;
							this.nextPosition.z += (temp.transform.GetChild(0).renderer.bounds.size.z);
						}
					} else {
						// First column tiles: First add z size of previous road to x scalar of nextPosition.
						// Then instantiate a vertical road, rotated 90 degrees because origin is at bottom
						// right of original object.
						this.nextPosition.x += (temp.transform.GetChild(0).renderer.bounds.size.z);
						temp = (GameObject) Instantiate(city.roadStraight, this.nextPosition, Quaternion.AngleAxis(90, Vector3.up));
						temp.transform.parent = this.transform;
						
						if (currtileY == city.blockSize) {
							if (this.fType == fenceType.bottom || this.fType == fenceType.bottomright) {
								GameObject fence = (GameObject) Instantiate(city.fence, new Vector3(this.nextPosition.x, this.nextPosition.y + this.fenceHeight, this.nextPosition.z), Quaternion.AngleAxis(90f, Vector3.up));
								fence.transform.localScale = new Vector3(fence.transform.localScale.x, fence.transform.localScale.y, this.fenceLength);
								fence.transform.Translate(new Vector3(-(temp.transform.GetChild(0).renderer.bounds.size.x)+(fence.transform.GetChild(0).renderer.bounds.size.z), 0f, -16.37f));
								fence.transform.parent = temp.transform;
								fence.name = "largeRoadBlock";
							}
						}
					}
				} else {
					// If the current tile is one of the "plane" tiles:
					
					// Make sure adjustedPos has the height of a road tile so building will get created at that height
					Vector3 adjustedPos = this.nextPosition;
					adjustedPos.y += (temp.transform.GetChild(0).renderer.bounds.size.y);
					
					// Make ground
					Vector3 ground_position = new Vector3(adjustedPos.x + ((temp.transform.GetChild(0).renderer.bounds.size.x)/2), adjustedPos.y, adjustedPos.z + ((temp.transform.GetChild(0).renderer.bounds.size.z)/2));
					GameObject groundContainer = (GameObject) Instantiate(city.ground, ground_position, Quaternion.identity);
					groundContainer.name = "Ground";
					groundContainer.transform.parent = this.transform;
					groundContainer.layer = LayerMask.NameToLayer("Ground");
					Vector3 fenceLoc = ground_position;
					Quaternion fenceRot = Quaternion.identity;
					if (
						(
						 (
						  (this.fType == fenceType.left && currtileX == city.blockSize) ||
						  (this.fType == fenceType.right && currtileX == 1)
						 ) && (currtileY < city.blockSize)
						) ||
					    (
						 (
						  (this.fType == fenceType.top && currtileY == 1) || 
						  (this.fType == fenceType.bottom && currtileY == city.blockSize)
						 ) && (currtileX < city.blockSize)
						)
					   ) {
						if (this.fType == fenceType.left) {
							fenceLoc += new Vector3(
									((groundContainer.transform.GetChild(0).renderer.bounds.size.x)*0.9f)/2,
									0f,
									(groundContainer.transform.GetChild(0).renderer.bounds.size.z)/2
									);
						} else if (this.fType == fenceType.right) {
							fenceLoc = ground_position - 
								new Vector3(
									((groundContainer.transform.GetChild(0).renderer.bounds.size.x)*0.9f)/2,
									0f,
									-(groundContainer.transform.GetChild(0).renderer.bounds.size.z)/2
									);
						} else {
							fenceRot = Quaternion.AngleAxis(90f, Vector3.up);
							if (this.fType == fenceType.top) {
								fenceLoc = ground_position + 
									new Vector3(
										(groundContainer.transform.GetChild(0).renderer.bounds.size.z)/2,
										0f,
										-((groundContainer.transform.GetChild(0).renderer.bounds.size.x)*0.9f)/2
										);
							} else {
								fenceLoc = ground_position + 
									new Vector3(
										(groundContainer.transform.GetChild(0).renderer.bounds.size.z)/2,
										0f,
										((groundContainer.transform.GetChild(0).renderer.bounds.size.x)*0.9f)/2
										);
							}
						}
						GameObject smallFence = (GameObject) Instantiate(city.fence, fenceLoc, fenceRot);
						smallFence.transform.localScale = this.smallFence;
						smallFence.name = "smallRoadBlock";
						smallFence.transform.parent = groundContainer.transform;
					}
//					GameObject groundContainer = new GameObject();
//					groundContainer.name = "Ground";
//					groundContainer.transform.parent = this.transform;
//					
//					GameObject groundPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
//					groundPlane.name = "GroundPlane";
//					groundPlane.layer = LayerMask.NameToLayer("Ground");
//					groundPlane.transform.localScale = new Vector3((temp.transform.GetChild(0).renderer.bounds.size.x)*0.1f, 0f, (temp.transform.GetChild(0).renderer.bounds.size.x)*0.1f);
//					groundPlane.renderer.material.mainTexture = city.asphaltTexture;
//					Vector2 groundScale = new Vector2(groundPlane.transform.localScale.x, groundPlane.transform.localScale.z);
//					groundPlane.renderer.material.mainTextureScale = groundScale;
//					groundPlane.transform.parent = groundContainer.transform;
//					
//					groundContainer.transform.position = new Vector3(adjustedPos.x + ((temp.transform.GetChild(0).renderer.bounds.size.x)/2), adjustedPos.y, adjustedPos.z + ((temp.transform.GetChild(0).renderer.bounds.size.z)/2));
					
					// All buildings will consist of a square base slightly smaller than one block with a random height.
					Vector2 dimensions = new Vector2((temp.transform.GetChild(0).renderer.bounds.size.x), city.storyHeight*Random.Range(city.minStories, city.maxStories+1));

					// This makes sure that windows and doors get drawn only on the sides facing the street and that the middle is devoid of any buildings.
					buildingType bldgType;
					if (currtileY == 1) {
						if (currtileX == 1) {
							bldgType = buildingType.bottomLeft;
						} else if (currtileX == city.blockSize) {
							bldgType = buildingType.bottomRight;
						} else {
							bldgType = buildingType.bottom;
						}
					} else if (currtileY == city.blockSize) {
						if (currtileX == 1) {
							bldgType = buildingType.topLeft;
						} else if (currtileX == city.blockSize) {
							bldgType = buildingType.topRight;
						} else {
							bldgType = buildingType.top;
						}
					} else if (currtileX == 1) {
						bldgType = buildingType.left;
					} else if (currtileX == city.blockSize) {
						bldgType = buildingType.right;
					} else {
						bldgType = buildingType.middle;
					}
					
					// Each building gets a random texture
					int rand = Random.Range(0, (city.buildingTextures.Length));
					
					// Finally create the building
					if (bldgType != buildingType.middle) {
						if (!survival) {
							GameObject building = new GameObject("Building");
							building.AddComponent<Building>();
							Building.newBuilding(city, building.GetComponent<Building>(), 0.9f, adjustedPos, dimensions, bldgType, city.buildingTextures[rand]);
							building.transform.parent = groundContainer.transform;
						}
					}
					
					
					if (currtileX != city.blockSize) {
						// If current tile is not the last tile in the row, then add straight
						// road's x scalar to nextPosition.x.
						this.nextPosition.x += (temp.transform.GetChild(0).renderer.bounds.size.x);
					} else {
						// If current tile is the last tile in the row, but not in the column,
						// Reset nextPosition's x scalar to startingPosition's x scalar, and
						// add the z size of temp to nextPosition.z.
						if (currtileY != city.blockSize) {
							this.nextPosition.x = startingPosition.x;
							this.nextPosition.z += (temp.transform.GetChild(0).renderer.bounds.size.z);
						} else {
							// If the current tile is the last in the row and the column,
							// add x size of temp to nextPosition.x and set nextPosition.z to
							// startingPosition.z;
							this.nextPosition.x += (temp.transform.GetChild(0).renderer.bounds.size.x);
							this.nextPosition.z = startingPosition.z;
						}
					}
				}
				// Increment currtileX
				currtileX++;
			}
			// Reset currtileX and increment currtileY.
			currtileX = 0;
			currtileY++;
		}
	}
	
	// Instantiates a U configuration of roads, taking into account blockSize.
	// The left + is placed at the starting position.
	//
	// If blockSize were 3, it would yield:
	//
	//        |   |
	//        |   |
	//        |   |
	//        +===+
	//
	// Changes nextPosition to the right and top bound of the right-most tile on the TOP row.
	//
	private void instantiateU() {
		
		// First instantiateL using startingPosition
		instantiateL();
		
		// Initialize temp variables
		GameObject temp = null;
		int currtile = 0;
		
		// Build up the vertical road
		while (currtile <= city.blockSize) {
			// Place a cross road at first position, then vertical roads subsequently.
			if (currtile == 0) {
				temp = (GameObject) Instantiate(city.roadCross, this.nextPosition, Quaternion.AngleAxis(180, Vector3.up));
				temp.transform.parent = this.transform;
				nextPosition.z += (temp.transform.GetChild(0).renderer.bounds.size.z);
				nextPosition.x += (temp.transform.GetChild(0).renderer.bounds.size.x);
			} else {
				temp = (GameObject) Instantiate(city.roadStraight, this.nextPosition, Quaternion.AngleAxis(90, Vector3.up));
				temp.transform.parent = this.transform;
				nextPosition.z += (temp.transform.GetChild(0).renderer.bounds.size.x);
			}
			currtile++;
		}
	}
	
	// Instantiates a C configuration of roads, taking into account blockSize.
	// The bottom + is placed at the starting position.
	//
	// If blockSize were 3, it would yield:
	//
	//        +===
	//        |   
	//        |   
	//        |   
	//        +===
	//
	// Changes nextPosition to the right and bottom bound of the right-most tile on the BOTTOM row.
	//
	private void instantiateC() {
		
		// First instantiateL
		instantiateL();
		
		// Then set finalPosition to nextPosition
		Vector3 finalPosition = this.nextPosition;
		
		// Initialize temp variables
		int currtile = 0;
		GameObject temp = null;
		
		// Instantiate cross-road to get its z bound and then destroy it.
		temp = (GameObject) Instantiate(city.roadCross, this.nextPosition, Quaternion.AngleAxis(180, Vector3.up));
		this.nextPosition.z += (temp.transform.GetChild(0).renderer.bounds.size.z);
		Destroy(temp);
		
		// Then, instantiate vertical roads to get their z bounds and destroy them.
		while (currtile < city.blockSize) {
			temp = (GameObject) Instantiate(city.roadStraight, this.nextPosition, Quaternion.AngleAxis(90, Vector3.up));
			this.nextPosition.z += (temp.transform.GetChild(0).renderer.bounds.size.x);
			Destroy(temp);
			currtile++;
		}
		
		// Add temp's z bound to nextPosition.z and reset currtile to 0.
		this.nextPosition.z += (temp.transform.GetChild(0).renderer.bounds.size.z);
		currtile = 0;
		
		// Instantiate horizontal roads until the last tile, and then instantiate a cross-road.
		while (currtile <= city.blockSize) {
			if (currtile != city.blockSize) {
				temp = (GameObject) Instantiate(city.roadStraight, this.nextPosition, Quaternion.identity);
				temp.transform.parent = this.transform;
				this.nextPosition.x -= (temp.transform.GetChild(0).renderer.bounds.size.x);
			} else {
				temp = (GameObject) Instantiate(city.roadCross, this.nextPosition, Quaternion.identity);
				temp.transform.parent = this.transform;
			}
			currtile++;
		}
		
		this.nextPosition = finalPosition;
	}
	
	
	// Instantiates an O configuration of roads, taking into account blockSize.
	// The bottom left + is placed at the starting position.
	//
	// If blockSize were 3, it would yield:
	//
	//        +===+
	//        |   |
	//        |   |
	//        |   |
	//        +===+
	//
	// Changes nextPosition to the right and top bound of the right-most tile on the TOP row.
	//
	private void instantiateO() {
		
		// First instantiateC using startingPosition
		instantiateC();
		
		// Make the cross-road at the bottom and move nextPosition to top-right corner
		GameObject temp = (GameObject) Instantiate(city.roadCross, this.nextPosition, Quaternion.AngleAxis(180, Vector3.up));
		temp.transform.parent = this.transform;
		this.nextPosition.x += (temp.transform.GetChild(0).renderer.bounds.size.x);
		this.nextPosition.z += (temp.transform.GetChild(0).renderer.bounds.size.z);
		
		// Initialize temp variable
		int currtile = 0;
		
		// Build up the road
		while (currtile <= city.blockSize) {
			
			// If it's not the last tile, lay a vertical road; otherwise lay a cross-road.
			if (currtile != city.blockSize) {
				temp = (GameObject) Instantiate(city.roadStraight, this.nextPosition, Quaternion.AngleAxis(90, Vector3.up));
				temp.transform.parent = this.transform;
				this.nextPosition.z += (temp.transform.GetChild(0).renderer.bounds.size.x);
			} else {
				this.nextPosition.z += (temp.transform.GetChild(0).renderer.bounds.size.x);
				temp = (GameObject) Instantiate(city.roadCross, this.nextPosition, Quaternion.identity);
				temp.transform.parent = this.transform;
			}
			currtile++;
		}
	}
}
