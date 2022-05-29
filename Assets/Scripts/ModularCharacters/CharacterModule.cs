using System.Collections.Generic;
using UnityEngine;

namespace ModularCharacters
{
    public class CharacterModule : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private SkinnedMeshData skinnedMeshData;

        [field:SerializeField] public List<EquipmentSlot> Slots  { get; private set; }= new List<EquipmentSlot>();
        [field:SerializeField]  public List<EquipmentTag> Tags { get; private set; } = new List<EquipmentTag>();
        public string Id => name;

        private void Reset()
        {
            skinnedMeshData = GetComponent<SkinnedMeshData>();
        }

        public void Initialize(ModularCharacter modularCharacter)
        {
            if(skinnedMeshData) skinnedMeshData.CollectBonesFrom(modularCharacter.hierarchyIndex);
        }


        public void OnBeforeSerialize()
        {
            List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>(Slots);
            Slots.Clear();
            foreach (EquipmentSlot equipmentSlot in equipmentSlots)
            {
                if(equipmentSlot != null && !Slots.Contains(equipmentSlot))
                    Slots.Add(equipmentSlot);
            }
        
        
            List<EquipmentTag> tags = new List<EquipmentTag>(Tags);
            Tags.Clear();
            foreach (var tag in tags)
            {
                if(tag != null && !Tags.Contains(tag))
                    Tags.Add(tag);
            }
        }

        public void OnAfterDeserialize()
        {
        }
    }
}