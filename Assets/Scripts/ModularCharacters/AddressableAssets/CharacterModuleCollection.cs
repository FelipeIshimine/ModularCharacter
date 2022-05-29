using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ModularCharacters.AddressableAssets
{
    [CreateAssetMenu(menuName = "ModularCharacters/CharacterModuleCollection", fileName = "CharacterModuleCollection", order = 0)]
    public class CharacterModuleCollection : ScriptableObject
    {
        [field: SerializeField]
        public List<AssetReferenceCharacterModule> Elements { get; private set; } =
            new List<AssetReferenceCharacterModule>();

        public void Add(CharacterModule module)
        {
            var assetReference = new AssetReferenceCharacterModule(new AssetReferenceGameObject(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(module))));
            Elements.Add(assetReference);
        }

        [Button]
        public void Add(CharacterModule[] modules)
        {
            foreach (CharacterModule characterModule in modules)
                Add(characterModule);
        }

        private void OnEnable()
        {
            RefreshAll();
        }

        [Button]
        public void RefreshAll()
        {
            foreach (var element in Elements)
                element.Refresh();
            
            Elements.Sort(Sorter);
        }

        private int Sorter(AssetReferenceCharacterModule x, AssetReferenceCharacterModule y) =>
            string.Compare(x.Name, y.Name, StringComparison.Ordinal);

        [Button]
        public Dictionary<EquipmentSlot, List<AssetReferenceCharacterModule>> GetSlotsIndex()
        {
            Dictionary<EquipmentSlot, List<AssetReferenceCharacterModule>> slotIndex =
                new Dictionary<EquipmentSlot, List<AssetReferenceCharacterModule>>();

            foreach (AssetReferenceCharacterModule characterModule in Elements)
            {
                foreach (EquipmentSlot equipmentSlot in characterModule.Slots)
                {
                    if (!slotIndex.TryGetValue(equipmentSlot, out List<AssetReferenceCharacterModule> list))
                        slotIndex[equipmentSlot] = list = new List<AssetReferenceCharacterModule>();
                    list.Add(characterModule);
                }
            }
            return slotIndex;
        }

        public Dictionary<string, AssetReferenceCharacterModule> GetIdIndex()
        {
            Dictionary<string, AssetReferenceCharacterModule> slotIndex = new Dictionary<string, AssetReferenceCharacterModule>();
            foreach (var characterModule in Elements)
                slotIndex.Add(characterModule.Id, characterModule);
            return slotIndex;
        }
        
        public Dictionary<EquipmentTag, List<AssetReferenceCharacterModule>> GetTagIndex()
        {
            Dictionary<EquipmentTag, List<AssetReferenceCharacterModule>> slotIndex = new Dictionary<EquipmentTag, List<AssetReferenceCharacterModule>>();
            foreach (var characterModule in Elements)
            {
                foreach (var tag in characterModule.Tags)
                {
                    if (!slotIndex.TryGetValue(tag, out List<AssetReferenceCharacterModule> list))
                        slotIndex[tag] = list = new List<AssetReferenceCharacterModule>();
                    list.Add(characterModule);
                }
            }
            return slotIndex;
        }

        public IEnumerable<EquipmentTag> GetAllTags()
        {
            var tags = Elements.ConvertAll(x => x.Tags);
            HashSet<EquipmentTag> tagsSet = new HashSet<EquipmentTag>();
            foreach (EquipmentTag[] tagArray in tags)
            foreach (var tag in tagArray)
                tagsSet.Add(tag);
            return tagsSet;
        }

        public IEnumerable<AssetReferenceCharacterModule> GetFromSlot(EquipmentSlot slot)
        {
            List<AssetReferenceCharacterModule> assetReferenceCharacterModules =
                new List<AssetReferenceCharacterModule>();

            foreach (AssetReferenceCharacterModule element in Elements)
            {
                foreach (var equipmentSlot in element.Slots)
                {
                    if (equipmentSlot == slot)
                    {
                        assetReferenceCharacterModules.Add(element);
                        break;
                    }
                }
            }
            return assetReferenceCharacterModules;
        }
    }
}