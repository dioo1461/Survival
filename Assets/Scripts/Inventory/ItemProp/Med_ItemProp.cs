using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Med_ItemProp : Usable_ItemProp
{
	public new Med_ItemData Data { get; private set; }

	public Med_ItemProp(Med_ItemData data, int amount) : base(data, amount) {

	}
}
