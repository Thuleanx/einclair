using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Thuleanx.Optimization {
	[CreateAssetMenu(fileName = "Pool", menuName = "~/Optimization/BubblePool", order = 0)]
	public class BubblePool : ScriptableObject {
		public GameObject prefab;

		[HideInInspector] public Queue<Bubble> Pool = new Queue<Bubble>();
		[HideInInspector] public HashSet<Bubble> Borrowed = new HashSet<Bubble>();
		bool _Active = false;

		void Awake() {
			_Active = false;
		}

		public GameObject Borrow() {
			return Borrow(Vector2.zero, Quaternion.identity);
		}

		public GameObject Borrow(Vector3 position, Quaternion rotation) {
			try {
				SceneManager.activeSceneChanged += OnSceneChange;
			} catch {
				Debug.Log("Pool already added");
			}
			if (Pool.Count==0) Expand();

			Bubble bubble = Pool.Dequeue();
			bubble.gameObject.transform.position = position;
			bubble.gameObject.transform.rotation = rotation;
			bubble.gameObject.SetActive(true);
			bubble.InPool = false;

			if (!_Active) {
				BubbleManager.Instance.ActivePools.Add(this);
				_Active = true;
			}

			Borrowed.Add(bubble);
			return bubble.gameObject;
		}

		void OnSceneChange(Scene prev, Scene nxt) => CollectsAll();

		void Expand() => Expand(Mathf.Max(Borrowed.Count, 1));
		void Expand(int count) {
			while (count-->0) {
				GameObject obj = Instantiate(prefab, Vector2.zero, Quaternion.identity);
				obj.SetActive(false);
				DontDestroyOnLoad(obj);
				Bubble bubble = obj.AddComponent<Bubble>();
				bubble.Pool = this;
				bubble.InPool = true;
				Pool.Enqueue(bubble);
			}
		}

		public void Shrink() {
			Debug.Log(Pool.Count + " " + Borrowed.Count);
			if (Pool.Count >= 3*Borrowed.Count) {
				int desiredSz = (Pool.Count + Borrowed.Count) / 4;
				while (Pool.Count + Borrowed.Count > desiredSz) {
					Bubble bubble = Pool.Dequeue();
					Destroy(bubble.gameObject);
				}
			}
		}

		public void CollectsAll() {
			foreach (Bubble bubble in Borrowed)
				if (!bubble.InPool)
					Collects(bubble, false);
			Borrowed.Clear();
		}

		public void Collects(Bubble bubble, bool remove = true) {
			// Sus code
			bubble.transform.SetParent(null);
			bubble.gameObject.SetActive(false);
			Pool.Enqueue(bubble);
			DontDestroyOnLoad(bubble.gameObject);
			bubble.InPool = true;
			if (remove) Borrowed.Remove(bubble);
		}

		
	}
}