using UnityEngine;
using System.Collections;

public class DamageControl : MonoBehaviour {
	public ZombieAI z;
	public bool limb, body, head;

	public void TakeDamage(int damage) {
		if (limb) z.TakeDamage(damage-5);
		if (body) z.TakeDamage(damage);
		if (head) z.TakeDamage(damage*3);
	}
}
