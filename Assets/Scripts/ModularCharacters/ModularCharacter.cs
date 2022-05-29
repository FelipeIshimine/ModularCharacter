using System;
using System.Collections.Generic;
using ModularCharacters.AddressableAssets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ModularCharacters
{
    public class ModularCharacter : MonoBehaviour
    {
        public Transform renderersContainer;
        public HierarchyIndex hierarchyIndex;

        public ModularCharacterPreset defaultPreset;

        private readonly List<CharacterModule> _equipped = new List<CharacterModule>();
        private readonly Dictionary<EquipmentSlot, CharacterModule> _equipmentIndex = new Dictionary<EquipmentSlot, CharacterModule>();
        
        private void Reset()
        {
            renderersContainer = transform;
        }


        private void Start()
        {
            if(defaultPreset)
                Equip(defaultPreset);
        }

        private void Equip(ModularCharacterPreset characterModulePreset)
        {
            foreach (CharacterModule characterModule in characterModulePreset.CharacterModules)
                EquipPrefab(characterModule);
        }

        [Button]
        public void EquipPrefab(CharacterModule characterModule)
        {
            var go = Instantiate(characterModule, renderersContainer);
            go.name = go.name.Replace("(Clone)", string.Empty);
            Equip(go);
        }

        [Button]
        public async void EquipPrefabAsync(AssetReferenceCharacterModule prefabReference)
        {
            var task= prefabReference.AssetReference.InstantiateAsync(renderersContainer);
            GameObject go = await task.Task;
            go.name = go.name.Replace("(Clone)", string.Empty);
            go.AddComponent<SelfReleaseAsset>();
            Equip(go.GetComponent<CharacterModule>());
        }

        private void Equip(CharacterModule characterModule)
        {
            foreach (var slot in characterModule.Slots)
                Unequip(slot);
            
            foreach (var slot in characterModule.Slots)
                _equipmentIndex.Add(slot, characterModule);
            
            characterModule.transform.SetParent(renderersContainer);
            characterModule.Initialize(this);
            _equipped.Add(characterModule);
        }

        public void Unequip(EquipmentSlot slot)
        {
            if (_equipmentIndex.TryGetValue(slot, out var equipment))
                Unequip(equipment);
        }

        private void Unequip(CharacterModule characterModule)
        {
            _equipped.Remove(characterModule);

            foreach (EquipmentSlot equipmentSlot in characterModule.Slots)
                _equipmentIndex.Remove(equipmentSlot);
            

            if (characterModule.gameObject.TryGetComponent(out SelfReleaseAsset selfReleaseAsset))
            {
                selfReleaseAsset.Release();
            }
            else
            {
                if(Application.isPlaying)
                    Destroy(characterModule.gameObject);
                else
                    DestroyImmediate(characterModule.gameObject);
            }
        }

        [Button]
        private void UnequipAll()
        {
            var elements = _equipped.ToArray();
            foreach (CharacterModule characterModule in elements)
                Unequip(characterModule);
        }

        [Button]
        public void SaveTo(ModularCharacterPreset characterPreset)
        {
            List<string> ids = _equipped.ConvertAll(x => x.Id);
            characterPreset.Set(ids);
        }
    }
}
