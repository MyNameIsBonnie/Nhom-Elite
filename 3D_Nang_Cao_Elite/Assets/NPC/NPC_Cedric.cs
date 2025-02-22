using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC_Cedric : MonoBehaviour
{
    public GameObject conversationPanel;
    public TextMeshProUGUI conversationText;
    public GameObject choicesPanel;
    public Button choice1Button;
    public TextMeshProUGUI choice1Text;

    private bool isPlayerNearby = false;
    private Dictionary<string, string[]> dialogueOptions = new Dictionary<string, string[]>();
    private string currentDialogueKey = "intro";

    void Start()
    {
        conversationPanel.SetActive(false);
        choicesPanel.SetActive(false);
        InitializeDialogue();
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
        ShowDialogue(currentDialogueKey);
    }

    void HideConversationPanel()
    {
        conversationPanel.SetActive(false);
        choicesPanel.SetActive(false);
    }

    public void ShowDialogue(string key)
    {
        if (dialogueOptions.ContainsKey(key))
        {
            conversationText.text = dialogueOptions[key][0];
            choice1Text.text = dialogueOptions[key][1];

            choicesPanel.SetActive(true);
            choice1Button.onClick.RemoveAllListeners();
            choice1Button.onClick.AddListener(() => SelectDialogueOption(dialogueOptions[key][1]));
        }
    }

    void SelectDialogueOption(string choice)
    {
        choicesPanel.SetActive(false);

        if (choice == "Kết thúc")
        {
            conversationText.text = "Hãy suy nghĩ thật kỹ...";
            StartCoroutine(CloseConversationAfterDelay());
            return;
        }

        if (dialogueOptions.ContainsKey(choice))
        {
            currentDialogueKey = choice;
            ShowDialogue(currentDialogueKey);
        }
        else
        {
            HideConversationPanel();
        }
    }

    IEnumerator CloseConversationAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        HideConversationPanel();
    }

    void InitializeDialogue()
    {
        dialogueOptions["intro"] = new string[] {
            "Kael... Ta không bao giờ nghĩ rằng có ngày lại gặp ngươi ở đây. Ngươi còn nhớ vương quốc ta từng bảo vệ không? Giờ nó chỉ còn là đống đổ nát, bị những kẻ không còn linh hồn thống trị.",
            "Điểm yếu của Mordain là gì?"
        };

        dialogueOptions["Điểm yếu của Mordain là gì?"] = new string[] {
            "Hắn không phải bất tử. Hắn chỉ là con người, dù đã bị bóng tối nuốt chửng. Nhưng hắn sợ lửa – ngọn lửa có thể thiêu rụi tất cả những gì hắn xây dựng.",
            "Kết thúc"
        };

        dialogueOptions["Kết thúc"] = new string[] { "Hãy suy nghĩ thật kỹ...", "" };
    }
}
