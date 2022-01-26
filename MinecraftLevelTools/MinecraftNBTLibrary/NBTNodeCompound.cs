using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftNBTLibrary
{
    public class NBTNodeCompound : NBTNodeDataCollection<NBTNode>
    {
        public NBTNodeCompound(string name) : base(name)
        {
        }

        public NBTNodeCompound(string name, List<NBTNode> data) : base(name, data)
        {
        }

        public override List<NBTNode> Value
        {
            get => base.Value;
            set
            {
                Value.Clear();
                foreach (var item in value)
                {
                    Add(item);
                }

            }
        }

        public override void Add(NBTNode item)
        {
            CheckIsData(item);
            base.Add(item);
        }

        private void CheckIsData(NBTNode item)
        {
            bool flag = false;
            Type t;
            t = item.GetType();
            while (t != typeof(object))
            {
                if (t.GetGenericTypeDefinition() == typeof(NBTNodeData<>))
                {
                    flag = true;
                }
                t = t.BaseType;
            }
            if (!flag)
            {
                Value.Clear();
                throw new WrongDataTypeException();
            }

        }

        public override byte TypeIndex => 10;

        public override byte[] ToBytes()
        {
            int length = 0;
            byte[][] load = new byte[Value.Count][];
            for (int i = 0; i < Value.Count; i++)
            {
                load[i] = Value[i].ToBytes();
                length += load[i].Length;
            }
            byte[] result = new byte[length + 1];
            int pos = 0;
            for (int i = 0; i < Value.Count; i++)
            {
                load[i].CopyTo(result, pos);
                pos += load[i].Length;
            }
            result[result.Length - 1] = 0;//NBTNodeEnd
            return result;
        }
    }
}
