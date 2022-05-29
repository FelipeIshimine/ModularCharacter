using UnityEngine;

namespace ModularCharacters
{
    [ExecuteAlways]
    public class MaterialSwapper : MonoBehaviour
    {
        public Material mat;

        [SerializeField]private Material _activeMat;
    
        public void Start()
        {
            ApplyMaterial();
        }

        public void Update()
        {
            if (_activeMat != mat)
            {
                _activeMat = mat;
                ApplyMaterial();
            }
        }
    
        private void ApplyMaterial()
        {
            var renderers = GetComponentsInChildren<Renderer>(true);
            foreach (var renderer in renderers)
            {
                renderer.material = mat;
            }
        }
    }
}
