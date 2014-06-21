using UnityEngine;
using System.Collections;

public class DungeonMaster : MonoBehaviour {
	GameObject player;
	PlayerScript ps;
	/* States:
	 * 0 = starting player's turn
	 * 1 = player's turn
	 * 2 = starting enemy turn
	 * 3 = enemy turn
	 * 4 = player phase GUI 
	 */
	int state;
	int turns; // contador de turnos del jugador que han progresado
	int timer;
	int enemyCount; // contador total de enemigos en el turno actual
	int enemiesFinished; // contador de enemigos que ya han terminado su turno
	bool gameOver;
	public bool waitingOnEnemy;
	public int playerMoves;
	public GUIText theMoves;
	public GUIText plaPhase;
	public GUIText enePhase;
	// Use this for initialization
	private GameObject[] spiderList;
	void Start () {
		state = 0;
		turns = 0;
		enemyCount = 0;
		gameOver = false;
		player = GameObject.FindGameObjectWithTag("Player");
		ps = player.GetComponent<PlayerScript>();
		playerMoves = 1;
		enemiesFinished = 0;
		waitingOnEnemy = false;
	}
	void startEnemyTurn()
	{
		GameObject[] spiders = GameObject.FindGameObjectsWithTag("Spider");
		enemyCount = spiders.Length;
		for(int i = 0; i < spiders.Length; i++)
		{
			SpiderScript spiderScr = spiders[i].GetComponent<SpiderScript>();
			if (spiderScr != null) {
				spiderScr.startTurn();
			}
		}
	}
	public void notifyTurnFinish() {
		waitingOnEnemy = false;
		enemiesFinished++;
	}
	void updateTraps()
	{
		/*GameObject[] traps= GameObject.FindGameObjectsWithTag("Trap");
		for(int i=0;i<traps.Length;i++)
		{
			TrapScript ts =(EnemyScript) traps[i].GetComponent(typeof(TrapScript));
			// to do llamar al metodo del turno de las trampas.
		}*/
	}
	void setPhase()
	{	if(state == 4)
			plaPhase.text = "Player Phase";
		else if(state == 5)
			enePhase.text = "Enemy Phase";
	}
	void checkItems()
	{
		/*GameObject[] items= GameObject.FindGameObjectsWithTag("Items");
		for(int i =0;i<items.Length;i++)
		{
			ItemScript it =(EnemyScript) items[i].GetComponent(typeof(ItemScript));
			// to do llamar al metodo del turno del enemigo
		}*/
	}
	// Update is called once per frame
	void Update () {
		theMoves.text = "Moves: " + (playerMoves + 1);
		switch (state) {
		case 0:
			plaPhase.text = "";
			enePhase.text ="";
			ps.startTurn();
			state = 1;
			break;
		case 1:
			if (!ps.getTurn()) { // si ya termino el turno del jugador
				if(playerMoves < 1)  {
					state = 2;
					playerMoves--;
					timer = 30;
				}
				else {
					playerMoves--;
					state = 0;
				}
				turns++;
			}
			enemiesFinished = 0;
			spiderList = GameObject.FindGameObjectsWithTag("Spider");
			enemyCount = spiderList.Length;
			break;
		case 2:

			enePhase.text ="";
			if(enemyCount > 0){
			waitingOnEnemy = true;
			spiderList[enemiesFinished].GetComponent<SpiderScript>().startTurn();
			}
			state = 3;
			break;
		case 3:
			if (!waitingOnEnemy)  // si ya terminaron de moverse todos los enemigos
				if(enemiesFinished == enemyCount){
					state = 0;
					playerMoves = 1;
					timer = 30;
				}
				else{
					state = 2;
				}

			
			break;
		case 4:
			setPhase();
			if(timer>0){
				timer--;
			}
			else {
				state = 0;
			}
			break;
		case 5:
			setPhase();
			if(timer>0){
				timer--;
			}
			else {
				state = 2;
			}
			break;

					

		}
	}
}
