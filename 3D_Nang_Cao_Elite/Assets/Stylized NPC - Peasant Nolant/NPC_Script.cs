using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class NPC_Script : MonoBehaviour
{
    public GameObject conversationPanel; // Gán panel cuộc trò chuyện vào đây từ Inspector
    public Button exitButton; // Gán button Exit vào đây từ Inspector
    public Button continueButton; // Gán button Continue vào đây từ Inspector
    public TextMeshProUGUI conversationText; // Gán TextMeshPro để hiển thị cuộc trò chuyện vào đây từ Inspector

    [SerializeField]
    private List<string> conversationLines = new List<string>(); // Danh sách các dòng trò chuyện
    private int currentLineIndex = 0; // Chỉ số dòng trò chuyện hiện tại
    private Coroutine typingCoroutine; // Coroutine cho hiệu ứng chữ chạy

    public float typingSpeed = 0.05f; // Tốc độ chữ chạy (thời gian giữa mỗi ký tự)

    private PlayerMovementVerTwo playerScript; // Biến lưu trữ script điều khiển di chuyển của người chơi
    private CinemachineFreeLook freeLookCamera; // Biến lưu trữ FreeLook Camera của Cinemachine

    void Start()
    {
        // Ẩn panel cuộc trò chuyện khi bắt đầu
        conversationPanel.SetActive(false);

        // Gán sự kiện cho button
        exitButton.onClick.AddListener(HideConversationPanel);
        continueButton.onClick.AddListener(ShowNextLine);

        // Lấy các script điều khiển di chuyển và FreeLook Camera của người chơi
        playerScript = FindObjectOfType<PlayerMovementVerTwo>();
        freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu player vào khu vực của NPC
        if (other.CompareTag("Player"))
        {
            // Hiển thị panel cuộc trò chuyện và dòng trò chuyện đầu tiên
            conversationPanel.SetActive(true);
            currentLineIndex = 0;
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeLine(conversationLines[currentLineIndex]));

            // Hiển thị lại nút Continue
            continueButton.gameObject.SetActive(true);

            // Vô hiệu hóa các script điều khiển di chuyển và FreeLook Camera của người chơi
            if (playerScript != null)
            {
                playerScript.enabled = false;
            }
            if (freeLookCamera != null)
            {
                freeLookCamera.enabled = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Kiểm tra nếu player rời khỏi khu vực của NPC
        if (other.CompareTag("Player"))
        {
            // Ẩn panel cuộc trò chuyện
            conversationPanel.SetActive(false);

            // Kích hoạt lại các script điều khiển di chuyển và FreeLook Camera của người chơi
            if (playerScript != null)
            {
                playerScript.enabled = true;
            }
            if (freeLookCamera != null)
            {
                freeLookCamera.enabled = true;
            }
        }
    }

    void HideConversationPanel()
    {
        // Ẩn panel cuộc trò chuyện
        conversationPanel.SetActive(false);
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Kích hoạt lại các script điều khiển di chuyển và FreeLook Camera của người chơi
        if (playerScript != null)
        {
            playerScript.enabled = true;
        }
        if (freeLookCamera != null)
        {
            freeLookCamera.enabled = true;
        }
    }

    void ShowNextLine()
    {
        // Chuyển sang dòng trò chuyện tiếp theo
        if (currentLineIndex < conversationLines.Count - 1)
        {
            currentLineIndex++;
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeLine(conversationLines[currentLineIndex]));
        }
        else
        {
            // Nếu hết dòng trò chuyện, ẩn panel và nút Continue
            HideConversationPanel();
            continueButton.gameObject.SetActive(false);
        }
    }

    IEnumerator TypeLine(string line)
    {
        conversationText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            conversationText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Kiểm tra nếu đang ở dòng cuối cùng và đã hiển thị xong dòng text
        if (currentLineIndex == conversationLines.Count - 1)
        {
            continueButton.gameObject.SetActive(false);
        }
    }

}
