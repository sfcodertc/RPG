using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "new Consumable", menuName = "Items/Consumable")]
public class Consumable : Item {
	public int heal = 0;
	public int maxHealthUp = 0;

	public override void Use() {
		GameObject player = Inventory.instance.player;
		Health playerHealth = player.GetComponent<Health>();

		playerHealth.Heal(heal);
		playerHealth.maxHealth += maxHealthUp;
		Inventory.instance.Remove(this);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
