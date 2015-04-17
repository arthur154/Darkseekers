using UnityEngine;
using System.Collections;

public class ZCount : MonoBehaviour {
	private static int zombieKills=0;

	public static void ZombieKill () {
		zombieKills++;
	}

	public static void ResetKills () {
		zombieKills=0;
	}

	public static int GetKills () {
		return (zombieKills);
	}
}
