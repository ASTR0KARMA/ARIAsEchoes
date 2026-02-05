using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NarrationSystem : MonoBehaviour
{
    public static NarrationSystem Instance { get; private set; }

    [SerializeField] private InputActionReference inputAction;
    [SerializeField] private float typewriterSpeed = 0.03f;
    [SerializeField] private NarrationUI narrationUI;

    private Queue<NarrationPart> narrationQueue;
    private NarrationPart currentPart;
    private int currentTextIndex;
    private bool isTyping;
    private bool skipRequested;
    private string currentFullText;
    private Coroutine typewriterCoroutine;
    private Action onNarrationComplete;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        NarrationSystem.Instance.StartNarration(Narration.Aubergiste);
    }

    private void OnEnable()
    {
        if (inputAction != null && inputAction.action != null)
        {
            inputAction.action.Enable();
            inputAction.action.performed += OnInputPerformed;
        }
    }

    private void OnDisable()
    {
        if (inputAction != null && inputAction.action != null)
        {
            inputAction.action.performed -= OnInputPerformed;
            inputAction.action.Disable();
        }
    }

    public void StartNarration(List<NarrationPart> parts, Action onComplete = null)
    {
        narrationQueue = new Queue<NarrationPart>(parts);
        onNarrationComplete = onComplete;
        narrationUI.Show();
        ProcessNextPart();
    }

    public void StartNarration(NarrationPart[] parts, Action onComplete = null)
    {
        narrationQueue = new Queue<NarrationPart>(parts);
        onNarrationComplete = onComplete;
        narrationUI.Show();
        ProcessNextPart();
    }

    private void ProcessNextPart()
    {
        if (narrationQueue.Count == 0)
        {
            EndNarration();
            return;
        }

        currentPart = narrationQueue.Dequeue();
        currentTextIndex = 0;
        narrationUI.SetSpeakerName(currentPart.SpeakerName);
        narrationUI.HideChoices();
        DisplayCurrentText();
    }

    private void DisplayCurrentText()
    {
        if (currentTextIndex >= currentPart.Texts.Length)
        {
            if (currentPart.HasChoices)
            {
                ShowChoices();
            }
            else
            {
                ProcessNextPart();
            }
            return;
        }

        currentFullText = currentPart.Texts[currentTextIndex];

        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }

        typewriterCoroutine = StartCoroutine(TypewriterEffect());
    }

    private IEnumerator TypewriterEffect()
    {
        isTyping = true;
        skipRequested = false;
        string displayedText = "";

        foreach (char c in currentFullText)
        {
            if (skipRequested)
            {
                narrationUI.SetDialogueText(currentFullText);
                break;
            }

            displayedText += c;
            narrationUI.SetDialogueText(displayedText);
            yield return new WaitForSeconds(typewriterSpeed);
        }

        isTyping = false;
        typewriterCoroutine = null;
    }

    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        if (!narrationUI.IsVisible)
            return;

        if (isTyping)
        {
            skipRequested = true;
            return;
        }

        if (currentPart.HasChoices && currentTextIndex >= currentPart.Texts.Length)
            return;

        currentTextIndex++;
        DisplayCurrentText();
    }

    private void ShowChoices()
    {
        narrationUI.ShowChoices(currentPart.Choice1, currentPart.Choice2, OnChoiceSelected);
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        narrationUI.HideChoices();

        NarrationPart[] branchToLoad = choiceIndex == 0 ? currentPart.Choice1Branch : currentPart.Choice2Branch;

        if (branchToLoad != null && branchToLoad.Length > 0)
        {
            foreach (var part in branchToLoad)
            {
                narrationQueue.Enqueue(part);
            }
        }

        ProcessNextPart();
    }

    private void EndNarration()
    {
        narrationUI.Hide();
        currentPart = null;
        onNarrationComplete?.Invoke();
        onNarrationComplete = null;
    }

    public bool IsNarrationActive => narrationUI != null && narrationUI.IsVisible;
}