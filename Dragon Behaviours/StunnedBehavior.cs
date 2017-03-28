using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedBehavior : StateMachineBehaviour {

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){

        DragonManager dragonMgr = animator.GetComponent<DragonManager>();
		dragonMgr.stunCount += Time.deltaTime;

		if (dragonMgr.stunCount >= dragonMgr.stunRecovery) {
			animator.SetBool("Stunned", false);
			dragonMgr.stunCount=0;
			dragonMgr.tired=0;
		}

	}
}
