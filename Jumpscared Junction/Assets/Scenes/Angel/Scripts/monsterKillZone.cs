using UnityEngine;

public class monsterKillZone : MonoBehaviour
{
	//Lose condition when the monster touches the player
	void OnCollisionEnter(Collision other)
	{
		if(!other.gameObject.CompareTag("Player"))
		{
			return;
		}

		if(gameManager.instance == null)
		{
			return;
		}

		gameManager.instance.loseGame();
	}
}
