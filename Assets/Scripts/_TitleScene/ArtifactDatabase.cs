using UnityEngine;
using System.Collections.Generic;

namespace Inventory
{
    public class ArtifactDatabase : MonoBehaviour
    {
        public static ArtifactDatabase Instance { get; private set; }

        private List<ArtifactData> allArtifacts = new();
        private Dictionary<string, ArtifactData> artifactById = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            LoadArtifacts();
        }

        private void LoadArtifacts()
        {
            allArtifacts.Clear();
            artifactById.Clear();

            var loadedArtifacts = Resources.LoadAll<ArtifactData>("Artifacts");
            foreach (var artifact in loadedArtifacts)
            {
                if (!artifactById.ContainsKey(artifact.id))
                {
                    artifactById.Add(artifact.id, artifact);
                    allArtifacts.Add(artifact);
                }
                else
                {
                    Debug.LogWarning($"[ArtifactDatabase] 중복된 아티팩트 ID 발견: {artifact.id}");
                }
            }
        }

        public List<ArtifactData> GetAllArtifacts() => new(allArtifacts);

    
        public ArtifactData GetArtifactById(string id)
        {
            if (artifactById.TryGetValue(id, out var artifact))
                return artifact;

            Debug.LogWarning($"[ArtifactDatabase] 존재하지 않는 아티팩트 ID: {id}");
            return null;
        }
    }
}
