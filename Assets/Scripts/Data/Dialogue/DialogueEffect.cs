using System;

public enum EffectMode
{
    Add,
    Subtract,
    Set
}

[Serializable]
public class DialogueEffect
{
    public string targetVariable;
    public EffectMode mode;
    public int amount;
}