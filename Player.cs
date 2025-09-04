using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : Entity
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameInput gameInput;
    private float initialHealth = 100f;
    private int totalCoin = 0;
    private float moveSpeed = 1.2f;
    private float AttackPerSecond = 1f;
    private float AttackPerSecondCounter = 1f;

    public static Player Instance { get; private set; }
    private Vector3 lastInteractionDirection;
    private Furniture selectedFurniture;
    private float playerRadius = 0f;
    private bool isWalking;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gameInput.OnAttack += GameInput_Attack;
        gameInput.OnInteract += GameInput_Interact;
        SetInitialHealth(initialHealth);
        AssignWeapon();
    }

    private void GameInput_Attack(object sender, System.EventArgs e)
    {
        HandleAttack();
    }

    private void GameInput_Interact(object sender, System.EventArgs e)
    {
        HandleInteractAction();
    }

    private void HandleInteractAction()
    {
        selectedFurniture?.OnInteract();
        if (UpgradeSystem.Instance != null && UpgradeSystem.Instance.isInteract())
            if (UpgradeUI.Instance != null)
                UpgradeUI.Instance.ShowImage();

        foreach (SkeletonNPC skeleton in SkeletonNPCSpawner.Instance.GetSkeletonNPCs())
        {
            if (skeleton.CanInteract())
                skeleton.Gamble();
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleInteract();
        HandleAnimation();
        HandleNPCS();
        UpdateAttackTimer();
    }

    private void HandleNPCS()
    {
        if (SkeletonNPCSpawner.Instance == null)
            return;
        float interactionDistance = 2f;
        foreach (SkeletonNPC skeleton in SkeletonNPCSpawner.Instance?.GetSkeletonNPCs())
        {
            if (Vector3.Distance(this.transform.position, skeleton.transform.position) <= interactionDistance)
            {
                skeleton.Show();
            }
        }
    }

    private void HandleInteract()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // Get camera directions (flattened to XZ plane)
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        // Input as camera-relative
        Vector3 moveDirection = (cameraForward * inputVector.y + cameraRight * inputVector.x).normalized;

        if (moveDirection != Vector3.zero) lastInteractionDirection = moveDirection;

        float interactionDistance = 1f;
        if (Physics.Raycast(transform.position, lastInteractionDirection, out RaycastHit raycastHit, interactionDistance))
        {
            if (raycastHit.transform.TryGetComponent(out Furniture furniture))
            {
                selectedFurniture?.PlayerInteract(false);
                selectedFurniture = furniture;
                selectedFurniture.PlayerInteract(true);
            }
            else
            {
                selectedFurniture?.PlayerInteract(false);
                selectedFurniture = null;
            }
        }
        else
        {
            selectedFurniture?.PlayerInteract(false);
            selectedFurniture = null;
        }

        interactionDistance = 2f;
        UpgradeSystem.Instance?.SetInteract(Vector3.Distance(this.transform.position, UpgradeSystem.Instance.transform.position) <= interactionDistance && SceneManager.GetActiveScene().name == "SampleScene");
        
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // Get camera directions (flattened to XZ plane)
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        // Input as camera-relative
        Vector3 moveDirection = (cameraForward * inputVector.y + cameraRight * inputVector.x).normalized;

        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDirection,
            moveDistance
        );

        if (canMove && moveDirection != Vector3.zero)
            transform.position += moveDirection * moveDistance;

        isWalking = moveDirection != Vector3.zero;

        if (isWalking)
        {
            float rotateSpeed = 10f;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }


    private void HandleAnimation()
    {
        animator.SetBool("isWalking", isWalking);
    }

    override public void HandleAttack()
    {
        if (AttackPerSecondCounter == 0f)
        {
            GameManager.Instance.HitEnemy();
            animator.SetTrigger("attack");
        }
    }

    private void UpdateAttackTimer()
    {
        if (AttackPerSecondCounter > 0f)
        {
            AttackPerSecondCounter += Time.deltaTime;
        }

        if (AttackPerSecondCounter >= AttackPerSecond)
        {
            AttackPerSecondCounter = 0f;
        }
    }

    override public float GetInitialHealth()
    {
        return initialHealth;
    }

    public override float GetAttackPerSecond()
    {
        return AttackPerSecond;
    }

    public float GetRange()
    {
        return 0.8f;
    }

    public float GetDotRange()
    {
        return 0.75f;
    }

    public bool InRange(Enemy enemy)
    {
        Vector3 toTarget = (enemy.transform.position - transform.position).normalized;
        return Vector3.Dot(transform.forward, toTarget) >= GetDotRange() && Vector3.Distance(transform.position, enemy.transform.position) <= GetRange();
    }

    public void UpdateWeapon(Item item)
    {
        this.item = item;
        AssignWeapon();
    }

    public void CollectCoin(int amount)
    {
        totalCoin += amount;
        Dialogue.Instance.UpdateTotalCoin(totalCoin);
    }

    public int GetCoin() => totalCoin;

    public void UseCoin(int coin)
    {
        totalCoin -= coin;
        Dialogue.Instance.UpdateTotalCoin(totalCoin);
    }

    public void UpgradeWeapon()
    {
        UpdateWeapon(item.upgradedItem);
        initialHealth += item.health;
        SetInitialHealth(initialHealth);
    }

    public void ResetHealth()
    {
        SetInitialHealth(initialHealth);
        ResetHealthBar();
    }
}
