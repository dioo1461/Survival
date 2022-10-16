using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable_ItemProp : ItemProp 
{
	public new Usable_ItemData Data { get; private set; }

	public Usable_ItemProp(Usable_ItemData data, int amount) : base(data, amount){
		Data = data;
		Set_Amount(amount);
	}

}