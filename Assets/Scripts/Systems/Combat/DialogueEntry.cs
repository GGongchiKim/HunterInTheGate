public class DialogueEntry
{
    public bool isPlayer;
    public string speakerName;
    public string text;
    public string forceCardId;
    public TutorialTurnHint hint;

    public DialogueEntry() { }

    public DialogueEntry(
        bool isPlayer,
        string speakerName,
        string text,
        string forceCardId = null,
        TutorialTurnHint hint = null)
    {
        this.isPlayer = isPlayer;
        this.speakerName = speakerName;
        this.text = text;
        this.forceCardId = forceCardId;
        this.hint = hint;
    }
}