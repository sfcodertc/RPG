using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class rockSpitterController : MonoBehaviour {
	NavMeshAgent nav;
	public GameObject player;
	Vector3 anchor;
	Animator anim;
	string state;
	public GameObject rock;
	public Transform shootOrigin;
	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent>();
		anchor = transform.position;
		anim = GetComponent<Animator>();
		state = "Movement";
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
			case "Movement":
				Move();
				break;
			case "Shoot":
				break;
		}
	}

	void Move() {
		Vector3 target = player.transform.position;
		nav.stoppingDistance = 4f;

		if (Vector3.Distance(transform.position, target) > 7) {
			target = anchor;
			nav.stoppingDistance = 0;
		} else {
			if (Random.Range(0, 100f) < 2f) {
				ChangeState("Shoot");
			}
		}

		anim.SetFloat("movePercent", nav.velocity.magnitude / nav.speed);
		nav.SetDestination(target);
	}

	void ChangeState(string stateName) {
		state = stateName;
		anim.SetTrigger(stateName);
	}

	void ReturnToMovement() {
		ChangeState("Movement");
	}

	void ShootRock() {
		GameObject r = Instantiate(rock, shootOrigin.position, Quaternion.identity);

		Rigidbody rockBody = r.GetComponent<Rigidbody>();
		rockBody.AddForce(transform.forward * 500f);
	}

	void onDeath() {
		Destroy(gameObject, 2f);
	}
}
