using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

[RequireComponent (typeof (Seeker))]
public class SpiderScript : MonoBehaviour {
	protected Seeker seeker;
	protected Transform tr;
	protected Transform playerTr;

	int damage;
	/* Directions:
	 * 0 = none
	 * 1 = up
	 * 2 = down
	 * 3 = left
	 * 4 = right
	 */
	int direction;
	int time; // tiempo que falta de ejecutar de la animacion actual
	int timeMax = 9; // tiempo maximo de una animacion de movimiento
	Animator anim;
	public int life = 2;
	float maxSpeed = 2f;
	bool turn; // variable que checa si es el turno de la arania de hacer un movimiento
	bool moving; // checa si la arania se esta moviendo
	bool requestingPath; // checa si la arania esta esperando a que el Seeker calcule el camino
	//bool aggressive; // sin utilizar aun

	// Use this for initialization
	void Start () {
		damage = 1;
		//aggressive = false;
		time = 0;
		direction = 0;
		anim = GetComponent<Animator>();
		turn = false;
		moving = false;
		requestingPath = false;
		seeker = GetComponent<Seeker>();
		tr = GetComponent<Transform>();
		playerTr = GameObject.FindGameObjectWithTag("Player").transform;
	}

	public void startTurn() {
		turn = true;
	}

	public void setLife(int l) {
		life = l; 
	}

	public int getLife() {
		return life;
	}
	public  void reduceLife(int dam) {
		if (anim.GetBool ("SpiderUp")) {
			anim.SetBool("DamageUp", true);
			anim.SetBool("DamageDown", false);
			anim.SetBool("DamageLeft", false);
			anim.SetBool("DamageRight", false);
		} else if (anim.GetBool ("SpiderDown")) {
			anim.SetBool("DamageUp", false);
			anim.SetBool("DamageDown", true);
			anim.SetBool("DamageLeft", false);
			anim.SetBool("DamageRight", false);
		} else if (anim.GetBool ("SpiderLeft")) {
			anim.SetBool("DamageUp", false);
			anim.SetBool("DamageDown", false);
			anim.SetBool("DamageLeft", true);
			anim.SetBool("DamageRight", false);
		} else if (anim.GetBool ("SpiderRight")) {
			anim.SetBool("DamageUp", false);
			anim.SetBool("DamageDown", false);
			anim.SetBool("DamageLeft", false);
			anim.SetBool("DamageRight", true);
		}

		life -= dam;
	}
	/*
	void checkPlayer()
	{ float monsterPositionX = this.transform.position.x;
	  float monsterPositionY = this.transform.position.y;
		if(GameObject.FindGameObjectWithTag("Player").transform.position.x<=monsterPositionX+tilesize*vision
		   ||GameObject.FindGameObjectWithTag("Player").transform.position.x>=monsterPositionX-tilesize*vision)
			agressive = true;
		if(GameObject.FindGameObjectWithTag("Player").transform.position.y<=monsterPositionY+tilesize*vision
		   ||GameObject.FindGameObjectWithTag("Player").transform.position.y>=monsterPositionY-tilesize*vision)
			agressive = true;


	} */

