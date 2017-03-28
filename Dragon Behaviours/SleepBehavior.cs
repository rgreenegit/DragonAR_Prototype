using System.Collections;
using UnityEngine;

public class SleepBehavior : StateMachineBehaviour {

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
  		DragonManager dragonMgr = animator.GetComponent<DragonManager>();

  		dragonMgr.sleepCount++;

		if (dragonMgr.sleepCount >= dragonMgr.maxSleepTime-1) {
			animator.SetBool("Sleep", false);
			dragonMgr.sleepCount=0;
		}

	}

}
