using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Thuleanx.Optimization {
	[CreateAssetMenu(fileName = "Pool", menuName = "~/Optimization/BubblePool", order = 0)]
	public class BubblePool : ScriptableObject {
		public GameObject prefab;

		[HideInInspector] 
		public Dictionary<string, HashSet<Bubble>> Ledger = new Dictionary<string, HashSet<Bubble>>();
		[HideInInspector] public Queue<Bubble> Pool = new Queue<Bubble>();
		[HideInInspector] public HashSet<Bubble> Borrowed = new HashSet<Bubble>();

		bool _Activated;

		void Awake() {
			_Activated = false;
		}

		void OnEnable() {
			_Activated = false;
			Ledger.Clear();
			Pool.Clear();
			Borrowed.Clear();
		}

		void BeforeSceneUnload(Scene scene) {
			CollectsAll(scene);
		}
		public void TryInit() {
			if (!_Activated) {
				try {
					BubbleManager.Instance.ActivePools.Add(this);
					App.BeforeSceneUnload.AddListener(BeforeSceneUnload);
				} catch {
					Debug.Log("Bubble is loaded");
				}
			} 
			_Activated = true;
		}
		public GameObject Borrow(Scene scene) => Borrow(scene, Vector3.zero);
		public GameObject Borrow(Scene scene, Vector3 position) => Borrow(scene, position, Quaternion.identity);
		public GameObject Borrow(Scene scene, Vector3 position, Quaternion rotation) {
			TryInit();
			if (!Ledger.ContainsKey(scene.name))
				Ledger[scene.name] = new HashSet<Bubble>();

			if (Pool.Count == 0) Expand();

			Bubble bubble = Pool.Dequeue();
			bubble.gameObject.transform.position = position;
			bubble.gameObject.transform.rotation = rotation;
			bubble.gameObject.SetActive(true);

			foreach (var gameObject in scene.GetRootGameObjects())
				if (gameObject.name == "_Dynamic")
					bubble.gameObject.transform.SetParent(gameObject.transform);
			if (bubble.gameObject.transform.parent == null)
				SceneManager.MoveGameObjectToScene(bubble.gameObject, scene);
			bubble.InPool = false;
			bubble.scene = scene;

			Ledger[scene.name].Add(bubble);

			return bubble.gameObject;
		}
		public void Collects(Bubble bubble,  bool remove = true) {
			if (!bubble.InPool) {
				bubble.transform.SetParent(null);
				bubble.gameObject.SetActive(false);
				Pool.Enqueue(bubble);
				DontDestroyOnLoad(bubble.gameObject);
				bubble.InPool = true;
				if (remove) Ledger[bubble.scene.name].Remove(bubble);
			}
		}
		public void CollectsAll(Scene scene) {
			Debug.Log("Collecting all from " + scene.name);
			foreach (Bubble bubble in Ledger[scene.name])
				if (!bubble.InPool)
					Collects(bubble, false);
			Ledger.Remove(scene.name);
		}
		public void BubbleLoss(Bubble bubble) {
			if (BubbleManager.Instance != null)
				BubbleManager.Instance.StartCoroutine(_AttemptRecover(bubble));
		}
		IEnumerator _AttemptRecover(Bubble bubble) {
			yield return null;
			if (!bubble.InPool) {
				try {
					Collects(bubble);
				} catch {
					// Give up and pop the bubble forever
					if (bubble && Ledger.ContainsKey(bubble.gameObject.scene.name))
						Ledger[bubble.gameObject.scene.name].Remove(bubble);
				}
			}
		}

		void Expand() => Expand(Mathf.Max(Borrowed.Count, 1));
		void Expand(int count) {
			while (count-->0) {
				GameObject obj = Instantiate(prefab, Vector2.zero, Quaternion.identity);
				obj.SetActive(false);
				DontDestroyOnLoad(obj);
				Bubble bubble = obj.GetComponent<Bubble>();
				if (bubble == null) bubble = obj.AddComponent<Bubble>();
				bubble.Pool = this;
				bubble.InPool = true;
				Pool.Enqueue(bubble);
			}
		}
		public void Shrink() {
			int count = 0;
			foreach (var kvp in Ledger)
				count += kvp.Value.Count;
			if (count >= 3*Borrowed.Count) {
				int desiredSz = (Pool.Count + Borrowed.Count) / 4;
				while (Pool.Count + Borrowed.Count > desiredSz) {
					Bubble bubble = Pool.Dequeue();
					Destroy(bubble.gameObject);
				}
			}
		}
	}
}