using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Entity
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Coin coin;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float chaseRadius = 3.5f;

    private bool hasTarget = false;
    private float offset = 0.1f;
    [SerializeField] private float AttackPerSecond = 2f;
    private float AttackPerSecondCounter = 0f;
    [SerializeField] private float movementSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        if (Player.Instance != null && (Vector3.Distance(Player.Instance.transform.position, transform.position) <= chaseRadius))
        {
            agent.SetDestination(Player.Instance.transform.position);
            Vector3 direction = (Player.Instance.transform.position - transform.position).normalized;
            direction.y = 0; // keep only horizontal rotation

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            hasTarget = true;
        } else
        {
            hasTarget = false;
        }

        if (agent.velocity.magnitude != 0f)
        { 
            animator.SetBool("isWalking", true);
        } else
        {
            animator.SetBool("isWalking", false);
        }
    }

    override public void HandleAttack()
    {
        if (!hasTarget) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (AttackPerSecondCounter == 0f)
            {
                if (Vector3.Distance(Player.Instance.transform.position, transform.position) <= offset + agent.stoppingDistance)
                {
                    Player.Instance.Attacked(this);
                }
                animator.SetTrigger("attack");
            }

            AttackPerSecondCounter += Time.deltaTime;
        } else
        {
            if (AttackPerSecondCounter != 0f)
            {
                AttackPerSecondCounter += Time.deltaTime;
            }
        }

        if(AttackPerSecondCounter >= AttackPerSecond)
        {
            AttackPerSecondCounter = 0f;
        }
    }

    public override float GetAttackPerSecond()
    {
        return AttackPerSecond;
    }


    public Animator GetAnimator()
    {
        return animator;
    }

    public Coin GetCoin() => coin;
}
