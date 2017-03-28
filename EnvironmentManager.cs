using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

	//public GameObject character;
	public MeshRenderer terrain;

	public void toggleState(){
		terrain.enabled = !terrain.enabled;
	}

}
