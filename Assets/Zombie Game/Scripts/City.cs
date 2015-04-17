using UnityEngine;
using System.Collections;

public class City : MonoBehaviour {
	
	public GameObject roadStraight;
	public GameObject roadCross;
	public Texture2D[] buildingTextures;
//	public GameObject[] stories;
//	public GameObject roof;
//	public GameObject window;
	public GameObject ground;
	public GameObject fence;
	public int width;
	public int height;
	public int blockSize;
	public int minStories;
	public int maxStories;
	public float storyHeight;
	public bool survivalMode;
	public Vector3 finalPosition;
	
//	public static void newCity(City c, GameObject rStraight, GameObject rCross, Texture2D[] texts, GameObject[] strs, GameObject rf, GameObject wind, GameObject gnd, GameObject fen, int wid, int hei, int bSize, int minS, int maxS, float sHeight) {
//		c.roadStraight = rStraight;
//		c.roadCross = rCross;
//		c.buildingTextures = texts;
//		c.stories = strs;
//		c.roof = rf;
//		c.window = 
//		c.ground = gnd;
//		c.fence = fen;
//		c.width = wid;
//		c.height = hei;
//		c.blockSize = bSize;
//		c.minStories = minS;
//		c.maxStories = maxS;
//		c.storyHeight = sHeight;
//		c.initialize();
//	}

	public static void newCity(City c, GameObject rStraight, GameObject rCross, Texture2D[] texts, GameObject gnd, GameObject fen, int wid, int hei, int bSize, int minS, int maxS, float sHeight, bool surv) {
		c.roadStraight = rStraight;
		c.roadCross = rCross;
		c.buildingTextures = texts;
		c.ground = gnd;
		c.fence = fen;
		c.width = wid;
		c.height = hei;
		c.blockSize = bSize;
		c.minStories = minS;
		c.maxStories = maxS;
		c.storyHeight = sHeight;
		c.survivalMode = surv;
		c.initialize();
	}
	
	void initialize() {
		GameObject tempblock = new GameObject("Block");
		tempblock.AddComponent<Block>();
		Block temp_b = tempblock.GetComponent<Block>();
		Vector3 startingPosition = Block.newBlock(temp_b, this, new Vector3(0f, 0f, 0f), Block.blockType.U, Block.fenceType.none, false);
		GameObject temp = (GameObject) Instantiate(this.roadCross, new Vector3(0f, 0f, 0f), Quaternion.AngleAxis(180, Vector3.up));
		startingPosition -= 2*startingPosition;
		startingPosition.x += (temp.transform.GetChild(0).renderer.bounds.size.x);
		Destroy(tempblock);
		Destroy(temp);
		
		Vector3 nextPosition = startingPosition;
		Block.fenceType tempFence;
		int i=-1,j=-1;
		while (i <= this.height) {
			while (j <= this.width) {
				GameObject block = new GameObject("Block");
				block.AddComponent<Block>();
				Block b = block.GetComponent<Block>();
				block.layer = LayerMask.NameToLayer("Ground");
				block.transform.parent = this.transform;
				bool survival = false;
				
				if (i == -1) {
					if (j == -1) {
						tempFence = Block.fenceType.bottomleft;
					} else if (j < this.width) {
						tempFence = Block.fenceType.bottom;
					} else {
						tempFence = Block.fenceType.bottomright;
					}
				} else if (i == this.height) {
					if (j == -1) {
						tempFence = Block.fenceType.topleft;
					} else if (j < this.width) {
						tempFence = Block.fenceType.top;
					} else {
						tempFence = Block.fenceType.topright;
					}
				} else {
					if (j == -1) {
						tempFence = Block.fenceType.left;
					} else if (j == this.width) {
						tempFence = Block.fenceType.right;
					} else {
						tempFence = Block.fenceType.none;
						if (survivalMode) {
							survival = true;
						}
					}
				}
				
				if (i < this.height) {
					if (j < this.width) {
						if (j == this.width - 1 && i == this.width - 1) {
							tempblock = new GameObject("Block");
							tempblock.AddComponent<Block>();
							temp_b = tempblock.GetComponent<Block>();
							this.finalPosition = Block.newBlock(temp_b, this, nextPosition, Block.blockType.O, Block.fenceType.none, survival);
							this.finalPosition.y = (this.maxStories + 1) * this.storyHeight;
							Destroy(tempblock);
						}
						nextPosition = Block.newBlock(b, this, nextPosition, Block.blockType.L, tempFence, survival);
					} else {
						nextPosition = Block.newBlock(b, this, nextPosition, Block.blockType.U, tempFence, survival);
						nextPosition.x = startingPosition.x;
					}
				} else {
					if (j < this.width) {
						nextPosition = Block.newBlock(b, this, nextPosition, Block.blockType.C, tempFence, survival);
					} else {
						nextPosition = Block.newBlock(b, this, nextPosition, Block.blockType.O, tempFence, survival);
						nextPosition = new Vector3(0f, 0f, 0f);
					}
				}
				j++;
			}
			j = -1;
			i++;
		}
	}
	
	public static Vector3 randomSpawn(City c) {
		Vector3 randomSpawn = new Vector3(Random.Range(0f, c.finalPosition.x), c.finalPosition.y, Random.Range(0f, c.finalPosition.z));
		while (!IsGround(randomSpawn)) {
			randomSpawn = new Vector3(Random.Range(0f, c.finalPosition.x), c.finalPosition.y, Random.Range(0f, c.finalPosition.z));
		}
		randomSpawn.y = 5f;
		return randomSpawn;
	}
	
	public static bool IsGround(Vector3 pos) {
		bool result = false;
		RaycastHit hit;
		if (Physics.Raycast(pos, -Vector3.up, out hit)) {
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) {
				result = true;
			}
		}
		
		return result;
	}
}