	// Funcion que se llama cuando el Seeker termina de calcular el camino hacia el jugador
	void onPathComplete(Path p) {
		if (p.error) {
			direction = 0;
			moving = true;
			requestingPath = false;
			return;
		}
		List<Vector3> path = p.vectorPath;
		Vector3 dirVec = path[1] - path[0]; // vector del siguiente segmento del camino
		// se selecciona una de las cuatro direcciones en base al mayor componente del vector dirVec
		if (Mathf.Abs(dirVec.x) < Mathf.Abs (dirVec.y)) {
			if (dirVec.y > 0) {
				direction = 1; // arriba
				anim.SetBool("SpiderUp", true);
				anim.SetBool("SpiderDown", false);
				anim.SetBool("SpiderLeft", false);
				anim.SetBool("SpiderRight", false);
			}
			else {
				direction = 2; // abajo
				anim.SetBool("SpiderUp", false);
				anim.SetBool("SpiderDown", true);
				anim.SetBool("SpiderLeft", false);
				anim.SetBool("SpiderRight", false);
			}
		}
		else {
			if (dirVec.x > 0) {
				direction = 4; // derecha
				anim.SetBool("SpiderUp", false);
				anim.SetBool("SpiderDown", false);
				anim.SetBool("SpiderLeft", false);
				anim.SetBool("SpiderRight", true);
			}
			else {
				direction = 3; // izquierda
				anim.SetBool("SpiderUp", false);
				anim.SetBool("SpiderDown", false);
				anim.SetBool("SpiderLeft", true);
				anim.SetBool("SpiderRight", false);
			}
		}
		moving = true;
		requestingPath = false;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (direction == 0 && turn && !requestingPath) { // Al inicio del turno no tiene direccion
			Collider2D colUp = Physics2D.OverlapPoint(new Vector2(this.transform.position.x, this.transform.position.y + 0.32f));
			Collider2D colDown = Physics2D.OverlapPoint(new Vector2(this.transform.position.x, this.transform.position.y - 0.32f));
			Collider2D colLeft = Physics2D.OverlapPoint(new Vector2(this.transform.position.x - 0.32f, this.transform.position.y));
			Collider2D colRight = Physics2D.OverlapPoint(new Vector2(this.transform.position.x + 0.32f, this.transform.position.y));
			// Checar en las 4 direcciones si esta el jugador a un lado
			if (colUp != null && colUp.CompareTag("Player")) {
				direction = 1;
				anim.SetBool("SpiderUp", true);
				anim.SetBool("SpiderDown", false);
				anim.SetBool("SpiderLeft", false);
				anim.SetBool("SpiderRight", false);
				colUp.gameObject.GetComponent<PlayerScript>().reduceLife(damage);
				HealthControl.lives--;
			}
			else if (colDown != null && colDown.CompareTag("Player")) {
				direction = 2;
				anim.SetBool("SpiderUp", false);
				anim.SetBool("SpiderDown", true);
				anim.SetBool("SpiderLeft", false);
				anim.SetBool("SpiderRight", false);
				colDown.gameObject.GetComponent<PlayerScript>().reduceLife(damage);
				HealthControl.lives--;
			}
			else if (colLeft != null && colLeft.CompareTag("Player")) {
				direction = 3;
				anim.SetBool("SpiderUp", false);
				anim.SetBool("SpiderDown", false);
				anim.SetBool("SpiderLeft", true);
				anim.SetBool("SpiderRight", false);
				colLeft.gameObject.GetComponent<PlayerScript>().reduceLife(damage);
				HealthControl.lives--;
			}
			else if (colRight != null && colRight.CompareTag("Player")) {
				direction = 4;
				anim.SetBool("SpiderUp", false);
				anim.SetBool("SpiderDown", false);
				anim.SetBool("SpiderLeft", false);
				anim.SetBool("SpiderRight", true);
				colRight.gameObject.GetComponent<PlayerScript>().reduceLife(damage);
				HealthControl.lives--;
			}
			// Si no se ataca al jugador, checar el movimiento
			else {
				/* Obtener un camino del objeto Seeker */
				seeker.StartPath(tr.position, playerTr.position, onPathComplete);
				time = timeMax;
				requestingPath = true;
			}
		}
		// El turno se esta ejecutando y la arania se esta desplazando de un tile a otro
		else if (turn) {
			if (moving) {
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
			}

			if (time <= 0) {
				direction = 0;
				turn = false;
				if (moving) {
					moving = false;
					AstarPath.active.UpdateGraphs(GetComponent<SpriteRenderer>().bounds);
				}
				rigidbody2D.velocity = new Vector2 (0f, 0f);
				GameObject dm = GameObject.Find("Dungeon Master");
				DungeonMaster dmScript = dm.GetComponent<DungeonMaster>();
				dmScript.notifyTurnFinish();
			}
		}
	}

	void turnOffDamageAnimation() {
		anim.SetBool ("DamageUp", false);
		anim.SetBool ("DamageDown", false);
		anim.SetBool ("DamageLeft", false);
		anim.SetBool ("DamageRight", false);
	}
}
