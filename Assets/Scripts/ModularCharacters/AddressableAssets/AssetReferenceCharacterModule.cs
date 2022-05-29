using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ModularCharacters.AddressableAssets
{
    [System.Serializable]
    public class AssetReferenceCharacterModule 
    {
        [field:SerializeField] public string Name { get; private set; }
        [field:SerializeField] public string Id { get; private set; }

        public AssetReferenceCharacterModule(AssetReferenceGameObject assetReference)
        {
            AssetReference = assetReference;
        }

        [field:SerializeField] public EquipmentSlot[] Slots { get; private set; } = Array.Empty<EquipmentSlot>();
        [field:SerializeField] public EquipmentTag[] Tags { get; private set; } = Array.Empty<EquipmentTag>();

        [field:SerializeField, OnValueChanged(nameof(RefreshFromAssetReference))] public AssetReferenceGameObject AssetReference { get; private set; }

#if UNITY_EDITOR
        private void RefreshFromAsset(string guid)
        {
            var characterModule = AssetDatabase.LoadAssetAtPath<CharacterModule>(AssetDatabase.GUIDToAssetPath(guid));
            Refresh(characterModule);
        }

        private void RefreshFromAssetReference(AssetReferenceGameObject assetReferenceGameObject)
        {
            if (assetReferenceGameObject == null) return;
            Refresh(assetReferenceGameObject.editorAsset.GetComponent<CharacterModule>());
        }

        public void Refresh(CharacterModule characterModule)
        {
            Name = characterModule.name;
            Id = characterModule.Id;
            Slots = characterModule.Slots.ToArray();
            Tags = characterModule.Tags.ToArray();
        }

        public void Refresh() => RefreshFromAssetReference(AssetReference);
#endif
     
    }
}