using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ModularCharacters
{
    public class CharacterSplitter : MonoBehaviour
    {
        public SkinnedMeshRenderer[] renderers;

        [SerializeField] private List<SkinnedMeshData> datas = new List<SkinnedMeshData>();

        [Button]
        public void CollectRenderers()
        {
            renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        [Button]
        public void Split()
        {
            ClearSkinnedMeshData();

            int removeCount = "(Clone)".Length;
            foreach (SkinnedMeshRenderer meshRenderer in renderers)
            {
                var nMeshRenderer = Instantiate(meshRenderer);
                nMeshRenderer.name = nMeshRenderer.name.Remove(nMeshRenderer.name.Length - removeCount, removeCount);
                var data = nMeshRenderer.gameObject.AddComponent<SkinnedMeshData>();
                datas.Add(data);
                var characterModule = nMeshRenderer.gameObject.AddComponent<CharacterModule>();
            }
        }

        [Button]
        public void ClearSkinnedMeshData()
        {
            for (int i = datas.Count - 1; i >= 0; i--)
            {
                if(Application.isPlaying) Destroy(datas[i].gameObject);
                else DestroyImmediate(datas[i].gameObject);
            }
            datas.Clear();
        }
    }
}