using System;

[Serializable]
public class NarrationPart
{
    public string SpeakerName;
    public string[] Texts;
    public string Choice1;
    public string Choice2;
    public NarrationPart[] Choice1Branch;
    public NarrationPart[] Choice2Branch;

    public bool HasChoices => !string.IsNullOrEmpty(Choice1) && !string.IsNullOrEmpty(Choice2);

    public NarrationPart(string speakerName, string[] texts)
    {
        SpeakerName = speakerName;
        Texts = texts;
        Choice1 = null;
        Choice2 = null;
        Choice1Branch = null;
        Choice2Branch = null;
    }

    public NarrationPart(string speakerName, string[] texts, string choice1, NarrationPart[] choice1Branch, string choice2, NarrationPart[] choice2Branch)
    {
        SpeakerName = speakerName;
        Texts = texts;
        Choice1 = choice1;
        Choice2 = choice2;
        Choice1Branch = choice1Branch;
        Choice2Branch = choice2Branch;
    }
}