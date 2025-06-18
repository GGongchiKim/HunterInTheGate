using System;

public enum ConditionOperator
{
    Equal,
    NotEqual,
    GreaterThan,
    LessThan,
    GreaterOrEqual,
    LessOrEqual
}

[Serializable]
public class DialogueCondition
{
    public string variableName;
    public ConditionOperator op;
    public int value;
}