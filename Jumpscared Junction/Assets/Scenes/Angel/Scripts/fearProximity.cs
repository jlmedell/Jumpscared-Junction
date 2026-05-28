using UnityEngine;

public class fearProximity : MonoBehaviour
{
	//Fear settings
	public Transform monsterTransform;
	public float fearRadius = 5.0f;
	public float fearIncreaseRate = 2.0f;
	public float fearDecreaseRate = 1.0f;

	//Camera shake settings
	public float maxShakeIntensity = 0.15f;

	float currentFear = 0f;

	Transform playerCameraTransform;
	Vector3 originalCameraLocalPos;

	void Start()
	{
		playerCameraTransform = GetComponentInChildren<Camera>().transform;
		originalCameraLocalPos = playerCameraTransform.localPosition;
	}

	void Update()
	{
		updateFear();
		applyCameraShake();
	}

	//Updates fear based on distance to monster
	void updateFear()
	{
		if(monsterTransform == null)
		{
			currentFear = Mathf.MoveTowards(currentFear, 0f, (fearDecreaseRate * Time.deltaTime));
			return;
		}

		float distanceToMonster = Vector3.Distance(transform.position, monsterTransform.position);

		if(distanceToMonster <= fearRadius)
		{
			currentFear += (fearIncreaseRate * Time.deltaTime);
		}
		else
		{
			currentFear -= (fearDecreaseRate * Time.deltaTime);
		}

		currentFear = Mathf.Clamp01(currentFear);
	}

	//Applies camera shake based on fear amount
	void applyCameraShake()
	{
		if(playerCameraTransform == null)
		{
			return;
		}

		float shakeAmount = (currentFear * maxShakeIntensity);

		Vector3 shakeOffset = new Vector3(
			Random.Range(-shakeAmount, shakeAmount),
			Random.Range(-shakeAmount, shakeAmount),
			0f
		);

		playerCameraTransform.localPosition = (originalCameraLocalPos + shakeOffset);
	}

	//Expose fear value if other systems want it later
	public float getFearPercent()
	{
		return currentFear;
	}
}
