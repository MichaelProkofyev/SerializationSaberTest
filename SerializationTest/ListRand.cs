using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationTest
{
    class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        private Random random = new Random();

        public ListRand()
        {

        }

        public void Clear()
        {
            Count = 0;
            Head = null;
            Tail = null;
        }

        public void Add(string data)
        {
            ListNode newNode = new ListNode(data);

            if (Head == null)
            {
                Head = newNode;
                Tail = newNode;
                newNode.Rand = newNode;
            }
            else
            {
                int randomNodeIdx = random.Next(Count);
                newNode.Rand = NodeForIndex(randomNodeIdx);

                Tail.Next = newNode;
                Tail.Next.Prev = Tail;
                Tail = Tail.Next;
            }
            Count++;
        }

        public string this[int index]
        {
            get
            {
                ListNode node = NodeForIndex(index);
                if (node == null)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return node.Data;
            }
            set
            {
                ListNode node = NodeForIndex(index);
                if (node == null)
                {
                    throw new ArgumentOutOfRangeException();
                }
                node.Data = value;
            }
        }

        public override string ToString()
        {
            //Get node->idx mapping
            Dictionary<ListNode, int> idxForNodeDict = GetNodesToIndexesMapping();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Length: {0}\n", Count);
            ListNode node = Head;
            while (node != null)
            {
                sb.AppendFormat("( D:'{0}'", node.Data);
                sb.AppendFormat(" Rand:[i:{0} D:'{1}'] ) \n", idxForNodeDict[node.Rand], node.Rand.Data);
                node = node.Next;
            }
            return sb.ToString();
        }

        public void Serialize(FileStream fileStream)
        {
            //Write Number of nodes
            FileReadWriter.Write(fileStream, Count);

            //Get node->idx mapping
            Dictionary<ListNode, int> idxForNodeDict = GetNodesToIndexesMapping();

            ListNode node = Head;
            while (node != null)
            {
                //Write Node data string
                FileReadWriter.Write(fileStream, node.Data);

                //Write random node index
                FileReadWriter.Write(fileStream, idxForNodeDict[node.Rand]);

                node = node.Next;
            }
        }

        public void Deserialize(FileStream fileStream)
        {
            Clear();
            int serializedNodesCount = FileReadWriter.ReadInt32(fileStream);
            int[] randomNodesIndexes = new int[serializedNodesCount];

            for (int nodeIndex = 0; nodeIndex < serializedNodesCount; nodeIndex++)
            {
                //Read Node data string
                string nodeData = FileReadWriter.ReadString(fileStream);

                //Read random node index
                randomNodesIndexes[nodeIndex] = FileReadWriter.ReadInt32(fileStream);

                Add(nodeData);
            }

            //Get idx->node mapping
            Dictionary<int, ListNode> nodeForIdxDict = GetIndexesToNodesMapping();

            //Populate "Rand" node properties
            ListNode node = Head;
            foreach (int randNodeIdx in randomNodesIndexes)
            {
                node.Rand = nodeForIdxDict[randNodeIdx];
                node = node.Next;
            }

        }

        //Pre-calculated node->idx mapping reduces complexity from o(n^2) to o(n)  (Compared to calling IndexForNode for every node)
        private Dictionary<ListNode, int> GetNodesToIndexesMapping()
        {
            Dictionary<ListNode, int> idxForNodeDict = new Dictionary<ListNode, int>();
            ListNode node = Head;
            int nodeIdx = 0;
            while (node != null)
            {
                idxForNodeDict.Add(node, nodeIdx);
                nodeIdx++;
                node = node.Next;
            }
            return idxForNodeDict;
        }

        private Dictionary<int, ListNode> GetIndexesToNodesMapping()
        {
            Dictionary<int, ListNode> nodeForIdxDict = new Dictionary<int, ListNode>();
            ListNode node = Head;
            int nodeIdx = 0;
            while (node != null)
            {
                nodeForIdxDict.Add(nodeIdx, node);
                nodeIdx++;
                node = node.Next;
            }
            return nodeForIdxDict;
        }

        private int IndexForNode(ListNode nodeToSearch)
        {
            int index = 0;
            ListNode node = Head;
            while (node != null)
            {
                if (nodeToSearch == node)
                {
                    return index;
                }
                index++;
                node = node.Next;
            }
            throw new ArgumentNullException();
        }

        private ListNode NodeForIndex(int index)
        {
            ListNode current = Head;
            for (int i = 0; i < index; i++)
            {
                if (current == null)
                    return null;
                current = current.Next;
            }
            return current;
        }
    }
}
