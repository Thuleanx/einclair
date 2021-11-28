using UnityEngine;
using System.Collections.Generic;

namespace Thuleanx.Engine.Core {
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(BoxCollider2D))]
	public class Ladder : MonoBehaviour {
		public float Height = 2f;
		public float Extension = 1f;

		public Vector2 Top => transform.position + Height * transform.up;
		public Vector2 Bot => transform.position;

		public BoxCollider2D Collider {get; private set; }
		public GameObject TopAnchor, BotAnchor;
		public List<SpriteRenderer> Renderers = new List<SpriteRenderer>();

		void Awake() {
			Collider = GetComponent<BoxCollider2D>();
		}

		void Start() {
			AdjustSizes();
		}

		void OnValidate() {
			AdjustSizes();
		}

		public void AdjustSizes() {
			if (Collider == null) Collider = GetComponent<BoxCollider2D>();
			TopAnchor.transform.position = Top;
			BotAnchor.transform.position = Bot;

			Collider.size = new Vector2(Collider.size.x, Height + Extension);
			Collider.offset = new Vector2(Collider.offset.x, Collider.size.y / 2);

			foreach (var Renderer in Renderers)
				Renderer.size = new Vector2(Renderer.size.x, Height + Extension);
		}
	}
}