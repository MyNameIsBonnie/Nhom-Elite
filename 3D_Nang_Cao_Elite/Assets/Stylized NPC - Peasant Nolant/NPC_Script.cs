using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class NPC_Script : MonoBehaviour
{
    public GameObject conversationPanel; // Panel hiển thị hội thoại
    public TextMeshProUGUI conversationText; // TextMeshPro để hiển thị hội thoại

    [SerializeField]
    private List<string> conversationLines = new List<string>(); // Danh sách các dòng hội thoại
    private int currentLineIndex = 0; // Chỉ số của dòng hội thoại hiện tại
    private Coroutine typingCoroutine; // Coroutine chạy hiệu ứng đánh máy

    public float typingSpeed = 0.05f; // Tốc độ đánh máy
    private bool isTyping = false; // Kiểm tra xem có đang đánh máy không

    private PlayerMovementVerTwo playerScript; // Script điều khiển nhân vật
    private CinemachineFreeLook freeLookCamera; // Camera của Cinemachine

    void Start()
    {
        // Ẩn panel hội thoại ban đầu
        conversationPanel.SetActive(false);

        // Lấy script điều khiển nhân vật và camera
        playerScript = FindObjectOfType<PlayerMovementVerTwo>();
        freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
    }

    void Update()
    {
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (conversationLines.Count == 0) return;

            ShowConversation();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideConversationPanel();
        }
    }

    void ShowConversation()
    {
        conversationPanel.SetActive(true);
        currentLineIndex = 0;
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(conversationLines[currentLineIndex]));

        // Vô hiệu hóa điều khiển nhân vật và camera
        if (playerScript != null) playerScript.enabled = false;
        if (freeLookCamera != null) freeLookCamera.enabled = false;
    }

    void HideConversationPanel()
    {
        conversationPanel.SetActive(false);
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        // Bật lại điều khiển nhân vật và camera
        if (playerScript != null) playerScript.enabled = true;
        if (freeLookCamera != null) freeLookCamera.enabled = true;
    }

    void ShowNextLine()
    {
        if (isTyping) return; // Ngăn spam khi chưa hiển thị xong

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
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

}
