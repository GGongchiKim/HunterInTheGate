using System.Collections.Generic;

public static class GameVariables
{
    // 내부 변수 저장소 (모든 값은 object로 저장)
    private static Dictionary<string, object> variables = new();

    // --- Setter ---

    public static void Set(string key, int value) => variables[key] = value;
    public static void Set(string key, float value) => variables[key] = value;
    public static void Set(string key, bool value) => variables[key] = value;
    public static void Set(string key, string value) => variables[key] = value;
    public static void SetRaw(string key, object value) => variables[key] = value;

    // --- Getter ---

    public static int GetInt(string key, int defaultValue = 0) =>
        variables.TryGetValue(key, out var value) && value is int i ? i : defaultValue;

    public static float GetFloat(string key, float defaultValue = 0f) =>
        variables.TryGetValue(key, out var value) && value is float f ? f : defaultValue;

    public static bool GetBool(string key, bool defaultValue = false) =>
        variables.TryGetValue(key, out var value) && value is bool b ? b : defaultValue;

    public static string GetString(string key, string defaultValue = "") =>
        variables.TryGetValue(key, out var value) && value is string s ? s : defaultValue;

    public static object GetRaw(string key) =>
        variables.TryGetValue(key, out var value) ? value : null;

    // --- Helpers ---

    public static bool Exists(string key) => variables.ContainsKey(key);
    public static void Remove(string key) => variables.Remove(key);
    public static void Clear() => variables.Clear();
}