using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
	public void OnMouseUpAsButton() {
		GameManager.singleton.LoadGame(1);
	}
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
