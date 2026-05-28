using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class staminaSystem : MonoBehaviour
{
	//Stamina settings
	public float maxStamina = 3.0f;
	public float staminaDrainRate = 3.0f;
	public float staminaRegenRate = 0.8f;

	//Exhaustion settings
	public float exhaustedThreshold = 0.05f;
	public float recoveredThreshold = 0.6f;

	public bool isExhausted = false;

	//Optional UI
	public Slider staminaBar;

	float currentStamina = 0f;

	void Start()
	{
		currentStamina = maxStamina;
		updateUi();
	}

	void Update()
	{
		updateUi();
	}

	//Returns true if sprint is requested and allowed by stamina and exhaustion
	
	public bool isSprintingRequested()
	{
		if(Keyboard.current == null)
		{
			return false;
		}

		if(isExhausted)
		{
			return false;
		}

		if(!Keyboard.current.leftShiftKey.isPressed)
		{
			return false;
		}

		return (currentStamina > 0f);
	}

	//Updates stamina and exhaustion state
	public void updateStamina(bool isSprinting)
	{
		if(isSprinting)
		{
			currentStamina -= (staminaDrainRate * Time.deltaTime);
		}
		else
		{
			currentStamina += (staminaRegenRate * Time.deltaTime);
		}

		currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

		//Enter exhausted when empty
		if(!isExhausted && (currentStamina <= exhaustedThreshold))
		{
			isExhausted = true;
		}

		//Exit exhausted when mostly recovered
		if(isExhausted && (getStaminaPercent() >= recoveredThreshold))
		{
			isExhausted = false;
		}
	}

	public float getStaminaPercent()
	{
		if(maxStamina <= 0f)
		{
			return 0f;
		}

		return Mathf.Clamp01(currentStamina / maxStamina);
	}

	public float getCurrentStamina()
	{
		return currentStamina;
	}

	void updateUi()
	{
		if(staminaBar == null)
		{
			return;
		}

		staminaBar.maxValue = maxStamina;
		staminaBar.value = currentStamina;
	}
}
