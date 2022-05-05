using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private ThirdPersonActionsAsset playerActionAsset;
    private CharacterController characterController;
    private InputAction move;
    
    private Animator animator;
    private Vector2 currentInput;
    
    private PlayerStats playerStats;
    private PlayerWeaponLogic playerWeaponLogic;
    private UIManager uiManager;
    private InventoryManager inventoryManager;

    private Vector3 rootMotion;
    
    public float jumpHeight;
    public float gravity;
    public float stepDownForce;
    public float airControl;
    public float jumpDamp;
    public float maxSpeed;
    public float pushPower = 2.0F;

    
    private Vector3 velocity;
    private bool isJumping;

    public ParticleSystem levelUpParticle;
    public ParticleSystem questCompleteParticle;

    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private CinemachineVirtualCamera subCamera;
    [SerializeField] private CinemachineVirtualCamera menuCamera;

    private void Awake()
    {
        playerActionAsset = new ThirdPersonActionsAsset();
        playerStats = GetComponent<PlayerStats>(); 
        Cursor.visible = false;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerWeaponLogic = GetComponent<PlayerWeaponLogic>();
        animator = GetComponent<Animator>();
        uiManager = UIManager.Instance;
        uiManager.UpdatePlayerInfoUI();
        inventoryManager = InventoryManager.Instance;
        inventoryManager.UpdateUI();
        CameraManager.Instance.AddCamera(mainCamera);
        CameraManager.Instance.AddCamera(subCamera);
        CameraManager.Instance.AddCamera(menuCamera);
        CameraManager.Instance.SetFocusCamera(mainCamera.name);
        menuCamera.gameObject.SetActive(true);
        uiManager.ToggleQuestPanelUI();
    }
    
    private void OnEnable()
    {
        playerActionAsset.Player.Jump.started += DoJump;
        playerActionAsset.Player.Attack.started += DoAttack;
        playerActionAsset.Player.Interact.started += DoInteract;
        playerActionAsset.Player.UseItem.started += DoUseItem;
        playerActionAsset.Player.Inventory.started += DoToggleInventory;
        playerActionAsset.Player.Quest.started += DoToggleQuestPanel;
        playerActionAsset.Player.SwitchEquippedItem.started += doSwitchEquippedItem;
        playerActionAsset.Player.SwitchWeaponMode.started += DoSwitchWeaponMode;
        playerActionAsset.Player.Reload.started += DoReload;
        playerActionAsset.Player.NextWave.started += DoNextWave;
        playerActionAsset.Player.SwitchCamera.started += DoSwitchCamera;
        playerActionAsset.Player.Menu.started += DpOpenMenu;
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
        playerActionAsset.Player.Quest.started -= DoToggleQuestPanel;
        playerActionAsset.Player.SwitchEquippedItem.started -= doSwitchEquippedItem;
        playerActionAsset.Player.SwitchWeaponMode.started -= DoSwitchWeaponMode;
        playerActionAsset.Player.Reload.started -= DoReload;
        playerActionAsset.Player.NextWave.started -= DoNextWave;
        playerActionAsset.Player.SwitchCamera.started -= DoSwitchCamera;
        playerActionAsset.Player.Menu.started -= DpOpenMenu;
        playerActionAsset.Player.Disable();
    }

    private void Update()
    {
        

        // Getting input data
        currentInput.x = Input.GetAxis("Horizontal");
        currentInput.y = Input.GetAxis("Vertical");
        // input.x = move.ReadValue<Vector2>().x;
        // input.y = move.ReadValue<Vector2>().y;
        // Debug.Log($"x: {currentInput.x}, y: {currentInput.y}");
        
        // Calculating movement data based on status effects and inventory weight
        float speedOffset = playerStats.CalculateSpeedOffset();
        
        // Animator
        animator.SetFloat("x", currentInput.x * speedOffset);
        animator.SetFloat("y", currentInput.y * speedOffset);
        
        
    }

    private void FixedUpdate()
    {
        // Iterating exisiting effects applied to player
        playerStats.IterateEffects();
        
        // Differentiate movement base on jump state
        if (isJumping)
        {
            // Movement logic on air
            AirMovement(); 
        }
        else
        {
            //  Movement logic default
            GroundMovement();
        }

        // Consume or Recover Stamina
        if (currentInput.x != 0 || currentInput.y != 0)
        {
            playerStats.ReduceStamina(playerStats.staminaConsumeRate * playerStats.CalculateStaminaConsumeOffset());
        }
        else
        {
            playerStats.AddStamina(playerStats.staminaRecoverRate);
        }
        
        // Update UI
        uiManager.UpdatePlayerStatsUI();
    }

    void GroundMovement()
    {
        Vector3 stepDownAmount = Vector3.down * stepDownForce;
        Vector3 stepForwardAmount = rootMotion * maxSpeed;
        characterController.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;

        if (!characterController.isGrounded)
        {
            SetInAir(0);
        } 
    }

    void AirMovement()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 displacement = velocity * Time.deltaTime;
        displacement += CalculateAirControl();
        characterController.Move(displacement);
        isJumping = !characterController.isGrounded;
        rootMotion = Vector3.zero; 
        animator.SetBool("isOnAir", isJumping);
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (!isJumping && IsGrounded() && playerStats.canJump)
        {
            //Debug.Log("Jump");
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            playerStats.ReduceStamina(playerStats.jumpConsumeStaminaRate);
            SetInAir(jumpVelocity);
            // forceDirection += Vector3.up * (playerStats.jumpForce * playerStats.CalculateJumpForceOffset());
        }
    }

    private void DoAttack(InputAction.CallbackContext obj)
    {
        if (uiManager.IsInteractiveUIOpened())
        {
            return;
        }
        // playerStats.ReduceStamina(playerStats.attackConsumeStaminaRate);
        // animator.SetTrigger("attack");
    }

    private void DoToggleInventory(InputAction.CallbackContext obj)
    {
        uiManager.TogglePlayerInfoUI();
    }
    
    private  void DoToggleQuestPanel(InputAction.CallbackContext obj)
    {
        uiManager.ToggleQuestPanelUI();
    }
    private void doSwitchEquippedItem(InputAction.CallbackContext obj)
    {
        InventoryManager.Instance.SwitchEquippedItem();
    }
    
    private void DoSwitchWeaponMode(InputAction.CallbackContext obj)
    {
        Debug.Log("Switch weapon mode!");
        playerStats.SetWeaponMode(playerStats.GetWeaponMode() == 0 ? 1 : 0);
        playerWeaponLogic.AdjustWeaponConfig(); 
    } 

    private void DoInteract(InputAction.CallbackContext obj)
    {
        Debug.Log("Interact");
        QuestManager.Instance.CompleteQuest(1);
    }
    
    private void DoReload(InputAction.CallbackContext obj)
    {
        Debug.Log("Force Reload");
        playerStats.ReloadMag(); 
    }

    private void DoUseItem(InputAction.CallbackContext obj)
    {
        //playerStats.ApplyEffect(playerStats.debugStatusEffectData);
        inventoryManager.UseItem(inventoryManager.EquippedItem);
        //WaveManager.Instance.StartWave();
        // uiManager.PushNotification("You have some new quests..");
    }
    
    private void DoNextWave(InputAction.CallbackContext obj)
    {
        WaveManager.Instance.StartWave();
    }

    public void DoSwitchCamera(InputAction.CallbackContext obj)
    {
        if (CameraManager.Instance.IsFocusCamera(mainCamera.name))
        {
            CameraManager.Instance.SetFocusCamera(subCamera.name);
        }
        else
        {
            CameraManager.Instance.SetFocusCamera(mainCamera.name); 
        }
    }
    
    public void DpOpenMenu(InputAction.CallbackContext obj)
    {
        OpenMenu(); 
    }

    public void OpenMenu()
    {
        if (uiManager.instructionsMenuUI.activeSelf)
        {
            return;  
        }
        uiManager.ToggleInGameMenuUI();
        if (uiManager.InGameMenuUI.activeSelf)
        {
            CameraManager.Instance.SetFocusCamera(menuCamera.name);
        }
        else
        {
            CameraManager.Instance.SetFocusCamera(mainCamera.name); 
        } 
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
        if (!playerStats.ReduceHealth(damageAmount))
        {
            playerStats.gameStates = "dead";
            OnFailed();
        };
    }

    void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = animator.velocity * jumpDamp * maxSpeed;
        velocity.y = jumpVelocity;
        animator.SetBool("isOnAir", isJumping);
    }
    
    private Vector3 CalculateAirControl()
    {
        var transform1 = transform;
        return ((transform1.forward * currentInput.y) + (transform1.right * currentInput.x)) * (airControl / 100);
    }
    
    public void UpdateInfoUIIfOpened()
    {
        if (uiManager.IsInteractiveUIOpened())
        {
            UIManager.Instance.UpdatePlayerInfoUI();
        }
    }

    public void PlayParticle(string type)
    {
        switch (type)
        {
            case "levelUp":
                levelUpParticle.Play();
                break;
            case "questComplete":
                questCompleteParticle.Play();
                break;
        }
        
    }

    public void RestartLevel()
    {
        Debug.Log("RestartLevel");
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
    
    public void ExitGame()
    {
        Debug.Log("ExitGame");
        //Application.Quit();
        SceneManager.LoadScene(0);
    }


    public void OnFailed()
    {
        uiManager.ToggleOverMenu();
    }

    public void OnSuccess()
    {
        playerStats.gameStates = "success";
        uiManager.ToggleOverMenu(); 
    }
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log($"Hit {hit.gameObject.name} : {hit.gameObject.tag}");
        if (hit.gameObject.CompareTag("Item"))
        {
            hit.gameObject.GetComponent<ICollectable>().OnInteract();
            return;
        }
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3f)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }
}

