using UnityEditor;
using UnityEngine;
using MarkupAttributes.Editor;

[CustomEditor(typeof(ScriptableObject), true), CanEditMultipleObjects]
internal class MarkedupScriptableObjectEditor : MarkedUpEditor
{
}