using System.Collections.Generic;
using ModularCharacters.AddressableAssets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ModularCharacters.UI
{
    public class CanvasModularCharacter : MonoBehaviour
    {
        public OptionButtonUI buttonPrefab;
        public ToggleButtonUI toggleButtonUIPrefab;

        public ModularCharacter ModularCharacter;
    
        [field:SerializeField] public CharacterModuleCollection CharacterModuleCollection { get; private set; }

        private Queue<OptionButtonUI> pool = new Queue<OptionButtonUI>(); 

        public RectTransform slotsContainer;
        public RectTransform elementContainer;
        public RectTransform tagsContainer;

        public List<OptionButtonUI> slotsButtons = new List<OptionButtonUI>();
        public List<OptionButtonUI> elementButtons = new List<OptionButtonUI>();
        public List<OptionButtonUI> tagsButtons = new List<OptionButtonUI>();

        private List<EquipmentTag> _allTags = new List<EquipmentTag>();

        private List<EquipmentTag> _activeTags = new List<EquipmentTag>();

        private EquipmentSlot _activeSlot;

        public ModularCharacterPreset defaultPresetValues;

        private Dictionary<EquipmentSlot, CharacterModule> _defaultValues;

        private OptionButtonUI GetNext() => pool.Count > 0 ? pool.Dequeue() : Instantiate(buttonPrefab);

        private void Start()
        {
            if(ModularCharacter) 
                Initialize(ModularCharacter);
        }
    
        [Button]
        public void Initialize(ModularCharacter modularCharacter)
        {
            ModularCharacter = modularCharacter;

            InitializeDefaultValues();

            _allTags = new List<EquipmentTag>(CharacterModuleCollection.GetAllTags());
            _activeTags = new List<EquipmentTag>(_allTags.Count);

            foreach (var equipmentTag in _allTags)
            {
                var toggleButtonUI = Instantiate(toggleButtonUIPrefab, tagsContainer);
                toggleButtonUI.Initialize(equipmentTag.name, false, x => ToggleTag(equipmentTag, x));
            }
        
            ClearElementButtons();
            ClearSlotButtons();
            ClearTagsButtons();

            InitializeSlots();

            foreach (var pair in _defaultValues)
                ModularCharacter.EquipPrefab(pair.Value);
        }

        private void InitializeDefaultValues()
        {
            _defaultValues = new Dictionary<EquipmentSlot, CharacterModule>();
            foreach (var characterModule in defaultPresetValues.CharacterModules)
            {
                if (characterModule.Slots.Count != 1)
                {
                    Debug.LogWarning(
                        $"Careful, the module {characterModule.Id} in {defaultPresetValues.name} is not valid for default values, it has {characterModule.Slots} slots assigned.");
                    continue;
                }
                _defaultValues.Add(characterModule.Slots[0], characterModule);
            }
        }

        private void ToggleTag(EquipmentTag equipmentTag, bool value)
        {
            if (value)
            {
                if (!_activeTags.Contains(equipmentTag))
                    _activeTags.Add(equipmentTag);
            }
            else
                _activeTags.Remove(equipmentTag);
        
            SelectSlot(_activeSlot);
        }


        private void InitializeSlots()
        {
            var values = CharacterModuleCollection.GetSlotsIndex();

            foreach (var pair in values)
            {
                var button = GetNext();
                button.Initialize(pair.Key.name, () => SelectSlot(pair.Key));
                button.transform.SetParent(slotsContainer);
                button.gameObject.SetActive(true);
                button.transform.localScale = Vector3.one;
                slotsButtons.Add(button);
            }

            if (slotsButtons.Count > 1) slotsButtons[0].Press();
        }

    
        public void SelectSlot(EquipmentSlot slot)
        {
            _activeSlot = slot;
            ClearElementButtons();

            var button = GetNext();
            button.Initialize("None", ()=> EquipDefault(slot) );
            
            button.transform.SetParent(elementContainer);
            button.gameObject.SetActive(true);
            button.transform.localScale = Vector3.one;
            elementButtons.Add(button);

            var elements = CharacterModuleCollection.GetFromSlot(slot);
            foreach (AssetReferenceCharacterModule characterModule in elements)
            {
                if(_activeTags.Count > 0)
                {
                    bool skip = true;
                    foreach (var equipmentTag in characterModule.Tags)
                    {
                        if (_activeTags.Contains(equipmentTag))
                        {
                            skip = false;
                            break;
                        }
                    }

                    if (skip) continue;
                }
            
                button = GetNext();
                button.Initialize(characterModule.Name,()=> ModularCharacter.EquipPrefabAsync(characterModule));
                button.transform.SetParent(elementContainer);
                button.gameObject.SetActive(true);
                button.transform.localScale = Vector3.one;
                elementButtons.Add(button);
            }
        }

        private void EquipDefault(EquipmentSlot slot)
        {
            if(_defaultValues.TryGetValue(slot, out var value))
                ModularCharacter.EquipPrefab(value);
            else
                ModularCharacter.Unequip(slot);
        }

        private void ClearElementButtons()
        {
            for (var index = elementButtons.Count - 1; index >= 0; index--)
                Enqueue(elementButtons[index]);
            elementButtons.Clear();
        }
        private void ClearSlotButtons()
        {
            for (int index = slotsButtons.Count - 1; index >= 0; index--)
                Enqueue(slotsButtons[index]);
            slotsButtons.Clear();
        }
    
        private void ClearTagsButtons()
        {
            for (int index = tagsButtons.Count - 1; index >= 0; index--)
                Enqueue(tagsButtons[index]);
            tagsButtons.Clear();
        }
    
        private void Enqueue(OptionButtonUI slotsButton)
        {
            slotsButton.gameObject.SetActive(false);
            slotsButton.transform.SetParent(transform);
            pool.Enqueue(slotsButton);
        }
    }
}