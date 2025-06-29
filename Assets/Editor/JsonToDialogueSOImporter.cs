using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

public class JsonToDialogueSOImporter : EditorWindow
{
    private TextAsset jsonFile;

    [MenuItem("Tools/Dialogue/Import Dialogue JSON")]
    public static void ShowWindow()
    {
        GetWindow<JsonToDialogueSOImporter>("Dialogue JSON Importer");
    }

    void OnGUI()
    {
        GUILayout.Label("Dialogue JSON �� ScriptableObject ��ȯ��", EditorStyles.boldLabel);
        jsonFile = (TextAsset)EditorGUILayout.ObjectField("JSON ����", jsonFile, typeof(TextAsset), false);

        if (GUILayout.Button("DialogueEvent ����") && jsonFile != null)
        {
            ImportDialogueEventFromJson(jsonFile.text);
        }
    }

    private void ImportDialogueEventFromJson(string jsonText)
    {
        DialogueEventDataWrapper wrapper;

        try
        {
            wrapper = JsonConvert.DeserializeObject<DialogueEventDataWrapper>(jsonText);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("JSON �Ľ� ����: " + ex.Message);
            return;
        }

        if (wrapper == null || string.IsNullOrEmpty(wrapper.eventId))
        {
            Debug.LogError("eventId�� ��� �ֽ��ϴ�.");
            return;
        }

        DialogueEvent dialogueEvent = ScriptableObject.CreateInstance<DialogueEvent>();
        dialogueEvent.eventId = wrapper.eventId;
        dialogueEvent.nodes = wrapper.nodes;
#if UNITY_EDITOR
        if (!string.IsNullOrEmpty(wrapper.backgroundSpriteId))
        {
            dialogueEvent.backgroundSpriteId = wrapper.backgroundSpriteId;
        }
#endif

        string folderPath = "Assets/Resources/DialogueEvents";
        if (!AssetDatabase.IsValidFolder(folderPath))
            AssetDatabase.CreateFolder("Assets/Resources", "DialogueEvents");

        string assetPath = $"{folderPath}/{wrapper.eventId}.asset";
        AssetDatabase.CreateAsset(dialogueEvent, assetPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"DialogueEvent '{wrapper.eventId}' ���� �Ϸ�: {assetPath}");
    }

    // JSON �Ľ̿� ���� Ŭ���� ����
    [System.Serializable]
    public class DialogueEventDataWrapper
    {
        public string eventId;
        public string backgroundSpriteId;
        public List<DialogueNode> nodes;
    }
}