using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("Health")]
    public Slider healthSlider;
    public float maxHealth = 100f;
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
    public static PlayerMovementVerTwo Instance;
    private void Awake()
    {
        Instance = this;
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
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
            animator.SetTrigger("Jump");
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
        


        /*if (Input.GetKeyDown(inventoryKey))
        {
            if (inventoryButton != null)
            {
                inventoryOpen = !inventoryOpen;
                if (inventoryOpen)
                {
                    inventory.gameObject.SetActive(inventoryOpen);
                }
                else
                {
                    inventory.gameObject.SetActive(false);
                }
            }
        }*/

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
                Debug.Log("Hit enemy: " + enemy.name); // Kiểm tra xem đánh trúng ai
            }
        }

        // Debug hình cầu tấn công
        Debug.DrawRay(attackPosition, Vector3.up * 0.1f, Color.red, 1f);

    }

    /*public void TakeDamage(int damage)
    {
        //health -= damage;
        health = Mathf.Min(health - damage);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }*/

    public void TakeDamage(int damage)
    {
        /*health = Mathf.Max(0, health - damage); // Giữ giá trị từ 0 trở lên
        healthSlider.value = health; // Cập nhật UI ngay lập tức

        if (health <= 0)
        {
            Die();
        }*/

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
        //Debug.Log("Player has died.");
        animator.SetTrigger("Die");
    }


    public void InstantHealPotion()
    {
        Heal(30f); // Heal for 30
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
