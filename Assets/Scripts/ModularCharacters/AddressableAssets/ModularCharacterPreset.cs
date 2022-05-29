using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ModularCharacters.AddressableAssets
{
    [CreateAssetMenu(menuName = "ModularCharacters/CharacterPreset", fileName = "CharacterPreset", order = 0)]
    public class ModularCharacterPreset : ScriptableObject
    {
        [field:SerializeField] public List<CharacterModule> CharacterModules { get; private set; }= new List<CharacterModule>();


        
        #if UNITY_EDITOR
        [SerializeField] private CharacterModuleCollection collection;
        public void Set(List<string> ids)
        {
            var idIndex = collection.GetIdIndex();
            CharacterModules.Clear();

            foreach (string id in ids)
                CharacterModules.Add(idIndex[id].AssetReference.editorAsset.GetComponent<CharacterModule>());
        }

        [Button]
        public void SaveFromScene()
        {
#if UNITY_EDITOR
            bool save = true;
            if (CharacterModules.Count > 0)
                save = UnityEditor.EditorUtility.DisplayDialog("Save", "Override preset?", "Override", "Cancel");

            if (save)
            {
                var character = FindObjectOfType<ModularCharacter>();
                if(character) character.SaveTo(this);
            }
#endif
        }
        
        [Button]
        public void LoadIntoScene()
        {
            var character = FindObjectOfType<ModularCharacter>();
            if (!character) return;
            foreach (CharacterModule characterModule in CharacterModules)
                character.EquipPrefab(characterModule);
        }
        #endif
    }
}