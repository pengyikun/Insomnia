using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ThirdPersonController : MonoBehaviour
{

    

    private ThirdPersonActionsAsset playerActionAsset;
    private InputAction move;

    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;

    private Animator animator;
    private PlayerStats playerStats;

    UIManager uiManager;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActionAsset = new ThirdPersonActionsAsset();
        animator = this.GetComponent<Animator>();
        playerStats = this.GetComponent<PlayerStats>();
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
        uiManager.UpdatePlayerInfoUI();
        InventoryManager.Instance.UpdateUI();
    }

    private void FixedUpdate()
    {
        // Iterating exisiting effects applied to player
        playerStats.IterateEffects();
        
        forceDirection += GetCameraRight(playerCamera) * (move.ReadValue<Vector2>().x * movementForce);
        forceDirection += GetCameraForward(playerCamera) * (move.ReadValue<Vector2>().y * movementForce);

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        // Increasing acceleration as the player falls
        if (rb.velocity.y < 0f)
        {
            rb.velocity -= Vector3.down * (Physics.gravity.y * Time.fixedDeltaTime);
        }

        // Capping player's horizontal speed
        // Debug.Log("Speed offset " + playerStats.calculateSpeedOffset().ToString());
        float speed = playerStats.maxSpeed * playerStats.CalculateSpeedOffset();
        // Movement logic
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > speed * speed)
        {
            rb.velocity = horizontalVelocity.normalized * speed + Vector3.up * rb.velocity.y;
            playerStats.ReduceStamina(playerStats.staminaConsumeRate * playerStats.CalculateStaminaConsumeOffset());
        }
        else
        {
            playerStats.AddStamina(playerStats.staminaRecoverRate);
        }

        LookAt();
        uiManager.UpdatePlayerStatsUI();
        uiManager.UpdatePlayerStatusEffectsUI();
    }

    private void OnEnable()
    {
        playerActionAsset.Player.Jump.started += DoJump;
        playerActionAsset.Player.Attack.started += DoAttack;
        playerActionAsset.Player.Interact.started += DoInteract;
        playerActionAsset.Player.UseItem.started += DoUseItem;
        playerActionAsset.Player.Inventory.started += DoToggleInventory;
        playerActionAsset.Player.SwitchEquippedItem.started += doSwitchEquippedItem;
        move = playerActionAsset.Player.Move;
        playerActionAsset.Player.Enable();

    }

    private void OnDisable()
    {
        playerActionAsset.Player.Jump.started -= DoJump;
        playerActionAsset.Player.Attack.started -= DoAttack;
        playerActionAsset.Player.Interact.started -= DoInteract;
        playerActionAsset.Player.UseItem.started -= DoUseItem;
        playerActionAsset.Player.Inventory.started -= DoToggleInventory;
        playerActionAsset.Player.SwitchEquippedItem.started -= doSwitchEquippedItem;
        playerActionAsset.Player.Disable();
    }
    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }


    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded() && playerStats.canJump)
        {
            playerStats.ReduceStamina(playerStats.jumpConsumeStaminaRate);
            forceDirection += Vector3.up * (playerStats.jumpForce * playerStats.CalculateJumpForceOffset());
        }
    }

    private void DoAttack(InputAction.CallbackContext obj)
    {
        if (uiManager.IsInteractiveUIOpened())
        {
            return;
        }
        playerStats.ReduceStamina(playerStats.attackConsumeStaminaRate);
        animator.SetTrigger("attack");
    }

    private void DoToggleInventory(InputAction.CallbackContext obj)
    {
        uiManager.TogglePlayerInfoUI();
    }
    private void doSwitchEquippedItem(InputAction.CallbackContext obj)
    {
        InventoryManager.Instance.SwitchEquippedItem();
    }

    private void DoInteract(InputAction.CallbackContext obj)
    {
        Debug.Log("Interact");
    }

    private void DoUseItem(InputAction.CallbackContext obj)
    {
        playerStats.ApplyEffect(playerStats.debugStatusEffectData);
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            return true;
        }
        return false;
    }

    public void OnDamaged(int damageAmount)
    {
        Debug.Log("Took a damage " + damageAmount);
    }


    public void UpdateInfoUIIfOpened()
    {
        if (uiManager.IsInteractiveUIOpened())
        {
            UIManager.Instance.UpdatePlayerInfoUI();
        }
    }



}
