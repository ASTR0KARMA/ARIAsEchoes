using System;
using UnityEngine;

[Serializable]
public class NarrationPart
{
    public string SpeakerName;
    public string[] Texts;
    public string Choice1;
    public string Choice2;
    public NarrationPart[] Choice1Branch;
    public NarrationPart[] Choice2Branch;
    public Transform TargetTransform;

    public bool HasChoices => !string.IsNullOrEmpty(Choice1) && !string.IsNullOrEmpty(Choice2);

    public NarrationPart(string speakerName, string[] texts, Transform targetTransform = null)
    {
        SpeakerName = speakerName;
        Texts = texts;
        Choice1 = null;
        Choice2 = null;
        Choice1Branch = null;
        Choice2Branch = null;
        TargetTransform = targetTransform;
    }

    public NarrationPart(string speakerName, string[] texts, string choice1, NarrationPart[] choice1Branch, string choice2, NarrationPart[] choice2Branch, Transform targetTransform = null)
    {
        SpeakerName = speakerName;
        Texts = texts;
        Choice1 = choice1;
        Choice2 = choice2;
        Choice1Branch = choice1Branch;
        Choice2Branch = choice2Branch;
        TargetTransform = targetTransform;
    }
}