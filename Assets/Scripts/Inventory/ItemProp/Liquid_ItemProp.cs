using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid_ItemProp : Food_ItemProp
{
	public new Liquid_ItemData Data { get; private set; }

	public Liquid_ItemProp(Liquid_ItemData data, int amount = 0) : base(data, amount){
		Data = data;
		Amount = amount;
	}



}
