using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        GameManager.Instance.player.Shoot();
    }
}
