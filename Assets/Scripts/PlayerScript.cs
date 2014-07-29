using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public AudioClip swordSound;
	public AudioClip spiderSound;
	public float maxSpeed = 0.1f;
	public int timeMax = 2;
	public int life = 3;
	bool attacking;
	public string lvlChange;
	public bool turn; // Check whether it's the players' turn to move, if true
	bool moveUp;
	bool moveDown;
	bool moveLeft;
	bool moveRight;
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

	private float touch_delta;
	private Vector2 current_position;
	private Vector2 previous_position;
	public int comfort;
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

	public void setMoveUp(bool b) {
		moveUp = b;
	}

	public bool getMoveUp() {
		return moveUp;
	}

	public void setMoveDown(bool b) {
		moveDown = b;
	}
	
	public bool getMoveDown() {
		return moveDown;
	}

	public void setMoveLeft(bool b) {
		moveLeft = b;
	}
	
	public bool getMoveLeft() {
		return moveLeft;
	}

	public void setMoveRight(bool b) {
		moveRight = b;
	}
	
	public bool getMoveRight() {
		return moveRight;
	}

	public void reduceLife(int dam) {
		if (anim.GetBool ("MoveUp")) {
			anim.SetBool("DamageUp", true);
			anim.SetBool("DamageDown", false);
			anim.SetBool("DamageLeft", false);
			anim.SetBool("DamageRight", false);
		}
		else if (anim.GetBool ("MoveDown")) {
			anim.SetBool("DamageUp", false);
			anim.SetBool("DamageDown", true);
			anim.SetBool("DamageLeft", false);
			anim.SetBool("DamageRight", false);
		}
		else if (anim.GetBool ("MoveLeft")) {
			anim.SetBool("DamageUp", false);
			anim.SetBool("DamageDown", false);
			anim.SetBool("DamageLeft", true);
			anim.SetBool("DamageRight", false);
		}
		else if (anim.GetBool ("MoveRight")) {
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

			moveUp = Input.GetKey (KeyCode.UpArrow);
			moveDown = Input.GetKey (KeyCode.DownArrow);
			moveLeft = Input.GetKey (KeyCode.LeftArrow);
			moveRight = Input.GetKey (KeyCode.RightArrow);

			if (moveUp) {
				if (colUp != null) {
					if (colUp.CompareTag("Spider")) {
						attacking = true;
						time = timeMax;
						anim.SetBool("AttackUp", true);
						anim.SetBool("AttackDown", false);
						anim.SetBool("AttackLeft", false);
						anim.SetBool("AttackRight", false);
						if(colUp.gameObject.GetComponent<SpiderScript>().getLife() > 1)
							colUp.gameObject.GetComponent<SpiderScript>().reduceLife(1);
						else {
							Bounds spiderBounds = colUp.gameObject.GetComponent<SpriteRenderer>().bounds;
							Destroy(colUp.gameObject);
							AstarPath.active.UpdateGraphs(spiderBounds);
						}
					}
					else if (colUp.CompareTag("Key")) {
						KeyScript.dungeonKey = true;
						Destroy (colUp.gameObject);
					}
					else if (colUp.CompareTag("Door") && KeyScript.dungeonKey) {
						Application.LoadLevel(lvlChange);
					}
					else if (!colUp.CompareTag("Wall")) {
						direction = 1;
						time = timeMax;
						anim.SetBool ("MoveUp", true);
						anim.SetBool ("MoveDown", false);
						anim.SetBool ("MoveLeft", false);
						anim.SetBool ("MoveRight", false);
					}
				}
				else {
					direction = 1;
					time = timeMax;
					anim.SetBool ("MoveUp", true);
					anim.SetBool ("MoveDown", false);
					anim.SetBool ("MoveLeft", false);
					anim.SetBool ("MoveRight", false);
				}
			}
			else if (moveDown) {
				if (colDown != null) {
					if (colDown.CompareTag("Spider")) {
						attacking = true;
						time = timeMax;
						anim.SetBool("AttackUp", false);
						anim.SetBool("AttackDown", true);
						anim.SetBool("AttackLeft", false);
						anim.SetBool("AttackRight", false);
						if(colDown.gameObject.GetComponent<SpiderScript>().getLife() > 1)
							colDown.gameObject.GetComponent<SpiderScript>().reduceLife(1);
						else {
							Bounds spiderBounds = colDown.gameObject.GetComponent<SpriteRenderer>().bounds;
							Destroy(colDown.gameObject);
							AstarPath.active.UpdateGraphs(spiderBounds);
						}
					}
					else if (colDown.CompareTag("Key")) {
						KeyScript.dungeonKey = true;
						Destroy (colDown.gameObject);
					}
					else if (colDown.CompareTag("Door") && KeyScript.dungeonKey) {
						Application.LoadLevel(lvlChange);
					}
					else if (!colDown.CompareTag("Wall")) {
						direction = 2;
						time = timeMax;
						anim.SetBool ("MoveUp", false);
						anim.SetBool ("MoveDown", true);
						anim.SetBool ("MoveLeft", false);
						anim.SetBool ("MoveRight", false);
					}
				}
				else {
					direction = 2;
					time = timeMax;
					anim.SetBool ("MoveUp", false);
					anim.SetBool ("MoveDown", true);
					anim.SetBool ("MoveLeft", false);
					anim.SetBool ("MoveRight", false);
				}
			}
			else if (moveLeft) {
				if (colLeft != null) {
					if (colLeft.CompareTag("Spider")) {
						attacking = true;
						time = timeMax;
						anim.SetBool("AttackUp", false);
						anim.SetBool("AttackDown", false);
						anim.SetBool("AttackLeft", true);
						anim.SetBool("AttackRight", false);
						if(colLeft.gameObject.GetComponent<SpiderScript>().getLife() > 1)
							colLeft.gameObject.GetComponent<SpiderScript>().reduceLife(1);
						else {
							Bounds spiderBounds = colLeft.gameObject.GetComponent<SpriteRenderer>().bounds;
							Destroy(colLeft.gameObject);
							AstarPath.active.UpdateGraphs(spiderBounds);
						}
					}
					else if (colLeft.CompareTag("Key")) {
						KeyScript.dungeonKey = true;
						Destroy (colLeft.gameObject);
					}
					else if (colLeft.CompareTag("Door") && KeyScript.dungeonKey) {
						Application.LoadLevel(lvlChange);
					}
					else if (!colLeft.CompareTag("Wall")) {
						direction = 3;
						time = timeMax;
						anim.SetBool ("MoveUp", false);
						anim.SetBool ("MoveDown", false);
						anim.SetBool ("MoveLeft", true);
						anim.SetBool ("MoveRight", false);
					}
				}
				else {
					direction = 3;
					time = timeMax;
					anim.SetBool ("MoveUp", false);
					anim.SetBool ("MoveDown", false);
					anim.SetBool ("MoveLeft", true);
					anim.SetBool ("MoveRight", false);
				}
			}
			else if (moveRight) {
				if (colRight != null) {
					if (colRight.CompareTag("Spider")) {
						attacking = true;
						time = timeMax;
						anim.SetBool("AttackUp", false);
						anim.SetBool("AttackDown", false);
						anim.SetBool("AttackLeft", false);
						anim.SetBool("AttackRight", true);
						if(colRight.gameObject.GetComponent<SpiderScript>().getLife() > 1 )
							colRight.gameObject.GetComponent<SpiderScript>().reduceLife(1);
						else {
							Bounds spiderBounds = colRight.gameObject.GetComponent<SpriteRenderer>().bounds;
							Destroy(colRight.gameObject);
							AstarPath.active.UpdateGraphs(spiderBounds);; 
						}
					}
					else if (colRight.CompareTag("Key")) {
						KeyScript.dungeonKey = true;
						Destroy (colRight.gameObject);
					}
					else if (colRight.CompareTag("Door") && KeyScript.dungeonKey) {
						Application.LoadLevel(lvlChange);
					}
					else if (!colRight.CompareTag("Wall")) {
						direction = 4;
						time = timeMax;
						anim.SetBool ("MoveUp", false);
						anim.SetBool ("MoveDown", false);
						anim.SetBool ("MoveLeft", false);
						anim.SetBool ("MoveRight", true);
					}
				}
				else {
					direction = 4;
					time = timeMax;
					anim.SetBool ("MoveUp", false);
					anim.SetBool ("MoveDown", false);
					anim.SetBool ("MoveLeft", false);
					anim.SetBool ("MoveRight", true);
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
		if (anim.GetBool ("AttackUp")) {
			anim.SetBool ("MoveUp", true);
			anim.SetBool ("MoveDown", false);
			anim.SetBool ("MoveLeft", false);
			anim.SetBool ("MoveRight", false);
		}
		else if (anim.GetBool ("AttackDown")) {
			anim.SetBool ("MoveUp", false);
			anim.SetBool ("MoveDown", true);
			anim.SetBool ("MoveLeft", false);
			anim.SetBool ("MoveRight", false);
		}
		else if (anim.GetBool ("AttackLeft")) {
			anim.SetBool ("MoveUp", false);
			anim.SetBool ("MoveDown", false);
			anim.SetBool ("MoveLeft", true);
			anim.SetBool ("MoveRight", false);
		}
		else if (anim.GetBool ("AttackRight")) {
			anim.SetBool ("MoveUp", false);
			anim.SetBool ("MoveDown", false);
			anim.SetBool ("MoveLeft", false);
			anim.SetBool ("MoveRight", true);
		}
		
		anim.SetBool ("AttackUp", false);
		anim.SetBool ("AttackDown", false);
		anim.SetBool ("AttackLeft", false);
		anim.SetBool ("AttackRight", false);
	}

	void playSpiderSound() {
		GetComponent<AudioSource>().PlayOneShot(spiderSound);
	}

	void playSwordSound() {
		GetComponent<AudioSource>().PlayOneShot(swordSound);
	}
}
