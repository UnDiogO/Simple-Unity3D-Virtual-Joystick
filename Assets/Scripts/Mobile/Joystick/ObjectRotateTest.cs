using UnityEngine;
using System.Collections;

public class ObjectRotateTest : MonoBehaviour {
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
		Vector3 result = Vector3.zero;
		result.x = joystick.axis.x;
		result.y = joystick.axis.y;
		self.localEulerAngles = result * maxRotation;
	}
}
