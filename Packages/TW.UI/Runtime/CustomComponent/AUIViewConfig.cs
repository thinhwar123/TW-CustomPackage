using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "AUIViewConfig", menuName = "AUIConfig/AUIViewConfig")]
public class AUIViewConfig : ScriptableObject
{
    [field: SerializeField] public RectOffset Padding { get; private set; }
    [field: SerializeField] public float Spacing { get; private set; }
    [field: SerializeField] public float HeaderHeight { get; private set; }
    [field: SerializeField] public float BodyHeight { get; private set; }
    [field: SerializeField] public float FooterHeight { get; private set; }
    
}