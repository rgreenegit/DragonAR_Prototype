using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FoodSpawner : MonoBehaviour {

	public GameObject foodPrefab;
	public GameObject[] spawnPoints;
	public int maxFood = 3;

	void Start () {
		int numberToSpawn = Random.Range(1, maxFood+1);
		Debug.Log(numberToSpawn);

		ShuffleArray(spawnPoints);

		for(int i = 0; i < numberToSpawn; i++){
			GameObject food = Instantiate(foodPrefab, new Vector3(0,0,0), Quaternion.identity);
			food.transform.parent = gameObject.transform;
			food.transform.position = spawnPoints[i].transform.position;

			food.GetComponent<InteractiveObject>().foodType = (InteractiveObject.FoodTypes) Random.Range (0, 1 + System.Enum.GetValues(typeof(InteractiveObject.FoodTypes)).Cast<int>().Max());
		}	
	}

	public static void ShuffleArray<T>(T[] arr){
		for (int i = arr.Length -1; i > 0; i--){
			int r = Random.Range(0, i+1);
			T tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
	}
}
