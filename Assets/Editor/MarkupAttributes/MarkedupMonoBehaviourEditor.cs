using UnityEditor;
using UnityEngine;
using MarkupAttributes.Editor;

[CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
internal class MarkedupMonoBehaviourEditor : MarkedUpEditor
{
}