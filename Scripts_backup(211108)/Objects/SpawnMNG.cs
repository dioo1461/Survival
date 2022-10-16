using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMNG : MonoBehaviour
{
	bool enable_spawn = true;
	public GameObject tempEnemy;

	void Spawn_TempEnemy() {
		if (enable_spawn) {
			float randX = Random.Range(-8f, 8f);
			float randY = Random.Range(-4f, 4f);
			GameObject instance = Instantiate(tempEnemy, new Vector2(randX, randY), Quaternion.identity);
		}
	}

    void Start()
    {
		for(int i=0;i<3;i++)
			Invoke("Spawn_TempEnemy", (float)i);
    }

    void Update()
    {
        
    }
}
