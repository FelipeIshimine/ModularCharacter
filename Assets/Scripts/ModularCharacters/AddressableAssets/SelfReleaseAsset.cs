using UnityEngine;

namespace ModularCharacters.AddressableAssets
{
    public class SelfReleaseAsset : MonoBehaviour
    {
        public void Release() => UnityEngine.AddressableAssets.Addressables.ReleaseInstance(gameObject);
    }
}