using UnityEngine;

public class exitZone : MonoBehaviour
{
	//Win condition when the player reaches the exit
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

		gameManager.instance.winGame();
	}
}
