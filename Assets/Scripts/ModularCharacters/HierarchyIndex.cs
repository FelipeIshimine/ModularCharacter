using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class HierarchyIndex : MonoBehaviour
{
    private Dictionary<string, Transform> _nameToTransform;

    public Dictionary<string, Transform> NameToTransform
    {
        get
        {
            if(_nameToTransform == null)
                Initialize();
            return _nameToTransform;
        }
    }
    
    [Button]
    private void Initialize()
    {
        _nameToTransform = new Dictionary<string, Transform>();
        var childrenTransform= GetComponentsInChildren<Transform>(true);
        foreach (Transform cTransform in childrenTransform)
        {
            _nameToTransform.Add(cTransform.name,cTransform);
        }
    }

    public Transform Get(string rootBone) => NameToTransform[rootBone];
}