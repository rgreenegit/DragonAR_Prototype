using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent (typeof (Collider))]

public class InteractiveObject : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {

	public enum FoodTypes
	{
		Apple,
		Rock,
		Sweets
	};


	public float touchScale = 1.1f;
	public bool moveable = false;
	public bool edible = false;
	public FoodTypes foodType;
	public float hideSpeed = 0.01f;

	private Vector3 scale;
	private Vector3 touchOffSet;
	private bool fedToChar = false; // deactivate drag and collisions
	private bool eaten = false; // activate scale down
	private bool beingDragged = false; // active feeding when true
	private Animator anim;

	// Lerp Scale
	public float lerpTime = 5f;
	private float currentLerpTime;
	private Vector3 startScale;
	private Vector3 endScale;


	void Awake(){
		anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
		startScale = transform.localScale;
		endScale = new Vector3(0f,0f,0f);
	}

	void Start(){
		scale = transform.localScale;
	}

	void Update(){

		// Lerp Scale
		if(eaten){
			
			currentLerpTime += Time.deltaTime;
			if(currentLerpTime > lerpTime){
				currentLerpTime = lerpTime;
			}

			float perc = currentLerpTime/lerpTime;
			transform.localScale = Vector3.Lerp(startScale, endScale, perc);
		}
	}

	#region IPointerDownHandler implementation

	public void OnPointerDown (PointerEventData eventData)
	{
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("Eat")){
			return;
		}

		if(!fedToChar){
			transform.localScale = Vector3.Scale(scale, new Vector3(touchScale, touchScale, touchScale));
		}
	}

	#endregion

	#region IPointerUpHandler implementation

	public void OnPointerUp (PointerEventData eventData)
	{
		transform.localScale = scale;
		touchOffSet = Vector3.zero;
		beingDragged = false;
	}

	#endregion

	#region IDragHandler implementation
	public void OnDrag (PointerEventData eventData)
	{
		if(!moveable || fedToChar || anim.GetCurrentAnimatorStateInfo(0).IsName("Eat")){
			return;
		} else {
			beingDragged = true;
			// CAST RAY AND CALCULATE GROUND TOUCH POSITION
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			Plane hPlane = new Plane(Vector3.up, Vector3.zero);
			float distance = 0; 

			if (hPlane.Raycast(ray, out distance)){

				Vector3 groundPos = ray.GetPoint(distance);

				if(touchOffSet == Vector3.zero){
					touchOffSet = transform.position - groundPos;
				}

				transform.position = groundPos + touchOffSet;
			}
		}
	}
	#endregion

	void consumeThisFood(){
		eaten = true;
	}

	void OnTriggerEnter(Collider other){
		
		if(!fedToChar && edible && beingDragged && other.name == "FoodCollider"){
			if(!anim.GetBool("Sleep")){
				fedToChar = true;
				anim.SetTrigger("Eat");
				Invoke("consumeThisFood", 1f);
			} else {
				fedToChar = true;
				anim.SetBool("Sleep", false);
				StartCoroutine(DelayEating());
			}
		}
	}

	IEnumerator DelayEating(){
		yield return new WaitForSeconds(5.0f);
		anim.SetTrigger("Eat");
		Invoke("consumeThisFood", 1f);
	}
}
