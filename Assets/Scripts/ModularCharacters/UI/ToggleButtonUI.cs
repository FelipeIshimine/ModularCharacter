using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModularCharacters.UI
{
    public class ToggleButtonUI : MonoBehaviour
    {
        private event Action<bool> OnPressedCallback;
    
        [field:SerializeField] public Toggle Toggle { get; private set; }
        [field:SerializeField] public TextMeshProUGUI Text { get; private set; }

        public void Initialize(string text, bool startValue, Action<bool> callback)
        {
            Text.text = text;
            OnPressedCallback = callback;
            name = text;
            Toggle.SetIsOnWithoutNotify(startValue);
        }
    
        public void Press(bool value) => OnPressedCallback?.Invoke(value);
    }
}