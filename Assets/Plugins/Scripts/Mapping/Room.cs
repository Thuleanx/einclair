using UnityEngine;
using XNode;

using System.Collections.Generic;

using Thuleanx.Utils;

namespace Thuleanx.Mapping {
	[NodeTint(.5f, 0.4f, 0f)]
	public class Room : Node {
		public new string name;
		public SceneReference Scene;

		[Input(connectionType = ConnectionType.Multiple)] public int In;
		[Output(connectionType = ConnectionType.Multiple)] public int Out;

		public override object GetValue(NodePort port) {
			return GetPort(port.fieldName);
		}

		public List<Passage> AdjacentPassages {
			get {
				List<Passage> passages = new List<Passage>();
				foreach (var node in graph.nodes) {
					if (node is Passage) {
						Passage passage = node as Passage;
						if (passage.From == this || passage.To == this)
							passages.Add(passage);
					}
				}
				return passages;
			}
		} 
	}
}