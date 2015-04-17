using UnityEngine;
using System.Collections;

public class Building2 : MonoBehaviour {

	public City city;
	public GameObject parent;
	public GameObject story;
	public GameObject roof;
	public Vector3 pos;

	public static void initialize (City c, Building2 b, GameObject paren, GameObject sty, GameObject rf, Vector3 orig) {
		b.city = c;
		b.parent = paren;
		b.story = sty;
		b.roof = rf;
		b.pos = orig;
	}
		
	public void Start() {
		int i = Random.Range(city.minStories, city.maxStories + 1);
		GameObject storyObj = null;
		while (i > 0) {
			storyObj = (GameObject) Instantiate(story, pos, Quaternion.identity);
			storyObj.name = "Story " + i+1;
			storyObj.transform.parent = parent.transform;
			pos.y += storyObj.transform.GetChild(0).transform.GetChild(0).renderer.bounds.size.y;
			i--;
		}
		if (i >= 0) {
			pos.y -= storyObj.transform.GetChild(0).transform.GetChild(0).renderer.bounds.size.y;
			GameObject roofObj = (GameObject) Instantiate(roof, pos, Quaternion.identity);
			roofObj.name = "Roof";
			roofObj.transform.parent = parent.transform;
		}
	}
}
