using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using Thuleanx.Manager.Core;

namespace Thuleanx {
	public class Context : MonoBehaviour {
		public static Context Instance => ctxs.Count > 0 ? ctxs[0] : null;
		static List<Context> ctxs = new List<Context>();
		
		[SerializeField] ReferenceManager _ReferenceManager;
		public static ReferenceManager ReferenceManager => Instance._ReferenceManager;

		void OnEnable() {
			ctxs.Add(this);
		}
		void OnDisable() {
			ctxs.Remove(this);
		}
	}
}