using UnityEngine;
using System.Collections;

public class ObjectJoystickRotate : MonoBehaviour {
	public int maxRotation = 45;
	public Vector3 pivot = Vector3.up;	
	private Transform self;
	private JoyInput joystick;
	public void Awake () {
		self = transform;
	}
	public void Start () {
		joystick = VirtualJoystick.GetInput();
	}
	public void Update () {
		self.localEulerAngles = joystick.axis * maxRotation;
	}
}
