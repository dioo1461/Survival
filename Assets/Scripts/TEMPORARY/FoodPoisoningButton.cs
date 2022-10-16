using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPoisoningButton : MonoBehaviour
{
	public DiseasesMGR diseasesMNG;

	public void OnMouseUpAsButton() {
		diseasesMNG.Incur_FoodPoisoning();
	}

	void Start()
    {
        
    }

    void Update()
    {
        
    }
}
