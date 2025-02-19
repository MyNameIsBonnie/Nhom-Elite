using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class NPC_Move : MonoBehaviour
{
    public GameObject conversationPanel; // Panel hiển thị hội thoại
    public TextMeshProUGUI conversationText; // Hiển thị nội dung hội thoại
    public List<Transform> waypoints; // Danh sách các điểm di chuyển
    public float stoppingDistance = 1f; // Khoảng cách dừng lại khi đến waypoint

    [SerializeField]
    private List<string> conversationLines = new List<string>(); // Danh sách hội thoại
    private int currentLineIndex = 0; // Chỉ số dòng hội thoại hiện tại
    private Coroutine typingCoroutine; // Coroutine cho hiệu ứng đánh máy
    private bool isTyping = false; // Kiểm tra có đang đánh máy không

    private PlayerMovementVerTwo playerScript; // Điều khiển người chơi
    private CinemachineFreeLook freeLookCamera; // Camera Cinemachine
    private NavMeshAgent agent; // NavMeshAgent để di chuyển NPC
    private int currentWaypointIndex = 0; // Chỉ số điểm đích hiện tại
    private bool isPlayerNearby = false; // Kiểm tra có người chơi gần không

    private Animator animator; // 🎭 Bộ điều khiển animation

    void Start()
    {
        conversationPanel.SetActive(false);
        playerScript = FindObjectOfType<PlayerMovementVerTwo>();
        freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Lấy component Animator

        if (waypoints.Count > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position); // Thiết lập điểm đến đầu tiên
        }
    }

    void Update()
    {
        // Kiểm tra nếu NPC đang di chuyển thì bật animation Walk, nếu không thì bật Idle
        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (!isPlayerNearby && agent.remainingDistance < stoppingDistance)
        {
            MoveToNextWaypoint(); // Tiếp tục di chuyển nếu không có người chơi gần
        }

        if (conversationPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowNextLine(); // Nhấn Space để tiếp tục hội thoại
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                HideConversationPanel(); // Nhấn Enter để thoát hội thoại
            }
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Count == 0) return;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            ShowConversation();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            HideConversationPanel();
        }
    }

    void ShowConversation()
    {
        conversationPanel.SetActive(true);
        currentLineIndex = 0;
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(conversationLines[currentLineIndex]));

        // Dừng di chuyển khi hội thoại bắt đầu
        agent.isStopped = true;
        animator.SetBool("isWalking", false); // Dừng animation đi bộ

        // Vô hiệu hóa điều khiển nhân vật và camera
        if (playerScript != null) playerScript.enabled = false;
        if (freeLookCamera != null) freeLookCamera.enabled = false;
    }

    void HideConversationPanel()
    {
        conversationPanel.SetActive(false);
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        // Tiếp tục di chuyển sau khi hội thoại kết thúc
        agent.isStopped = false;
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // Bật lại điều khiển nhân vật và camera
        if (playerScript != null) playerScript.enabled = true;
        if (freeLookCamera != null) freeLookCamera.enabled = true;
    }

    void ShowNextLine()
    {
        if (isTyping) return;

        if (currentLineIndex < conversationLines.Count - 1)
        {
            currentLineIndex++;
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeLine(conversationLines[currentLineIndex]));
        }
        else
        {
            HideConversationPanel();
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        conversationText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            conversationText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
    }
}
