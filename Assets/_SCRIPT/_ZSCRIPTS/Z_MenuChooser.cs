using UnityEngine;
using System.Collections;

public class Z_MenuChooser : MonoBehaviour {


	public void LoadScene(string scene){

		Z_LoadInterlude.to_scene = scene;
		Application.LoadLevel ("Loading");

	}
}
