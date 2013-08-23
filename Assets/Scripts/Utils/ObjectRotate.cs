using UnityEngine;
using System.Collections;

public class ObjectRotate : MonoBehaviour {
	public float speed = 100f;
	public Vector3 pivot = Vector3.up;
	private Transform self;
	public void Awake () {
		self = transform;
	}
	public void Update () {
		self.Rotate(pivot * speed * Time.deltaTime);
	}
}
