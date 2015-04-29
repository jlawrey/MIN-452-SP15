using UnityEngine;
using System.Collections;

public class Z_MenuChooser : MonoBehaviour {

	public bool backToMenu = false;


	public void LoadScene(string scene){

		Z_LoadInterlude.to_scene = scene;
		Application.LoadLevel ("Loading");

	}

	void Start(){

		if (backToMenu) {
			StartCoroutine (backToGame ());
		}
	}

	public IEnumerator backToGame(){

		yield return new WaitForSeconds (6);
		Application.LoadLevel ("MMM_Level_01");
	}
}
