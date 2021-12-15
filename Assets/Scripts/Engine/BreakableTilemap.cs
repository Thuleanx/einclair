using UnityEngine;
using UnityEngine.Tilemaps;

namespace Thuleanx.Engine.Core {
	[RequireComponent(typeof(Tilemap))]
	[RequireComponent(typeof(Collider2D))]
	public class BreakableTilemap : MonoBehaviour {
		public Tilemap Map { get; private set; }
		public Collider2D Collider { get; private set; }
		public LayerMask DestructionLayer;

		void Awake() {
			Map = GetComponent<Tilemap>();
		}

		void OnCollisionEnter2D(Collision2D other) {
			if ((DestructionLayer.value & (1 << other.gameObject.layer)) != 0) {
				Vector3 hitPosition = Vector3.zero;
				foreach (ContactPoint2D hit in other.contacts) {
					hitPosition.x = hit.point.x - 0.1f;
					hitPosition.y = hit.point.y - 0.1f;
					Map.SetTile(Map.WorldToCell(hitPosition), null);
				}
			}
		}
	}
}