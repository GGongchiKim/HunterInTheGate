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
        GUILayout.Label("Dialogue JSON → ScriptableObject 변환기", EditorStyles.boldLabel);
        jsonFile = (TextAsset)EditorGUILayout.ObjectField("JSON 파일", jsonFile, typeof(TextAsset), false);

        if (GUILayout.Button("DialogueEvent 생성") && jsonFile != null)
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
            Debug.LogError("JSON 파싱 실패: " + ex.Message);
            return;
        }

        if (wrapper == null || string.IsNullOrEmpty(wrapper.eventId))
        {
            Debug.LogError("eventId가 비어 있습니다.");
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

        Debug.Log($"DialogueEvent '{wrapper.eventId}' 생성 완료: {assetPath}");
    }

    // JSON 파싱용 래퍼 클래스 정의
    [System.Serializable]
    public class DialogueEventDataWrapper
    {
        public string eventId;
        public string backgroundSpriteId;
        public List<DialogueNode> nodes;
    }
}