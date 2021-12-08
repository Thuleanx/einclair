using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Thuleanx.Engine {
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(CompositeCollider2D))]
	public class SetTilemapShadows : MonoBehaviour {
		private CompositeCollider2D tilemapCollider;
		private GameObject shadowCasterContainer;
		private List<GameObject> shadowCasters = new List<GameObject>();
		private List<PolygonCollider2D> shadowPolygons = new List<PolygonCollider2D>();
		private List<ShadowCaster2D> shadowCasterComponents = new List<ShadowCaster2D>();

		private bool doReset = false;
		public SortingLayer SortingLayer;

		void Awake() {
			tilemapCollider = GetComponent<CompositeCollider2D>();
		}

		public void Start() {
			shadowCasterContainer = gameObject;
			UpdateShadows();
		}

		private void Reset() {
			shadowCasters.Clear();
			shadowPolygons.Clear();
			shadowCasterComponents.Clear();

			foreach (Transform child in transform) {
				#if UNITY_EDITOR
					DestroyImmediate(child.gameObject);
				#else
					Destroy(child.gameObject);
				#endif
			}

			for (int i = 0; i < tilemapCollider.pathCount; i++) {
				Vector2[] pathVertices = new Vector2[tilemapCollider.GetPathPointCount(i)];
				tilemapCollider.GetPath(i, pathVertices);
				GameObject shadowCaster = new GameObject("shadow_caster_" + i);
				shadowCasters.Add(shadowCaster);
				PolygonCollider2D shadowPolygon = (PolygonCollider2D)shadowCaster.AddComponent(typeof(PolygonCollider2D));
				shadowPolygons.Add(shadowPolygon);
				shadowCaster.transform.parent = shadowCasterContainer.transform;
				shadowPolygon.points = pathVertices;
				shadowPolygon.enabled = false;
				//if (shadowCaster.GetComponent<ShadowCaster2D>() != null) // remove existing caster?
				//    Destroy(shadowCaster.GetComponent<ShadowCaster2D>());
				ShadowCaster2D shadowCasterComponent = shadowCaster.AddComponent<ShadowCaster2D>();
				shadowCasterComponents.Add(shadowCasterComponent);
				shadowCasterComponent.selfShadows = true;
			}
		}

		private void LateUpdate() {
			if (doReset) {
				Reset();
				doReset = false;
			}
		}

		public void UpdateShadows() {
			doReset = true;
		}

		void OnValidate() {
			UpdateShadows();
		}
	}
}


