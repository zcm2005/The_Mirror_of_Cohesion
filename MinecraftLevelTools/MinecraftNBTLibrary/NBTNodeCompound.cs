using System;
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
    /// 储存其他NBT节点的NBT节点
    /// </summary>
    public abstract class NBTNodeCompoundBase : NBTNodeDataArray<NBTNode>
    {
        public NBTNodeCompoundBase(string name) : base(name)
        {
        }

        public NBTNodeCompoundBase(string name, List<NBTNode> data) : base(name, data)
        {
        }

        private static void CheckIsData(NBTNode item)
        {
            if (item.Type == NBTNodeType.End)
                throw new WrongDataTypeException();
        }

        /*private void CheckIsData(NBTNode item)
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

}*/

        public override List<NBTNode> Value
        {
            get => base.Value;
            set
            {
                if(value == null)
                {
                    throw new WrongDataTypeException();
                }
                if (Value != null)
                    Value.Clear();
                foreach (var item in value)
                {
                    Add(item);
                }
                base.Value = value;
            }
        }


        public override void Add(NBTNode item)
        {
            CheckIsData(item);
            base.Add(item);
        }

    }
    public class NBTNodeCompound : NBTNodeCompoundBase
    {
        public NBTNodeCompound(string name) : base(name)
        {
        }

        public NBTNodeCompound(string name, List<NBTNode> data) : base(name, data)
        {
        }



        internal sealed override byte TypeIndex => 10;

        public override byte[] ToBytes()
        {
            int loadlength = 0;
            byte[] pre = GetPreBytes();
            byte[][] load = new byte[Value.Count][];
            for (int i = 0; i < Value.Count; i++)
            {
                load[i] = Value[i].ToBytes();
                loadlength += load[i].Length;
            }
            byte[] result = new byte[pre.Length + loadlength + 1];
            pre.CopyTo(result, 0);
            int pos = pre.Length;
            for (int i = 0; i < Value.Count; i++)
            {
                load[i].CopyTo(result, pos);
                pos += load[i].Length;
            }
            result[result.Length - 1] = 0;//NBTNodeEnd
            return result;
        }

        public override byte[] GetBytesForList()
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
