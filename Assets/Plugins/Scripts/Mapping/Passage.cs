using System;
using System.Reflection;
using UnityEngine;
using XNode;

namespace Thuleanx.Mapping {
	[NodeTint(0f, 0.4f, .5f)]
	public class Passage : Node {
		public new string name;
		public bool LoadBoth = false;

		[Input(connectionType = ConnectionType.Override)] public int In;
		[Output(connectionType = ConnectionType.Override)] public int Out;

		public override object GetValue(NodePort port) {
			return GetPort(port.fieldName);
		}

		Room _From, _To;
		public Room From {
			get {
				if (_From == null) RecalculateConnections();
				return _From;
			}
		}  
		public Room To {
			get {
				if (_To == null) RecalculateConnections();
				return _To;
			} 
		}

		public Room GetOther(Room room) => room == From ? To : From;

		public override void OnCreateConnection(NodePort from, NodePort to) {
			RecalculateConnections();
		}
		public override void OnRemoveConnection(NodePort port) {
			RecalculateConnections();
		}

		public void RecalculateConnections() {
			_From = GetPort("In").Connection.node as Room;
			_To = GetPort("Out").Connection.node as Room;
		}


	}
}
