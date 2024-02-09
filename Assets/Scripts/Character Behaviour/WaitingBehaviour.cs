using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private float _timeUntilBored;

    [SerializeField]
    private int _numberOfWaitingAnimations;

    private bool _isBored;
    private float _idleTime;
    private int _waitingAnimation;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_isBored == false)
        {
            _idleTime += Time.deltaTime;

            if (_idleTime > _timeUntilBored && stateInfo.normalizedTime % 1 < 0.02f)
            {
                _isBored = true;
                _waitingAnimation = Random.Range(1, _numberOfWaitingAnimations + 1);
                _waitingAnimation = _waitingAnimation * 2 - 1;

                animator.SetFloat("WaitingAnimation", _waitingAnimation - 1);
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }

        animator.SetFloat("WaitingAnimation", _waitingAnimation, 0.2f, Time.deltaTime);

    }

    private void ResetIdle()
    {
        if (_isBored)
        {
            _waitingAnimation--;
        }

        _isBored = false;
        _idleTime = 0;
    }
}
