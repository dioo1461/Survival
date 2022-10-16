using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPoisoningButton : MonoBehaviour
{
	public DiseasesMNG diseasesMNG;

	public void OnMouseUpAsButton() {
		diseasesMNG.FoodPoisoning_Occur();
	}

	void Start()
    {
        
    }

    void Update()
    {
        
    }
}
