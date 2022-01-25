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
    public class NBTNodeList : NBTNodeData, IList<NBTNode>
    {
        protected List<NBTNodeData> children;
        public NBTNode this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override object Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(NBTNode item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(NBTNode item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(NBTNode[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<NBTNode> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override byte GetTypeIndex() => 9;

        public int IndexOf(NBTNode item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, NBTNode item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(NBTNode item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public override byte[] ToBytes()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
