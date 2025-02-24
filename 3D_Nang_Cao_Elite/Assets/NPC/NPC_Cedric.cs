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

        if (choice == "Ket thuc")
        {
            conversationText.text = "Hay suy nghi that ky...";
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
            "Kael... Ta khong bao gio nghi rang co ngay lai gap nguoi o day. Nguoi con nho vuong quoc ta tung bao ve khong? Gio no chi con la dong do nat, bi nhung ke khong con linh hon thong tri.",
            "Diem yeu cua Mordain la gi?"
        };

        dialogueOptions["Diem yeu cua Mordain la gi?"] = new string[] {
            "Han khong phai bat tu. Han chi la con nguoi, du da bi bong toi nuot chung. Nhung han so lua – ngon lua co the thieu rui tat ca nhung gi han xay dung.",
            "Ket thuc"
        };

        dialogueOptions["Ket thuc"] = new string[] { "Hay suy nghi that ky...", "" };
    }
}
