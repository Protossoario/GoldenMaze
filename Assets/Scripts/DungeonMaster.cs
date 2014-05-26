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
	int state;
	int turns; // contador de turnos del jugador que han progresado
	int enemyCount; // contador total de enemigos en el turno actual
	int enemiesFinished; // contador de enemigos que ya han terminado su turno
	bool gameOver;
	// Use this for initialization
	void Start () {
		state = 0;
		turns = 0;
		enemyCount = 0;
		gameOver = false;
		player = GameObject.FindGameObjectWithTag("Player");
		ps = player.GetComponent<PlayerScript>();
	}
	void startEnemyTurn()
	{
		GameObject[] spiders =GameObject.FindGameObjectsWithTag("Spider");
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
				state = 2;
				turns++;
			}
			break;
		case 2:
			enemiesFinished = 0;
			startEnemyTurn();
			state = 3;
			break;
		case 3:
			if (enemiesFinished == enemyCount) { // si ya terminaron de moverse todos los enemigos
				state = 0;
			}
			break;
		}
	}
}
