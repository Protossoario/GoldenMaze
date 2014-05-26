﻿using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public float maxSpeed = 0.1f;
	public int timeMax = 2;
	public bool turn; // Check whether it's the players' turn to move, if true
	bool attacking;
	/* Directions:
	 * 0 = none
	 * 1 = up
	 * 2 = down
	 * 3 = left
	 * 4 = right
	 */
	int direction;
	int time;
	Animator anim;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		direction = 0;
		turn = false;
		attacking = false;
	}

	public void startTurn() {
		turn = true;
	}

	public bool getTurn() {
		return turn;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//rigidbody2D.velocity = new Vector2 (Mathf.Lerp(0, Input.GetAxis("Horizontal") * maxSpeed, 10.0f), Mathf.Lerp(0, Input.GetAxis("Vertical") * maxSpeed, 10.0f)) ;
		if (direction == 0 && turn && !attacking) {
			Collider2D colUp = Physics2D.OverlapPoint(new Vector2(this.transform.position.x, this.transform.position.y + 0.32f));
			Collider2D colDown = Physics2D.OverlapPoint(new Vector2(this.transform.position.x, this.transform.position.y - 0.32f));
			Collider2D colLeft = Physics2D.OverlapPoint(new Vector2(this.transform.position.x - 0.32f, this.transform.position.y));
			Collider2D colRight = Physics2D.OverlapPoint(new Vector2(this.transform.position.x + 0.32f, this.transform.position.y));
			bool moveUp = Input.GetKey (KeyCode.UpArrow);
			bool moveDown = Input.GetKey (KeyCode.DownArrow);
			bool moveLeft = Input.GetKey (KeyCode.LeftArrow);
			bool moveRight = Input.GetKey (KeyCode.RightArrow);
			if (moveUp) {
				if (colUp == null) {
					direction = 1;
					time = timeMax;
					anim.SetBool ("MoveUp", true);
					anim.SetBool ("MoveDown", false);
					anim.SetBool ("MoveLeft", false);
					anim.SetBool ("MoveRight", false);
				}
				else if (colUp.CompareTag("Spider")) {
					attacking = true;
					time = timeMax;
					anim.SetBool("AttackUp", true);
					anim.SetBool("AttackDown", false);
					anim.SetBool("AttackLeft", false);
					anim.SetBool("AttackRight", false);
					//Destroy(colUp.transform.parent.gameObject);
				}
			}
			else if (moveDown) {
				if (colDown == null) {
					direction = 2;
					time = timeMax;
					anim.SetBool ("MoveUp", false);
					anim.SetBool ("MoveDown", true);
					anim.SetBool ("MoveLeft", false);
					anim.SetBool ("MoveRight", false);
				}
				else if (colDown.CompareTag("Spider")) {
					attacking = true;
					time = timeMax;
					anim.SetBool("AttackUp", false);
					anim.SetBool("AttackDown", true);
					anim.SetBool("AttackLeft", false);
					anim.SetBool("AttackRight", false);
					//Destroy(colDown.transform.parent.gameObject);
				}
			}
			else if (moveLeft) {
				if (colLeft == null) {
					direction = 3;
					time = timeMax;
					anim.SetBool ("MoveUp", false);
					anim.SetBool ("MoveDown", false);
					anim.SetBool ("MoveLeft", true);
					anim.SetBool ("MoveRight", false);
				}
				else if (colLeft.CompareTag("Spider")) {
					attacking = true;
					time = timeMax;
					anim.SetBool("AttackUp", false);
					anim.SetBool("AttackDown", false);
					anim.SetBool("AttackLeft", true);
					anim.SetBool("AttackRight", false);
					//Destroy(colLeft.transform.parent.gameObject);
				}
			}
			else if (moveRight) {
				if (colRight == null) {
					direction = 4;
					time = timeMax;
					anim.SetBool ("MoveUp", false);
					anim.SetBool ("MoveDown", false);
					anim.SetBool ("MoveLeft", false);
					anim.SetBool ("MoveRight", true);
				}
				else if (colRight.CompareTag("Spider")) {
					attacking = true;
					time = timeMax;
					anim.SetBool("AttackUp", false);
					anim.SetBool("AttackDown", false);
					anim.SetBool("AttackLeft", false);
					anim.SetBool("AttackRight", true);
					//Destroy(colRight.transform.parent.gameObject);
				}
			}
		} else if (turn) {
			switch (direction) {
				case 1:
					rigidbody2D.velocity = new Vector2 (0f, maxSpeed);
				break;
				case 2:
					rigidbody2D.velocity = new Vector2 (0f, -maxSpeed);
				break;
				case 3:
					rigidbody2D.velocity = new Vector2 (-maxSpeed, 0f);
				break;
				case 4:
					rigidbody2D.velocity = new Vector2 (maxSpeed, 0f);
				break;
			}

			time--;
			if (time <= 0) {
				direction = 0;
				turn = false;
				if (attacking) {
					anim.SetBool("AttackUp", false);
					anim.SetBool("AttackDown", false);
					anim.SetBool("AttackLeft", false);
					anim.SetBool("AttackRight", false);
				}
				attacking = false;
				rigidbody2D.velocity = new Vector2 (0f, 0f);
			}
		}
	}
}
