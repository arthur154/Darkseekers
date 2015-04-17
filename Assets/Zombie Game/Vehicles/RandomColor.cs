using UnityEngine;
using System.Collections;

public class RandomColor : MonoBehaviour {

	public Color[] colors;

	// Use this for initialization
	void Start () {
		this.transform.GetChild(0).transform.GetChild(0).renderer.material.color = randomColor();
	}

	Color randomColor() {
		int rand = Random.Range(0, this.colors.Length);
		return this.colors[rand];
	}
}
