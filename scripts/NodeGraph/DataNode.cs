using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystallizeData
{
	public class DataNode
	{
		public int NodeID { get; set; }
		public int ItemID { get; set; }
		public List<DataNode> Dependencies { get; set; }
		public string type;

		public DataNode() {
			NodeID = 0;
			Dependencies = new List<DataNode> ();
			type = null;
			ItemID = 0;
		}

		public DataNode(int nodeID, List<DataNode> depends) {
			NodeID = nodeID;
			Dependencies = depends;
			type = null;
			ItemID = 0;
		}

		public void RemoveFromDependencies(int nodeID) {
		//public DataNode DependencyNodeWithID (int nodeID) {
			int index = Dependencies.FindIndex(x => (x.NodeID == nodeID));
			
			if (index > 0)
				Dependencies.RemoveAt(index);

			/*var temp = new DataNode ();
			foreach (var node in Dependencies) {
				if (node.NodeID == nodeID) {
					temp = node;
					break;
				}
			}
			if (temp.NodeID == nodeID) {
				Dependencies.Remove (temp);
			}*/
			//return null;
		}

		public List<List<DataNode>> GetNodeLayout(List<DataNode> nodes){
			var layout = new List<List<DataNode>> ();
			foreach (var node in nodes) {
				var depth = GetDepth(node);

				while(layout.Count <= depth){
					layout.Add(new List<DataNode>());
				}
				layout[depth].Add(node);
			}
			return layout;
		}

		int GetDepth(DataNode node){
			var closed = new HashSet<int> ();
			var open = new Queue<DataNode> ();

			var depths = new Dictionary<int, int> ();
			depths[node.NodeID] = 0;

			open.Enqueue (node);

			while(open.Count > 0){
				var n = open.Dequeue();
				var depth = depths[n.NodeID];
				closed.Add(n.NodeID);

				if(n.Dependencies.Count == 0){
					return depth;
				} else {
					foreach(var d in n.Dependencies){
						if(!closed.Contains(d.NodeID)){
							open.Enqueue(d);
							depths[n.NodeID] = depth + 1;
						}
					}
				}
			}
			return 0;
		}

		/*public bool CanGoTo(PlayerDialogueTreeTraversed P)
		{
			foreach (var node in Dependencies){
				if (P.IsTraversed(node.NodeID)) {
					return true;
				}
			}
			return false;
		}*/
	}
}
