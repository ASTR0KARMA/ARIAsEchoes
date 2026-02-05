using System;
using UnityEngine;
using UnityEngine.UIElements;

public class NarrationUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private VisualElement root;
    private VisualElement narrationContainer;
    private Label speakerLabel;
    private Label dialogueLabel;
    private VisualElement choicesContainer;
    private Button choice1Button;
    private Button choice2Button;

    private Action<int> onChoiceCallback;

    public bool IsVisible => narrationContainer != null && narrationContainer.style.display == DisplayStyle.Flex;

    private void Awake()
    {
        root = uiDocument.rootVisualElement;
        narrationContainer = root.Q<VisualElement>("narration-container");
        speakerLabel = root.Q<Label>("narrator-name");
        dialogueLabel = root.Q<Label>("narration-text");
        choicesContainer = root.Q<VisualElement>("choices-container");
        choice1Button = root.Q<Button>("choice1-button");
        choice2Button = root.Q<Button>("choice2-button");

        choice1Button.clicked += () => OnChoiceClicked(0);
        choice2Button.clicked += () => OnChoiceClicked(1);

        Hide();
    }

    public void Show()
    {
        narrationContainer.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        narrationContainer.style.display = DisplayStyle.None;
        HideChoices();
    }

    public void SetSpeakerName(string name)
    {
        speakerLabel.text = name;
    }

    public void SetDialogueText(string text)
    {
        dialogueLabel.text = text;
    }

    public void ShowChoices(string choice1Text, string choice2Text, Action<int> callback)
    {
        onChoiceCallback = callback;
        choice1Button.text = choice1Text;
        choice2Button.text = choice2Text;
        choicesContainer.style.display = DisplayStyle.Flex;
    }

    public void HideChoices()
    {
        choicesContainer.style.display = DisplayStyle.None;
        onChoiceCallback = null;
    }

    private void OnChoiceClicked(int index)
    {
        onChoiceCallback?.Invoke(index);
    }
}