using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModularCharacters.UI
{
    public class OptionButtonUI : MonoBehaviour
    {
        private event Action OnPressedCallback;
    
        [field:SerializeField] public Button Btn { get; private set; }
        [field:SerializeField] public TextMeshProUGUI Text { get; private set; }

        public void Initialize(string text, Action callback)
        {
            Text.text = text;
            OnPressedCallback = callback;
            name = text;
        }
    
        public void Press() => OnPressedCallback?.Invoke();
    }
}