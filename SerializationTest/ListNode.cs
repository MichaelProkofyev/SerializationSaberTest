using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationTest
{
    class ListNode
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand; //произвольный элемент внутри списка
        public string Data;

        public ListNode(string stringData)
        {
            Data = stringData;
        }
    }
}
