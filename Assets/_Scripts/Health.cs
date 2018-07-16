using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
	public Slider healthbar;
	public GameObject fill;
	public float maxHealth = 10f;
	float currentHealth;
	Animator anim;
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TakeDamage(float amount) {
		currentHealth -= amount;
		healthbar.value = currentHealth / maxHealth;

		if (currentHealth <= 0) {
			fill.SetActive(false);
			SendMessage("onDeath");
		} else {
			Animator anim = this.GetComponent<Animator>();
			if (anim) {
				anim.SetTrigger("Hurt");
			}
		}
	}

	void ReturnToMovement() {
		anim.SetTrigger("Movement");
	}

	public void Reset() {
		fill.SetActive(true);
		currentHealth = maxHealth;
		healthbar.value = currentHealth;
	}

	public void Heal(float amount) {
		currentHealth += amount;

		if (currentHealth > maxHealth) {
			maxHealth = currentHealth;
		}

		healthbar.value = currentHealth / maxHealth;
	}
}
