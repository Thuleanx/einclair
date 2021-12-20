using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

using Thuleanx.Utils;

namespace Thuleanx.Mapping {
	public class Loading : MonoBehaviour {
		public SceneReference Next;
		void Start() {
		}
		void Update() {
			try {
				if (FMODUnity.RuntimeManager.HasBanksLoaded)
				{
					Debug.Log("Master Bank Loaded");
					App.Instance.RequestLoad(Next.SceneName);
				} else {
					Debug.Log("Master Bank Not Yet Loaded " + FMODUnity.RuntimeManager.AnyBankLoading());
				}
			} catch (Exception err) {
				Debug.Log(err);
			}

		}
	}
}
