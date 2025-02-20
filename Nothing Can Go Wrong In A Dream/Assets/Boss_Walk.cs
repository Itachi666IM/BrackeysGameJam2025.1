using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Walk : StateMachineBehaviour
{
    public float speed;
    public float attackRange;
    Transform player;
    Rigidbody2D myRigidbody2D;
    Boss1 boss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindObjectOfType<Player>().transform;
        myRigidbody2D = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss1>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();
        Vector2 target = new Vector2(player.position.x, myRigidbody2D.position.y);
        Vector2 newPos = Vector2.MoveTowards(myRigidbody2D.position, target, speed * Time.fixedDeltaTime);
        myRigidbody2D.MovePosition(newPos);

        if(Vector2.Distance(target,myRigidbody2D.position)<= attackRange && boss.canAttack)
        {
            animator.SetTrigger("Attack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

    
}
