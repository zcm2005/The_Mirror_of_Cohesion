using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftNBTLibrary
{
    public class NBTNodeCompound : NBTNodeDataCollection
    {
        public override int Count => throw new NotImplementedException();

        public override bool IsReadOnly => throw new NotImplementedException();

        public override object Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Add(NBTNodeData item)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override bool Contains(NBTNodeData item)
        {
            throw new NotImplementedException();
        }

        public override void CopyTo(NBTNodeData[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<NBTNodeData> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override sealed byte GetTypeIndex() => 10;

        public override bool Remove(NBTNodeData item)
        {
            throw new NotImplementedException();
        }

        public override byte[] ToBytes()
        {
            throw new NotImplementedException();
        }
    }
}
