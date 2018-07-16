using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour {

	CharacterController cc;
	Animator anim;
	Camera cam;
	Health health;
	public float moveSpeed;
	float gravity = 0f;
	float jumpVelocity = 0f;
	public float jumpHeight;
	public Vector3 checkpoint;
	string state = "Movement";

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
		health = GetComponent<Health>();
		cam = Camera.main;
		checkpoint = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
			case "Movement":
				Movement();
				Swing();
				break;
			case "Jump":
				Jump();
				Movement();
				break;
		}

		Menu();
	}

	void Menu() {
		if (Input.GetKeyDown(KeyCode.Tab)) {
			GameObject ip = Inventory.instance.inventoryPanel;

			if (!ip.activeSelf) {
				ip.SetActive(true);
			} else {
				ip.SetActive(false);
			}
		}
	}

	void DealWeaponDamage() {
		Vector3 center = transform.position + transform.forward + transform.up;
		Vector3 halfExtents = new Vector3(0.5f, 1.0f, 0.5f);

		Collider[] hits = Physics.OverlapBox(center, halfExtents, transform.rotation);
	
		foreach (Collider hit in hits) {
			Health otherHealth = hit.GetComponent<Health>();

			if (otherHealth) {
				otherHealth.TakeDamage(1f);
			}
		}
	}

	void Movement() {
		float x = Input.GetAxisRaw("Horizontal");
		float z = Input.GetAxisRaw("Vertical");

		Vector3 direction = new Vector3(x, 0, z).normalized;
		float cameraDirection = cam.transform.localEulerAngles.y;

		direction = Quaternion.AngleAxis(cameraDirection, Vector3.up) * direction;

		Vector3 velocity = direction * moveSpeed * Time.deltaTime;

		float percentSpeed = velocity.magnitude / (moveSpeed * Time.deltaTime);
		anim.SetFloat("movePercent", percentSpeed);

		if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded) {
			jumpVelocity = jumpHeight;	
			ChangeState("Jump");
		}

		Vector3 gravityVector = -Vector3.up * gravity * Time.deltaTime;
		Vector3 jumpVector = Vector3.up * jumpVelocity * Time.deltaTime;

		cc.Move(velocity + gravityVector + jumpVector);

		if (cc.isGrounded) {
			gravity = 0;
			
		} else {
			gravity += 0.25f;
			gravity = Mathf.Clamp(gravity, 1f, 20f);
		}

		if (state == "Jump" && cc.isGrounded) {
			ChangeState("Movement");
		}

		if (velocity.magnitude > 0) {
			float yAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
			transform.localEulerAngles = new Vector3(0, yAngle, 0);
		}
	}

	void Jump() {

		if (jumpVelocity < 0) return;
		jumpVelocity -= 1.25f;


	}

	void Swing() {
		if (Input.GetMouseButtonDown(0)) {
			ChangeState("Swing");
		}
	}

	void ChangeState(string stateName) {
		state = stateName;
		anim.SetTrigger(stateName);
	}

	void ReturnToMovement() {
		ChangeState("Movement");
	}

	void onDeath() {
		if (state != "Break") {
			ChangeState("Break");
		}
	}

	void ReturnToCheckpoint() {
		//Debug.Log("Check");
		health.Reset();
		ChangeState("Movement");
		transform.position = checkpoint;
		
	}
}
