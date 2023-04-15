using System;

public enum ChatCommands
{
    [StringValue("DM")] Dm,
    [StringValue("chat")] Chat,
    [StringValue("voice")] Voice,
    [StringValue("resetTopic")] ResetTopic,
    [StringValue("question")] Question,
}

public class StringValueAttribute : Attribute
{
    public string Value { get; }

    public StringValueAttribute(string value)
    {
        Value = value;
    }
}

public static class ChatCommandsExtensions
{
    public static string GetStringValue(this ChatCommands command)
    {
        var type = typeof(ChatCommands);
        var memberInfo = type.GetMember(command.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(StringValueAttribute), false);
        return ((StringValueAttribute)attributes[0]).Value;
    }
}
