using UnityEngine;
using System.Collections;

[System.Serializable]
public class JoyInput {
	public Vector2 axis;
	public bool[] buttons;

	public JoyInput (int buttonsLength) {
		axis = Vector2.zero;
		buttons = new bool[buttonsLength];
	}
};

public class VirtualJoystick : MonoBehaviour {

	private class TouchWritable {
		public int fingerId = -1;
		public TouchPhase phase = TouchPhase.Ended;
		public Vector2 position;

		public void CopyFrom (Touch reference) {
			this.fingerId = reference.fingerId;
			this.phase = reference.phase;
			this.position = reference.position;
		}
	};

	[Range(0.1f, 1f)]
	public float proportion = 0.2f;
	private int radius = 100;

	public int margin = 20;
	public float speed = 5f;
	public bool useButton = true;
	public bool useDebug = true;

	public Texture2D graphicAxisArea;
	public Texture2D graphicAxisCursor;
	public Texture2D[] graphicButtons;

	private Rect axisLocation;
	private Rect cursorLocation;
	private Rect[] btLocations;

	private Vector2 center;
	private Vector2 axis;
	private Vector2 location;
	private Vector2 direction;

	private TouchWritable axisTouch = null;
	private static JoyInput input = null;

	public void Awake () {
		radius = Mathf.RoundToInt(Screen.width * proportion);
		int doubleRadius = 2 * radius;
		axisLocation = new Rect(margin, Screen.height - (doubleRadius + margin), doubleRadius, doubleRadius);
		center = new Vector2(axisLocation.x + radius, axisLocation.y + radius);
		location = center;
		int halfMargin = margin / 2;
		int sum = radius + halfMargin;
		int sub = radius - halfMargin;
		Vector2[] buttonsRef = new Vector2[4] {
			new Vector2(-sum, -sum), 
			new Vector2(-2 * sum, -sum), 
			new Vector2(-sum, -2 * sum), 
			new Vector2(-2 * sum, -2 * sum)
		};
		btLocations = new Rect[4];
		for (int index = 0; index < 4; index++) {
			btLocations[index] = new Rect(
				Screen.width + buttonsRef[index].x,
				Screen.height + buttonsRef[index].y,
				sub,
				sub
			);
		}
		int halfRadius = radius / 2;
		cursorLocation = new Rect(
			center.x - halfRadius / 2,
			center.y - halfRadius / 2,
			halfRadius,
			halfRadius
		);
		axisTouch = new TouchWritable();
	}

	public void Update () {
		ResetButtons(input);
		for (int index = 0; index < Input.touchCount; index++) {
			Touch touch = Input.GetTouch(index);
			Vector2 pos = touch.position;
			pos.y = Screen.height - pos.y;
			if (touch.position.x < Screen.width / 2) {
				if (touch.phase == TouchPhase.Began && axisLocation.Contains(pos))
					axisTouch.CopyFrom(touch);
				if (touch.fingerId == axisTouch.fingerId) {
					axisTouch.CopyFrom(touch);
				}
			} else {
				if (touch.fingerId == axisTouch.fingerId) {
					axisTouch.phase = TouchPhase.Ended;
					axisTouch.fingerId = -1;
				}
				if (useButton) {
					for (int i = 0; i < btLocations.Length; i++) {
						if (btLocations[i].Contains(pos)) {
							input.buttons[i] = touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled;
						}
					}
				}
			}
		}
		if (Input.touchCount == 0 || axisTouch.phase == TouchPhase.Ended || axisTouch.phase == TouchPhase.Canceled) {
			axisTouch.fingerId = -1;
			location = Vector2.Lerp(location, center, Time.deltaTime * speed);
		}
		if (axisTouch.phase == TouchPhase.Moved || axisTouch.phase == TouchPhase.Stationary) {
			location = axisTouch.position;
			location.y = Screen.height - location.y;
		}
		direction = location - center;
		direction.x = Mathf.Clamp(direction.x, -radius, radius);
		direction.y = -1f * Mathf.Clamp(direction.y, -radius, radius);
		if (input != null) {
			input.axis = direction / radius;
		}
	}

	public void OnGUI () {
#if !UNITY_IOS && !UNITY_ANDROID && !UNITY_WP8
		Rect warning = new Rect (Screen.width / 2 - 100, Screen.height / 2, 200, 20);
		GUI.Label (warning, "Use mobile Plataform !!!");
#endif
		if (input != null && useDebug) {
			GUI.Label(new Rect(margin, margin, 300, 20), "Axis " + input.axis);

			if (useButton) {
				int height = margin;
				for (int index = 0; index < input.buttons.Length; index++) {
					GUI.Label(new Rect(Screen.width - 130, height, 120, 20), "Button[" + index + "] = " + input.buttons[index]);
					height += 20;
				}
			}
		}
		GUI.DrawTexture(axisLocation, graphicAxisArea);
		int halfRadius = radius / 2;
		cursorLocation.x = location.x - halfRadius / 2;
		cursorLocation.y = location.y - halfRadius / 2;
		GUI.DrawTexture(cursorLocation, graphicAxisCursor);
		if (useButton) {
			for (int index = 0; index < btLocations.Length; index++) {
				GUI.DrawTexture(btLocations[index], graphicButtons[index]);
			}
		}
	}

	private void ResetButtons (JoyInput joyInput) {
		if (joyInput != null) {
			for (int index = 0; index < joyInput.buttons.Length; index++) {
				joyInput.buttons[index] = false;
			}
		}
	}

	public static JoyInput GetInput () {
		if (input == null) {
			input = new JoyInput(4);
		}
		return input;
	}
}
