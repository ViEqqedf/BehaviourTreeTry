using BehaviourTree.Nodes;
using BehaviourTree.Precondition;
using UnityEngine;

public class PlayAnimation : BTAction {
    private string animationName;


    public PlayAnimation (string animationName, BTPrecondition precondition = null) :
        base ("PlayAnimation", precondition) {
        this.animationName = animationName;
    }

    protected override void Enter () {
        Animator animator = btContext.GetComponent<Animator>();
        animator.Play(animationName);
    }
}