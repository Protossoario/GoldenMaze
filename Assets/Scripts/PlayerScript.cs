using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public float maxSpeed = 0.1f;
	public int timeMax = 2;
	public int life = 3;
	bool attacking;
	public bool turn; // Check whether it's the players' turn to move, if true
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

	public void reduceLife(int dam) {
		if (anim.GetBool ("MoveUp")) {
			anim.SetBool("DamageUp", true);
			anim.SetBool("DamageDown", false);
			anim.SetBool("DamageLeft", false);
			anim.SetBool("DamageRight", false);
		} else if (anim.GetBool ("MoveDown")) {
			anim.SetBool("DamageUp", false);
			anim.SetBool("DamageDown", true);
			anim.SetBool("DamageLeft", false);
			anim.SetBool("DamageRight", false);
		} else if (anim.GetBool ("MoveLeft")) {
			anim.SetBool("DamageUp", false);
			anim.SetBool("DamageDown", false);
			anim.SetBool("DamageLeft", true);
			anim.SetBool("DamageRight", false);
		} else if (anim.GetBool ("MoveRight")) {
			anim.SetBool("DamageUp", false);
			anim.SetBool("DamageDown", false);
			anim.SetBool("DamageLeft", false);
			anim.SetBool("DamageRight", true);
		}

		life -= dam;
	}

	public void recoverLife(int l) {
		life += l;
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
					if(colUp.gameObject.GetComponent<SpiderScript>().getLife() > 1)
						colUp.gameObject.GetComponent<SpiderScript>().reduceLife(1);
					else {
						Destroy(colUp.gameObject); 
					}
				}
				else if (colUp.CompareTag("Key")) {
					KeyScript.dungeonKey = true;
					Destroy (colUp.gameObject);
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
					if(colDown.gameObject.GetComponent<SpiderScript>().getLife() > 1)
						colDown.gameObject.GetComponent<SpiderScript>().reduceLife(1);
					else {
						Destroy(colDown.gameObject); 
					}
				}
				else if (colDown.CompareTag("Key")) {
					KeyScript.dungeonKey = true;
					Destroy (colDown.gameObject);
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
					if(colLeft.gameObject.GetComponent<SpiderScript>().getLife() > 1)
						colLeft.gameObject.GetComponent<SpiderScript>().reduceLife(1);
					else {
						Destroy(colLeft.gameObject); 
					}
				}
				else if (colLeft.CompareTag("Key")) {
					KeyScript.dungeonKey = true;
					Destroy (colLeft.gameObject);
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
					if(colRight.gameObject.GetComponent<SpiderScript>().getLife() > 1 )
						colRight.gameObject.GetComponent<SpiderScript>().reduceLife(1);
					else {
						Destroy(colRight.gameObject); 
					}
				}
				else if (colRight.CompareTag("Key")) {
					KeyScript.dungeonKey = true;
					Destroy (colRight.gameObject);
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
				rigidbody2D.velocity = new Vector2 (0f, 0f);
				attacking = false;
			}
		}
	}

	void turnOffDamageAnimation() {
		anim.SetBool ("DamageUp", false);
		anim.SetBool ("DamageDown", false);
		anim.SetBool ("DamageLeft", false);
		anim.SetBool ("DamageRight", false);
	}

	void turnOffAttackAnimation() {
		anim.SetBool ("AttackUp", false);
		anim.SetBool ("AttackDown", false);
		anim.SetBool ("AttackLeft", false);
		anim.SetBool ("AttackRight", false);
	}
}
