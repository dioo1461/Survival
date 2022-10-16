using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
	public void OnMouseUpAsButton() {
		GameManager.singleton.SaveGame(1);
	}
	void Start()
    {
        
    }

    void Update()
    {
        
    }
}
