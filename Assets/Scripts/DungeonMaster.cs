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
	 */
	public int state;
	public int turns; // contador de turnos del jugador que han progresado
	public int enemyCount; // contador total de enemigos en el turno actual
	public int playerMoves;
	public int enemiesFinished;
	public bool waitingOnEnemy;
	// Use this for initialization
	private GameObject[] spiderList;
	void Start () {
		state = 0;
		turns = 0;
		enemyCount = 0;
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
		switch (state) {
		case 0:
			ps.startTurn();
			state = 1;
			break;
		case 1:
			if (!ps.getTurn()) { // si ya termino el turno del jugador
				if(playerMoves < 1)  {
					state = 2;
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
			if (enemyCount > 0) {
				waitingOnEnemy = true;
				spiderList[enemiesFinished].GetComponent<SpiderScript>().startTurn();
			}
			state = 3;
			break;
		case 3:
			if (!waitingOnEnemy) {
				if (enemiesFinished == enemyCount) { // si ya terminaron de moverse todos los enemigos
					state = 0;
					playerMoves = 1;
				}
				else {
					state = 2;
				}
			}
			break;
		}
	}
}
