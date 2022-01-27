using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
This file is part of The Mirror of Cohesion.

This product is unofficial and not from Minecraft or approved by Minecraft.

Copyright (C) 2022  ZCM

This program is released under license AGPL-3.0-only.

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License, Version 3.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/


namespace MinecraftNBTLibrary
{

    public class NBTNodeList<T> : NBTNodeDataArray<NBTNodeData<T>>
    {
        public NBTNodeList(string name, List<NBTNodeData<T>> data) : base(name, data)
        {
        }

        public NBTNodeList(string name) : this(name, new List<NBTNodeData<T>>()) { }


        internal sealed override byte TypeIndex => 9;

        public override byte[] ToBytes()
        {
            if (Value.Count == 0)
            {
                throw new EmptyListException();
            }
            byte[] pre = GetPreBytes();
            byte[] tagid = new NBTNodeByte("tagid", Value[0].TypeIndex).ToBytes();
            byte[] size = new NBTNodeInt("size", Value.Count).ToBytes();
            byte[][] load = new byte[Value.Count][];
            int length = 0;
            length += pre.Length;
            length += tagid.Length;
            length += size.Length;
            for (int i = 0; i < Value.Count; i++)
            {
                load[i] = Value[i].GetBytesForList();
                length += load[i].Length;
            }
            byte[] result = new byte[length];
            pre.CopyTo(result, 0);
            tagid.CopyTo(result, pre.Length);
            size.CopyTo(result, pre.Length + tagid.Length);
            int pos = pre.Length + tagid.Length + size.Length;
            for (int i = 0; i < Value.Count; i++)
            {
                load[i].CopyTo(result, pos);
                pos += load[i].Length;
            }
            return result;
        }

        public override byte[] GetBytesForList()
        {
            if (Value.Count == 0)
            {
                throw new EmptyListException();
            }
            byte[] tagid = new NBTNodeByte("tagid", Value[0].TypeIndex).ToBytes();
            byte[] size = new NBTNodeInt("size", Value.Count).ToBytes();
            byte[][] load = new byte[Value.Count][];
            int length = tagid.Length + size.Length;
            for (int i = 0; i < Value.Count; i++)
            {
                load[i] = Value[i].GetBytesForList();
                length += load[i].Length;
            }
            byte[] result = new byte[length];
            tagid.CopyTo(result, 0);
            size.CopyTo(result, tagid.Length);
            int pos = tagid.Length + size.Length;
            for (int i = 0; i < Value.Count; i++)
            {
                load[i].CopyTo(result, pos);
                pos += load[i].Length;
            }
            return result;
        }
    }

    public class EmptyListException : Exception { }
}
