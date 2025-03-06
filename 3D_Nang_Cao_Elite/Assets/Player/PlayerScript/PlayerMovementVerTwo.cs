/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerMovementVerTwo : MonoBehaviour
{
    [SerializeField]
    private float maximumSpeed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float jumpSpeed;

    [SerializeField]
    private float jumpBufferTime = 0.2f;

    [SerializeField]
    private float jumpButtonGracePeriod;

    [SerializeField]
    private Transform cameraTransform;

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    [Header("Attack Settings")]
    public float attackDamage = 25f;
    public float attackRange = 2f;
    public float attackCooldown = 0.5f;
    private float lastAttackTime;
    private int attackHash = Animator.StringToHash("Attack");
    private bool isAttacking = false;

    [Header("VFX")]
    public GameObject healEffectPrefab;

    [Header("Health")]
    public Slider healthSlider;
    public float maxHealth = 200f;
    public float health;

    public float regenHealPerSecond = 5f;
    public float regenDuration = 10f;

    [Header("InventoryUI")]
    public GameObject inventory;
    public Button inventoryButton;
    public Button reverseInventoryButton;
    public KeyCode inventoryKey = KeyCode.E;
    private bool inventoryOpen = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip moveSound;
    public AudioClip attackSound;
    public AudioClip hurtSound;


    [Header("CursorToggle")]
    public KeyCode toggleKeyCursorLock = KeyCode.F;
    private bool cursorLocked = false;
    private int enemyKillCount = 0;
    public static PlayerMovementVerTwo Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        health = maxHealth;
        inventory.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (inventoryButton != null)
        {
            inventoryOpen = inventoryButton.gameObject.activeInHierarchy; // Check if the GameObject is active at start
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking) return;
         
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude /= 2;
        }

        animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);

        float speed = inputMagnitude * maximumSpeed;
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
            animator.SetBool("Jump", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
            animator.SetBool("Jump", true);
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;

            if (jumpButtonPressedTime.HasValue && Time.time - jumpButtonPressedTime <= jumpBufferTime)
            {
                ySpeed = jumpSpeed;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }

        Vector3 velocity = movementDirection * speed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
           

            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            if (!audioSource.isPlaying) // Chỉ phát nếu chưa có âm thanh đang chạy
            {
                audioSource.clip = moveSound;
                audioSource.loop = true; // Loop cho âm thanh di chuyển
                audioSource.Play();
            }
        }
        else
        {
            audioSource.loop = false;
        }

        //take damage UI
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

       


        if (Input.GetKeyDown(toggleKeyCursorLock))
        {
            cursorLocked = !cursorLocked; // Toggle the state

            if (cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        

        if (Input.GetKeyDown(inventoryKey))
        {
            
            inventoryOpen = !inventoryOpen;
            inventory.SetActive(inventoryOpen);

            // Khi mở Inventory, mở con trỏ chuột
            Cursor.lockState = inventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = inventoryOpen;
            
        }

        // Gọi hàm Attack nếu nhấn chuột phai (hoặc phím tấn công)
        if (Input.GetMouseButtonDown(1))
        {
            Attack();
        }

    }
    private void Attack()
    {

        if (Time.time - lastAttackTime < attackCooldown) return; // Kiểm tra cooldown
        lastAttackTime = Time.time;

        isAttacking = true; // Đặt trạng thái đang tấn công
        animator.SetTrigger(attackHash); // Chạy animation đánh

        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        Vector3 attackPosition = transform.position + transform.forward * 1f; // Lấy vị trí tấn công phía trước
        float attackRadius = 1.5f; // Phạm vi đánh

        Collider[] hitEnemies = Physics.OverlapSphere(attackPosition, attackRadius); // Lấy tất cả kẻ địch trong vùng

        foreach (Collider enemyCollider in hitEnemies)
        {
            Enemy_Health enemy = enemyCollider.GetComponent<Enemy_Health>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }

            Boss_controller boss = enemyCollider.GetComponent<Boss_controller>();
            if(boss != null)
            {
                boss.TakeDamage(35);
            }
        }

        
        // Debug hình cầu tấn công
        //Debug.DrawRay(attackPosition, Vector3.up * 0.1f, Color.red, 1f);
        
    }

    public void StartAttackP()
    {
       
           
        
    }
    public void EndAttackP()
    {
        isAttacking = false;
        
    }
    public void TakeDamage(int damage)
    {
        health = Mathf.Max(0, health - damage);
        healthSlider.value = health;

        if (hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject,1.5f);
        animator.SetTrigger("Die");
    }


    public void InstantHealPotion()
    {
        Heal(30f); // Heal for 30
        if (healEffectPrefab != null)
        {
            GameObject healEffect = Instantiate(healEffectPrefab, transform.position, Quaternion.identity);
            Destroy(healEffect, 1f); // Xóa hiệu ứng sau 2 giây
        }

    }

    public void RegenPotion()
    {
        StartCoroutine(RegenCoroutine());
    }
    private IEnumerator RegenCoroutine()
    {
        float timer = 0f;

        while (timer < regenDuration)
        {
            if (health <= 0) yield break; // Ngừng hồi máu nếu đã chết
            Heal(regenHealPerSecond * Time.deltaTime); // Heal over time
            timer += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
    }
    public void Heal(float healAmount)
    {
        health = Mathf.Min(health + healAmount, maxHealth);

    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerMovementVerTwo : MonoBehaviour
{
    [SerializeField] private float maximumSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpButtonGracePeriod;
    [SerializeField] private Transform cameraTransform;

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    [Header("Attack Settings")]
    public float attackDamage = 25f;
    public float attackRange = 2f;
    public float attackCooldown = 0.5f;
    private float lastAttackTime;
    private int attackHash = Animator.StringToHash("Attack");
    private bool isAttacking = false;

    [Header("Weapon Settings")]
    public List<GameObject> weapons; // Danh sách các vũ khí
    private int currentWeaponIndex = 0; // Vũ khí hiện tại

    [Header("VFX")]
    public GameObject healEffectPrefab;

    [Header("Health")]
    public Slider healthSlider;
    public float maxHealth = 200f;
    public float health;

    public float regenHealPerSecond = 5f;
    public float regenDuration = 10f;

    [Header("InventoryUI")]
    public GameObject inventory;
    public Button inventoryButton;
    public Button reverseInventoryButton;
    public KeyCode inventoryKey = KeyCode.E;
    private bool inventoryOpen = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip moveSound;
    public AudioClip attackSound;
    public AudioClip hurtSound;

    [Header("CursorToggle")]
    public KeyCode toggleKeyCursorLock = KeyCode.F;
    private bool cursorLocked = false;
    private int enemyKillCount = 0;
    public static PlayerMovementVerTwo Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        health = maxHealth;
        inventory.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (inventoryButton != null)
        {
            inventoryOpen = inventoryButton.gameObject.activeInHierarchy;
        }

        // Kích hoạt vũ khí đầu tiên và vô hiệu hóa các vũ khí khác
        SwitchWeapon(0);
    }

    void Update()
    {
        if (isAttacking) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude /= 2;
        }

        animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);

        float speed = inputMagnitude * maximumSpeed;
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
            animator.SetBool("Jump", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
            animator.SetBool("Jump", true);
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;

            if (jumpButtonPressedTime.HasValue && Time.time - jumpButtonPressedTime <= jumpBufferTime)
            {
                ySpeed = jumpSpeed;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }

        Vector3 velocity = movementDirection * speed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            if (!audioSource.isPlaying)
            {
                audioSource.clip = moveSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.loop = false;
        }

        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if (Input.GetKeyDown(toggleKeyCursorLock))
        {
            cursorLocked = !cursorLocked;

            if (cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        if (Input.GetKeyDown(inventoryKey))
        {
            inventoryOpen = !inventoryOpen;
            inventory.SetActive(inventoryOpen);

            Cursor.lockState = inventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = inventoryOpen;
        }

        // Chuyển đổi vũ khí
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Phím 1
        {
            SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Phím 2
        {
            SwitchWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Phím 3
        {
            SwitchWeapon(2);
        }

        // Gọi hàm Attack nếu nhấn chuột phải (hoặc phím tấn công)
        if (Input.GetMouseButtonDown(1))
        {
            Attack();
        }
    }

    private void SwitchWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count) return;

        // Tắt tất cả các vũ khí
        foreach (var weapon in weapons)
        {
            weapon.SetActive(false);
        }

        // Bật vũ khí được chọn
        weapons[index].SetActive(true);
        currentWeaponIndex = index;

        // Cập nhật animation và thông số tấn công nếu cần
        // Ví dụ: animator.SetInteger("WeaponType", index);
    }

    private void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;

        isAttacking = true;
        animator.SetTrigger(attackHash);

        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        Vector3 attackPosition = transform.position + transform.forward * 1f;
        float attackRadius = 1.5f;

        Collider[] hitEnemies = Physics.OverlapSphere(attackPosition, attackRadius);

        foreach (Collider enemyCollider in hitEnemies)
        {
            Enemy_Health enemy = enemyCollider.GetComponent<Enemy_Health>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }

            Boss_controller boss = enemyCollider.GetComponent<Boss_controller>();
            if (boss != null)
            {
                boss.TakeDamage(35);
            }
        }
    }

    public void StartAttackP()
    {
        // Animation Event: Called at the start of the attack animation
    }

    public void EndAttackP()
    {
        isAttacking = false;
        // Animation Event: Called at the end of the attack animation
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Max(0, health - damage);
        healthSlider.value = health;

        if (hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject, 1.5f);
        animator.SetTrigger("Die");
    }

    public void InstantHealPotion()
    {
        Heal(30f);
        if (healEffectPrefab != null)
        {
            GameObject healEffect = Instantiate(healEffectPrefab, transform.position, Quaternion.identity);
            Destroy(healEffect, 1f);
        }
    }

    public void RegenPotion()
    {
        StartCoroutine(RegenCoroutine());
    }

    private IEnumerator RegenCoroutine()
    {
        float timer = 0f;

        while (timer < regenDuration)
        {
            if (health <= 0) yield break;
            Heal(regenHealPerSecond * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void Heal(float healAmount)
    {
        health = Mathf.Min(health + healAmount, maxHealth);
    }
}