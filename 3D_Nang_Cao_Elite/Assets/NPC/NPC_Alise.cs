using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class NPC_Alise : MonoBehaviour
{
    public GameObject conversationPanel;
    public TextMeshProUGUI conversationText;
    public GameObject choicesPanel;
    public Button choice1Button;
    public Button choice2Button;
    public TextMeshProUGUI choice1Text;
    public TextMeshProUGUI choice2Text;
    public List<Transform> waypoints;
    public float stoppingDistance = 1f;
    public float idleTime = 3f;

    private NavMeshAgent agent;
    private Animator animator;
    private int currentWaypointIndex = 0;
    private bool isPlayerNearby = false;
    private bool isIdle = false;
    private Coroutine typingCoroutine;
    //private bool isTyping = false;

    private Dictionary<string, string[]> dialogueOptions = new Dictionary<string, string[]>();
    private string currentDialogueKey = "intro";

    void Start()
    {
        conversationPanel.SetActive(false);
        choicesPanel.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        InitializeDialogue();
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (!isPlayerNearby && agent.remainingDistance < stoppingDistance && !isIdle)
        {
            StartCoroutine(IdleBeforeMoving());
        }
    }

    IEnumerator IdleBeforeMoving()
    {
        isIdle = true;
        yield return new WaitForSeconds(idleTime);
        MoveToNextWaypoint();
        isIdle = false;
    }
    IEnumerator CloseConversationAfterDelay()
    {
        yield return new WaitForSeconds(2f);  // Đợi 2 giây trước khi đóng hội thoại
        HideConversationPanel();
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
        ShowDialogue(currentDialogueKey);
        agent.isStopped = true;
    }

    void HideConversationPanel()
    {
        conversationPanel.SetActive(false);
        choicesPanel.SetActive(false);
        agent.isStopped = false;
        MoveToNextWaypoint();
    }

    public void ShowDialogue(string key)
    {
        if (dialogueOptions.ContainsKey(key))
        {
            conversationText.text = dialogueOptions[key][0];
            choice1Text.text = dialogueOptions[key][1];
            choice2Text.text = dialogueOptions[key][2];

            choicesPanel.SetActive(true);
            choice1Button.onClick.RemoveAllListeners();
            choice2Button.onClick.RemoveAllListeners();
            choice1Button.onClick.AddListener(() => SelectDialogueOption(dialogueOptions[key][1]));
            choice2Button.onClick.AddListener(() => SelectDialogueOption(dialogueOptions[key][2]));
        }
    }

    /*   void SelectDialogueOption(string choice)
       {
           choicesPanel.SetActive(false);
           if (dialogueOptions.ContainsKey(choice))
           {
               currentDialogueKey = choice;
               ShowDialogue(currentDialogueKey);
           }
           else
           {
               HideConversationPanel();
           }
       }*/
    void SelectDialogueOption(string choice)
    {
        choicesPanel.SetActive(false);

        if (choice == "Ket thuc")
        {
            conversationText.text = "Hay suy nghi can than...";
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

    void InitializeDialogue()
    {
        dialogueOptions["intro"] = new string[] {
            "Nguoi khong phai nguoi duy nhat muon Mordain bien mat. Nhung ta tu hoi... nguoi muon huy diet han, hay huy diet tat ca?",
            "Nguoi biet gi ve loi nguyen?",
            "Ta nen lam gi?"
        };

        dialogueOptions["Nguoi biet gi ve loi nguyen?"] = new string[] {
            "Mordain khong chi la mot ke bao chua. Han da giao keo voi bong toi de giu vung ngai vang. Chung nao han con song, bong toi se bao trum Eldoria.",
            "Tro lai",
            "Ket thuc"
        };

        dialogueOptions["Ta nen lam gi?"] = new string[] {
            "Nguoi co nhieu lua chon hon nguoi tuong. Danh bai Mordain khong kho. Nhung lieu nguoi co dinh thay the han, hay de Eldoria sup do mai mai?",
            "Tro lai",
            "Ket thuc"
        };

        dialogueOptions["Tro lai"] = dialogueOptions["intro"];
        dialogueOptions["Ket thuc"] = new string[] { "Hay suy nghi can than...", "", "" };
    }
}