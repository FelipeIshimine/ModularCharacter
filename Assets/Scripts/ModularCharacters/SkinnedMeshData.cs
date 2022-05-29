using Sirenix.OdinInspector;
using UnityEngine;

namespace ModularCharacters
{
    public class SkinnedMeshData : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;

        [SerializeField] private string rootBone;
        [SerializeField] private string[] bones;
    
        private void Reset()
        {
            _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            CollectData();
        }

        [Button]
        public void CollectData()
        {
            rootBone = _skinnedMeshRenderer.rootBone.name;
            var bonesTransforms = _skinnedMeshRenderer.bones;
            bones = new string[bonesTransforms.Length];
            for (int i = 0; i < bonesTransforms.Length; i++)
                bones[i] = bonesTransforms[i].name;
        }

        [Button]
        public void CollectBonesFrom(HierarchyIndex hierarchyIndex)
        {
            _skinnedMeshRenderer.rootBone = hierarchyIndex.Get(rootBone);
            Transform[] tBones = new Transform[bones.Length];
            for (var index = 0; index < bones.Length; index++)
                tBones[index] = hierarchyIndex.Get(bones[index]);
            _skinnedMeshRenderer.bones = tBones;
        }
    }
}