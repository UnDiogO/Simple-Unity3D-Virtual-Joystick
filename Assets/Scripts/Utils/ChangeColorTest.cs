using UnityEngine;
using System.Collections;

public class ChangeColorTest : MonoBehaviour {	
	private JoyInput joystick;
	private float[] rgba = new float[4];
	public void Start () {
		joystick = VirtualJoystick.GetInput();
	}
	public void Update () {
		float magnitude = Mathf.Clamp(joystick.axis.magnitude, 0.5f, 1f);
		for (int index = 0; index < joystick.buttons.Length; index++) {
			rgba[index] = joystick.buttons[index] ? 1f : 0.5f;
		}
		renderer.material.SetColor("_Color", new Color (
			magnitude * rgba[0], 
			magnitude * rgba[1], 
			magnitude * rgba[2], 
			1f - magnitude * rgba[3]
		));
	}
}
