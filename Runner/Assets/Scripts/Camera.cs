using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	public Player _Player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		switch (_Player.State) {
			case 0:
				if (GUI.Button (new Rect (Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "RUN!")) {
					_Player.Run();
				}
				if(GUI.Button (new Rect (Screen.width / 2 - 50, Screen.height / 2 + 35, 100, 50), "Restart"))
				{
					Application.LoadLevel("base");
				}
				break;
			default:
				if (GUI.Button (new Rect (10, 10, 50, 50), "||")) {
					_Player.Stop();
				}
				if (GUI.Button (new Rect (Screen.width - 120, Screen.height - 60, 50, 50), "Jerk")) {
					_Player.Jerk();
				}
				if (GUI.Button (new Rect (Screen.width - 60, Screen.height - 60, 50, 50), "Jump")) {
					_Player.Jump();
				}
				break;
		}
		
	}
}
