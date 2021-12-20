using UnityEngine;

namespace Thuleanx.Effects.Core {
	[RequireComponent(typeof(Camera))]
	public class PostProcessingUnit : MonoBehaviour {
		public static PostProcessingUnit Instance;
		public Camera RealCamera;
		public Camera CurrentCamera {get; private set; }

		[SerializeField] Material PostProcessingMaterial;
		[SerializeField] string _focal_point = "Focal_Point";
		[SerializeField] string _time_offset = "Time_Offset";
		[SerializeField] string _ripple_active = "Ripple_Active";

		public Vector2 worldFocalPoint;

		void Awake() {
			Instance = this;
			PostProcessingMaterial.SetFloat(_ripple_active, 0f);
			CurrentCamera = GetComponent<Camera>();
		}

		public void StartShockwave(Vector2 position) {
			PostProcessingMaterial.SetFloat(_time_offset, Time.time);
			worldFocalPoint = position;
			UpdateShockwavePosition();
			PostProcessingMaterial.SetFloat(_ripple_active, 1f);
		}

		void Update() {
			UpdateShockwavePosition();
		}

		public void UpdateShockwavePosition() {
			PostProcessingMaterial.SetVector(_focal_point, 
				(Vector2) transform.position + (worldFocalPoint - (Vector2) RealCamera.transform.position) 
					* CurrentCamera.orthographicSize / RealCamera.orthographicSize);
		}
	}
}