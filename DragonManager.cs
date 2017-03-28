using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragonManager : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {

	public int tired = 0; // tired counter in units:idle anim cycle counts
	public float stunCount = 0;// stunned counter in units: frames
	public int stunRecovery = 5; // number of seconds before recovery
	public int goToSleep; // number of idle anim cycles before sleeping
	public float sleepCount = 0; // sleep counter when sleeping in units:frames
	public int maxSleepTime; // max sleep time in units:seconds
	public int health = 10; // editor health setting in units: number of hits before stunned
	public ParticleSystem breathFireParticle;

	private Animator anim;
	private int characterHealth; // health applied from editor - for resetting

	void Start () {
		anim = GetComponent<Animator>();
		characterHealth = health;
	}

	void Update () {
		if (anim.GetBool("Sleep")){
			sleepCount += Time.deltaTime;
		} else {
			sleepCount = 0;
		}

		if (sleepCount > maxSleepTime){
			anim.SetBool("Sleep", false);
		}

		// Landing
		anim.SetFloat("Char TY", transform.position.y);
	}

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{

        if (anim.GetBool("Sleep"))
        {
            anim.SetBool("Sleep", false);
        } else if(anim.GetBool("Pet")) {
			anim.SetBool("Pet", false);
        } else if(!anim.GetBool("Stunned") && !anim.GetBool("Fly")) {
        	anim.Play("Take_Hit");
        	characterHealth--;
        	if(characterHealth < 0){
        		characterHealth = health;
				anim.SetBool("Stunned", true);
        	}
        }
	}

	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		// WIP: petting test
	}

	#endregion

	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{
		if(!anim.GetBool("Pet"))
		{
			anim.SetBool("Pet", true);
		}

	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		if(anim.GetBool("Pet")) {
			anim.SetBool("Pet", false);
		}
	}

	#endregion

    public void ResetCharacter(){
		Debug.Log("Reset");
    	gameObject.transform.position = new Vector3(0, 0, 0);

    }

	void OnDisable(){
		ResetCharacter();
	}

	public void ToggleActive(){
		gameObject.SetActive(!gameObject.activeSelf);
	}

	public void BreathFire(bool fire){
		if(!anim.GetBool("Sleep") 
		&& !anim.GetBool("Entrance") 
		&& !anim.GetBool("Stunned")
		&& !anim.GetBool("Fly")){
			if(fire){
				anim.Play("Dragon_Breath Fire Start");
			} 
			anim.SetBool("BreathingFire", fire);
		}
	}

	public void StartFireParticles(){
		breathFireParticle.Play();
	}

	public void StopFireParticles(){
		if(breathFireParticle.isPlaying){
			breathFireParticle.Stop();
		} 
	}

	public void Sleep(){
		if(!anim.GetBool("BreathingFire") 
		&& !anim.GetBool("Entrance") 
		&& !anim.GetBool("Stunned")
		&& !anim.GetBool("Fly")){
			if(!anim.GetBool("Sleep")){
				anim.Play("Dragon_Sleep Enter");
				anim.SetBool("Sleep", true);
			} else {
				anim.SetBool("Sleep", false);
			}
		}
	}

	public void Fly(){
		if(!anim.GetBool("Sleep") 
		&& !anim.GetBool("Entrance") 
		&& !anim.GetBool("Stunned")){
			if(!anim.GetBool("Fly")){
				anim.SetBool("Fly", true);
				anim.Play("Fly Take off");
			} else {
				anim.SetBool("Fly", false);
			}
		}
	}

	public void ResetYOnLand(){
		StartCoroutine(ResetY());
	}

	IEnumerator ResetY(){
		float speed= 0.2f; // seconds
		float elapsedTime = 0;
		while (elapsedTime < speed){
			float currentTY = transform.position.y;
			float endTY = 0.044f;

			float newTY = Mathf.Lerp(currentTY, endTY, elapsedTime/speed);

			transform.position = new Vector3(transform.position.x, newTY, transform.position.z);

			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}

}
