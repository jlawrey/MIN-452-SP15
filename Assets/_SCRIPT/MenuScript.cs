using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {
	
	public void PlayGame(){
		Application.LoadLevel("MMM_Level_01");
	}

	public void Options(){
		Application.LoadLevel("Options");
	}

	public void Instructions(){
		Application.LoadLevel("Instructions");
	}

	public void QuitGame(){
		Application.Quit ();
	}
}
