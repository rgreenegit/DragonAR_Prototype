using UnityEngine;
using System.Collections;

public class IdleBehavior : StateMachineBehaviour {

    public int Range;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        animator.SetInteger("IdleInt", Random.Range(1, Range));

		DragonManager dragonMgr = animator.GetComponent<DragonManager>();
      
		dragonMgr.tired++;

		if (dragonMgr.tired >= dragonMgr.goToSleep-1) {
			animator.SetBool("Sleep", true);
			dragonMgr.tired=0;
		}

	}
}