using UnityEngine;
using UnityEditor;

namespace Thuleanx.Engine {
	[CustomEditor(typeof(SetTilemapShadows))]
	public class SetTilemapShadowsEditor : Editor {
		public override void OnInspectorGUI() {
			serializedObject.Update();
			DrawDefaultInspector();

			SetTilemapShadows STS = (SetTilemapShadows) target;
			if (GUILayout.Button("Recompute Shadows", new GUILayoutOption[]{
				GUILayout.Height(32)} )) {
				
				STS.UpdateShadows();
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
}