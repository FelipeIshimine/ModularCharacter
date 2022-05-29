using System;
using ModularCharacters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ModularCharacters
{
    [CreateAssetMenu(menuName = "ModularCharacters/Slots", fileName = "Slot", order = 0)]
    public class EquipmentSlot : ScriptableObject
    {
#if UNITY_EDITOR

        [OnValueChanged(nameof(ApplyToMany))] public CharacterModule[] characterModules;

        public void ApplyTo(CharacterModule characterModule)
        {
            if (!characterModule.Slots.Contains(this))
                characterModule.Slots.Add(this);
        }

        public void ApplyToMany(CharacterModule[] characterModule)
        {
            foreach (CharacterModule module in characterModule)
                ApplyTo(module);

            characterModules = Array.Empty<CharacterModule>();
        }
#endif
    }
}