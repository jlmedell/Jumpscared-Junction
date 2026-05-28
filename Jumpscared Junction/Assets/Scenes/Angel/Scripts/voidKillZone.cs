using UnityEngine;

public class voidKillZone : MonoBehaviour
{
	//Lose condition when the player falls off the map
	void OnTriggerEnter(Collider other)
	{
		if(!other.CompareTag("Player"))
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
