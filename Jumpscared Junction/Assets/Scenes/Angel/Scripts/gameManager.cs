using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public enum gameState
{
	playing,
	win,
	lose
}

public class gameManager : MonoBehaviour
{
	public static gameManager instance { get; private set; }

	//Current game state
	public gameState currentState = gameState.playing;

	//Optional UI panels
	public GameObject winPanel;
	public GameObject losePanel;

	//Jumpscare
	public GameObject jumpscarePanel;
	public AudioClip jumpscareSound;
	public float jumpscareDuration = 1.5f;

	private AudioSource audioSource;

	void Awake()
	{
		if((instance != null) && (instance != this))
		{
			Destroy(gameObject);
			return;
		}

		instance = this;

		audioSource = GetComponent<AudioSource>();
		if(audioSource == null)
		{
			audioSource = gameObject.AddComponent<AudioSource>();
		}

		//Reset time scale for play mode
		Time.timeScale = 1f;

		//Hide UI at start
		if(winPanel != null)
		{
			winPanel.SetActive(false);
		}

		if(losePanel != null)
		{
			losePanel.SetActive(false);
		}

		if(jumpscarePanel != null)
		{
			jumpscarePanel.SetActive(false);
		}
	}

	void Update()
	{
		if(Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
		{
			restartScene();
		}
	}


	//This function is used to transition the game into a win state.
	public void winGame()
	{
		if(currentState != gameState.playing)
		{
			return;
		}

		currentState = gameState.win;
		Time.timeScale = 0f;

		if(winPanel != null)
		{
			winPanel.SetActive(true);
		}

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		Debug.Log("WIN");
	}


	//This function is used to transition the game into a lose state.
	//Shows a jumpscare before revealing the lose panel.
	public void loseGame()
	{
		if(currentState != gameState.playing)
		{
			return;
		}

		currentState = gameState.lose;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		StartCoroutine(jumpscareSequence());

		Debug.Log("LOSE");
	}

	private IEnumerator jumpscareSequence()
	{
		//Show jumpscare image and play sound
		if(jumpscarePanel != null)
		{
			jumpscarePanel.SetActive(true);
		}

		if(jumpscareSound != null)
		{
			audioSource.PlayOneShot(jumpscareSound);
		}

		//Wait in real time so timeScale = 0 doesn't block it
		yield return new WaitForSecondsRealtime(jumpscareDuration);

		//Hide jumpscare, freeze time, show lose panel
		if(jumpscarePanel != null)
		{
			jumpscarePanel.SetActive(false);
		}

		Time.timeScale = 0f;

		if(losePanel != null)
		{
			losePanel.SetActive(true);
		}
	}


	//This function reloads the active scene.
	public void restartScene()
	{
		Time.timeScale = 1f;

		int sceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(sceneIndex);
	}
}
