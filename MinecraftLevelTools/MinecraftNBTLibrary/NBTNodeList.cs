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

    /// <summary>
    /// 存有同类型NBT节点的Compound
    /// </summary>
    public class NBTNodeList : NBTNodeCompoundBase
    {
        public NBTNodeList(string name, List<NBTNode> data) : base(name, data)
        {
            if (data.Count > 0)
                for (int i = 1; i < data.Count; i++)
                {
                    if (data[i].TypeIndex != data[0].TypeIndex)
                    {
                        throw new WrongDataTypeException();
                    }
                }
        }

        public NBTNodeList(string name) : this(name, new List<NBTNode>()) { }


        internal sealed override byte TypeIndex => 9;

        public override void Add(NBTNode item)
        {
            if (Value.Count > 0)
            {
                if (item.TypeIndex != Value[0].TypeIndex)
                {
                    throw new WrongDataTypeException();
                }
            }
            base.Add(item);
        }

        public override List<NBTNode> Value
        {
            get => base.Value;
            set
            {
                if (Value != null && Value.Count > 0)
                {
                    foreach (NBTNode item in Value)
                    {
                        if (item.TypeIndex != Value[0].TypeIndex)
                        {
                            throw new WrongDataTypeException();
                        }
                    }
                }
                base.Value = value;
            }
        }


        public override byte[] ToBytes()
        {
            if (Value.Count == 0)
            {
                byte[] pre = GetPreBytes();
                int length = 0;
                length += pre.Length;
                length += 5;
                byte[] result = new byte[length];
                pre.CopyTo(result, 0);
                result[pre.Length] = 0;
                result[pre.Length + 1] = (byte)(Value.Count >> 24);
                result[pre.Length + 2] = (byte)(Value.Count >> 16);
                result[pre.Length + 3] = (byte)(Value.Count >> 8);
                result[pre.Length + 4] = (byte)Value.Count;
                return result;
            }
            else
            {
                byte[] pre = GetPreBytes();
                byte[][] load = new byte[Value.Count][];
                int length = 0;
                length += pre.Length;
                length += 5;
                for (int i = 0; i < Value.Count; i++)
                {
                    load[i] = Value[i].GetBytesForList();
                    length += load[i].Length;
                }
                byte[] result = new byte[length];
                pre.CopyTo(result, 0);
                result[pre.Length] = Value[0].TypeIndex;
                result[pre.Length + 1] = (byte)(Value.Count >> 24);
                result[pre.Length + 2] = (byte)(Value.Count >> 16);
                result[pre.Length + 3] = (byte)(Value.Count >> 8);
                result[pre.Length + 4] = (byte)Value.Count;
                int pos = pre.Length + 5;
                for (int i = 0; i < Value.Count; i++)
                {
                    load[i].CopyTo(result, pos);
                    pos += load[i].Length;
                }
                return result;
            }
        }

        public override byte[] GetBytesForList()
        {
            if (Value.Count == 0)
            {
                throw new EmptyListException();
            }
            byte[] pre = GetPreBytes();
            byte[][] load = new byte[Value.Count][];
            int length = 0;
            length += 5;
            for (int i = 0; i < Value.Count; i++)
            {
                load[i] = Value[i].GetBytesForList();
                length += load[i].Length;
            }
            byte[] result = new byte[length];
            result[0] = Value[0].TypeIndex;
            result[1] = (byte)(Value.Count >> 24);
            result[2] = (byte)(Value.Count >> 16);
            result[ 3] = (byte)(Value.Count >> 8);
            result[ 4] = (byte)Value.Count;
            int pos = pre.Length + 5;
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
