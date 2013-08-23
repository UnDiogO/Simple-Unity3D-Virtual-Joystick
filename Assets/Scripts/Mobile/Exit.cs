using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {
	public void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}
}
