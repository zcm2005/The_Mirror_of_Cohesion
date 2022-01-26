using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
This file is part of MinecraftLevelTools.

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

    public class NBTNodeList : NBTNodeDataArray
    {
        protected readonly List<NBTNodeData> children = new();

        internal readonly NBTNodeType ChildrenType;

        public override object Value
        {
            get => new List<NBTNodeData>(children);
            set
            {
                if (value is IEnumerable<NBTNodeData> t)
                {
                    foreach (var item in t)
                    {
                        if (!NBT.TypeIsRight(ChildrenType, item))
                            throw new WrongDataTypeException();
                    }
                    children.Clear();
                    children.AddRange(t);
                }
                else
                {
                    throw new WrongDataTypeException();
                }
            }
        }


        public override int Count => children.Count;

        public override bool IsReadOnly => false;

        public NBTNodeList(byte type) : this(type, String.Empty) { }

        public NBTNodeList(byte type, string name) : this((NBTNodeType)type, name) { }

        internal NBTNodeList(NBTNodeType type, string name)
        {
            ChildrenType = type;
            Name = name;
        }

        public override NBTNodeData this[int index]
        {
            get => children[index];
            set
            {
                if (value.Type != ChildrenType)
                {
                    throw new WrongDataTypeException();
                }
                children[index] = value;
            }

        }

        public override sealed byte GetTypeIndex() => 9;

        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] tagid = new NBTNodeByte("tagid", (byte)ChildrenType).ToBytes();
            byte[] size = new NBTNodeInt("size", children.Count).ToBytes();
            byte[][] load = new byte[children.Count][];
            int length = 0;
            length += pre.Length;
            length += tagid.Length;
            length += size.Length;
            for (int i = 0; i < children.Count; i++)
            {
                load[i] = children[i].ToBytes();
                length += load[i].Length;
            }
            byte[] result = new byte[length];
            pre.CopyTo(result, 0);
            tagid.CopyTo(result, pre.Length);
            size.CopyTo(result, pre.Length + tagid.Length);
            int pos = pre.Length + tagid.Length + size.Length;
            for (int i = 0; i < children.Count; i++)
            {
                load[i].CopyTo(result, pos);
                pos += load[i].Length;
            }
            return result;
        }

        public override int IndexOf(NBTNodeData item)
        {

            for (int i = 0; i < children.Count; i++)
            {
                if (ReferenceEquals(children[i], item))
                    return i;
            }

        }

        public override void Insert(int index, NBTNodeData item)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

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

        public override bool Remove(NBTNodeData item)
        {
            throw new NotImplementedException();
        }
    }
}
