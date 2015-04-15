using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {
	
	public void PlayGame(int screen){
		Application.LoadLevel(screen);
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
