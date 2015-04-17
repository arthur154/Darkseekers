using UnityEngine;
using System.Collections;

public class GenerateMap : MonoBehaviour {
	
	public GameObject roadStraight;
	public GameObject roadCross;
//	public GameObject[] stories;
//	public GameObject roof;
	public Texture2D[] buildingTextures;
	public GameObject player;
	public GameObject burningCar;
	public int numCars;
	public bool randomSpawn;
	public GameObject spawnPointPrefab;
	public int numSpawnPoints;
//	public GameObject window;
	public GameObject ground;
	public GameObject fence;
	public int width = 3;
	public int height = 3;
	public int blockSize = 3;
	public int minStories = 1;
	public int maxStories = 5;
	public float storyHeight = 6.5f;
	public bool survivalMode;

	// Use this for initialization
	void Start () {
		GameObject city = new GameObject("City");
		city.AddComponent<City>();
		City c = city.GetComponent<City>();
		City.newCity(c, roadStraight, roadCross, buildingTextures, ground, fence, width, height, blockSize, minStories, maxStories, storyHeight, survivalMode);
		int i = 0;
		while (i < numCars) {
			GameObject car = (GameObject) Instantiate(burningCar, City.randomSpawn(c), Quaternion.Euler(0f, Random.Range(0.0f,360.0f), Random.Range(0.0f,360.0f)));
			car.name = "Burning Car" + i+1;
			car.transform.parent = city.transform;
			i++;
		}

		if (this.randomSpawn) {
			i = 0;
			GameObject spawnPoints = new GameObject("ZombieSpawnPoints");
			while (i < numSpawnPoints) {
				GameObject spawnPoint = (GameObject) Instantiate(spawnPointPrefab, City.randomSpawn(c), Quaternion.identity);
				spawnPoint.name = "SpawnPoint";
				spawnPoint.transform.parent = spawnPoints.transform;
				SwarmAI script = spawnPoint.GetComponent<SwarmAI>();
				script.playerObj = player;
				script.zombieNumber = Random.Range(3, 9);
				i++;
			}	
		}
	}
}
