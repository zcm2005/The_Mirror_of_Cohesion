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

    public abstract class NBTNode
    {

        public abstract byte TypeIndex { get; }

        public abstract byte[] ToBytes();

        internal NBTNodeType Type { private init; get; }

        protected NBTNode() { Type = (NBTNodeType)TypeIndex; }

        public NBTNode? Parent;



    }

    public class NBTNodeEnd : NBTNode
    {
        public NBTNodeEnd() : base()
        {
        }

        public override sealed byte TypeIndex => 0;


        public override byte[] ToBytes()
        {
            return new byte[1] { 0 };
        }

        public static bool operator ==(NBTNodeEnd a, NBTNodeEnd b) => true;
        public static bool operator !=(NBTNodeEnd a, NBTNodeEnd b) => false;

    }

    public abstract class NBTNodeData<T> : NBTNode
    {

        public string Name;


        public NBTNodeData(string name, T data) : base()
        {
            Value = data;
            Name = name;
        }

        public virtual T Value { get; set; }

        protected byte[] PreBytes()
        {
            if (Name == null)
            {
                return new byte[2] { 0, 0 };

            }
            byte[] bytes = new byte[3 + Name.Length];
            bytes[0] = TypeIndex;
            bytes[1] = ((byte)(Name.Length >> 8));
            bytes[2] = ((byte)(Name.Length));
            Encoding.UTF8.GetBytes(Name).CopyTo(bytes, 3);
            return bytes;
        }

        public abstract byte[] GetBytesForList();

    }


    public class NBTNodeByte : NBTNodeData<byte>
    {
        public override sealed byte TypeIndex => 1;

        public static bool operator ==(NBTNodeByte a, NBTNodeByte b) => (a.Name == b.Name && a.Value == b.Value);
        public static bool operator !=(NBTNodeByte a, NBTNodeByte b) => (a.Name != b.Name || a.Value != b.Value);


        public NBTNodeByte(string name, byte data) : base(name, data) { }

        public override byte[] GetBytesForList() => BitConverter.GetBytes(Value);

        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] result = new byte[pre.Length + 1];
            pre.CopyTo(result, 0);
            result[result.Length - 1] = Value;
            return result;
        }
    }

    public class NBTNodeShort : NBTNodeData<short>
    {
        public override sealed byte TypeIndex => 2;

        public static bool operator ==(NBTNodeShort a, NBTNodeShort b) => (a.Name == b.Name && a.Value == b.Value);
        public static bool operator !=(NBTNodeShort a, NBTNodeShort b) => (a.Name != b.Name || a.Value != b.Value);

        public NBTNodeShort(string name, short data) : base(name, data) { }

        public override byte[] GetBytesForList() => BitConverter.GetBytes(Value);


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] data = BitConverter.GetBytes(Value);
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }

    }

    public class NBTNodeInt : NBTNodeData<int>
    {
        public static bool operator ==(NBTNodeInt a, NBTNodeInt b) => (a.Name == b.Name && a.Value == b.Value);
        public static bool operator !=(NBTNodeInt a, NBTNodeInt b) => (a.Name != b.Name || a.Value != b.Value);

        public NBTNodeInt(string name, int data) : base(name, data) { }
        public override byte[] GetBytesForList() => BitConverter.GetBytes(Value);


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] data = BitConverter.GetBytes(Value);
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }


        public override sealed byte TypeIndex => 3;

    }

    public class NBTNodeLong : NBTNodeData<long>
    {
        public override byte[] GetBytesForList() => BitConverter.GetBytes(Value);

        public static bool operator ==(NBTNodeLong a, NBTNodeLong b) => (a.Name == b.Name && a.Value == b.Value);
        public static bool operator !=(NBTNodeLong a, NBTNodeLong b) => (a.Name != b.Name || a.Value != b.Value);

        public override sealed byte TypeIndex => 4;

        public NBTNodeLong(string name, long data) : base(name, data) { }


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] data = BitConverter.GetBytes(Value);
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }

    }

    public class NBTNodeFloat : NBTNodeData<float>
    {

        public override byte[] GetBytesForList() => BitConverter.GetBytes(Value);

        public static bool operator ==(NBTNodeFloat a, NBTNodeFloat b) => (a.Name == b.Name && a.Value == b.Value);
        public static bool operator !=(NBTNodeFloat a, NBTNodeFloat b) => (a.Name != b.Name || a.Value != b.Value);

        public NBTNodeFloat(string name, float data) : base(name, data) { }


        public override sealed byte TypeIndex => 5;


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] data = BitConverter.GetBytes(Value);
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }
    }

    public class NBTNodeDouble : NBTNodeData<double>
    {

        public override byte[] GetBytesForList() => BitConverter.GetBytes(Value);

        public static bool operator ==(NBTNodeDouble a, NBTNodeDouble b) => (a.Name == b.Name && a.Value == b.Value);
        public static bool operator !=(NBTNodeDouble a, NBTNodeDouble b) => (a.Name != b.Name || a.Value != b.Value);


        public NBTNodeDouble(string name, float data) : base(name, data) { }

        public override sealed byte TypeIndex => 6;


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] data = BitConverter.GetBytes(Value);
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }
    }

    public abstract class NBTNodeDataCollection<T> : NBTNodeData<List<T>>, ICollection<T>
    {
        protected NBTNodeDataCollection(string name, List<T> data) : base(name, data)
        {
        }

        protected NBTNodeDataCollection(string name) : this(name, new List<T>()) { }


        public static bool operator ==(NBTNodeDataCollection<T> a, NBTNodeDataCollection<T> b) => Enumerable.SequenceEqual(a.Value, b.Value);

        public static bool operator !=(NBTNodeDataCollection<T> a, NBTNodeDataCollection<T> b) => !(a == b);


        public int Count => Value.Count;

        public bool IsReadOnly => false;

        public virtual void Add(T item) => Value.Add(item);
        public void Clear() => Value.Clear();
        public bool Contains(T item) => Value.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => Value.CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => Value.GetEnumerator();
        public bool Remove(T item) => Value.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public abstract class NBTNodeDataArray<T> : NBTNodeDataCollection<T>, IList<T>
    {
        protected NBTNodeDataArray(string name, List<T> data) : base(name, data)
        {
        }

        public T this[int index]
        {
            set => Value[index] = value;
            get => Value[index];
        }

        public int IndexOf(T item) => Value.IndexOf(item);
        public void Insert(int index, T item) => Value.Insert(index, item);
        public void RemoveAt(int index) => Value.RemoveAt(index);

    }

    public class NBTNodeByteArray : NBTNodeDataArray<byte>
    {
        public NBTNodeByteArray(string name, List<byte> data) : base(name, data) { }
        public NBTNodeByteArray(string name) : this(name, new List<byte>()) { }



        public override sealed byte TypeIndex => 7;


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] size;
            size = new NBTNodeInt("size", Value.Count).ToBytes();
            byte[] result = new byte[pre.Length + size.Length + Value.Count];
            pre.CopyTo(result, 0);
            size.CopyTo(result, pre.Length);
            Value.CopyTo(result, pre.Length + size.Length);
            return result;
        }

        public override byte[] GetBytesForList()
        {
            byte[] size;
            size = new NBTNodeInt("size", Value.Count).ToBytes();
            byte[] result = new byte[size.Length + Value.Count];
            size.CopyTo(result, 0);
            Value.CopyTo(result, size.Length);
            return result;
        }
    }

    public class NBTNodeString : NBTNodeData<string>
    {
        public NBTNodeString(string name, string data) : base(name, data) { }


        public override sealed byte TypeIndex => 8;

        public override byte[] GetBytesForList()
        {
            byte[] length;
            byte[] content;
            content = Encoding.UTF8.GetBytes(Value);
            length = new NBTNodeShort("length", (short)content.Length).ToBytes();
            byte[] result = new byte[length.Length + content.Length];
            length.CopyTo(result, 0);
            content.CopyTo(result, length.Length);
            return result;
        }

        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] length;
            byte[] content;
            content = Encoding.UTF8.GetBytes(Value);
            length = new NBTNodeShort("length", (short)content.Length).ToBytes();
            byte[] result = new byte[pre.Length + length.Length + content.Length];
            pre.CopyTo(result, 0);
            length.CopyTo(result, pre.Length);
            content.CopyTo(result, pre.Length + length.Length);
            return result;
        }
    }

    public class NBTNodeIntArray : NBTNodeDataArray<int>
    {


        public NBTNodeIntArray(string name, List<int> data) : base(name, data) { }
        public NBTNodeIntArray(string name) : this(name, new List<int>()) { }


        public override sealed byte TypeIndex => 11;

        public override byte[] GetBytesForList()
        {
            byte[] size;
            size = new NBTNodeInt("size", Value.Count).ToBytes();
            byte[] result = new byte[size.Length + Value.Count * 4];
            size.CopyTo(result, 0);
            for (int i = 0; i < Value.Count; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(result, size.Length + i * 4);
            }
            return result;
        }

        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] size;
            size = new NBTNodeInt("size", Value.Count).ToBytes();
            byte[] result = new byte[pre.Length + size.Length + Value.Count * 4];
            pre.CopyTo(result, 0);
            size.CopyTo(result, pre.Length);
            for (int i = 0; i < Value.Count; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(result, pre.Length + size.Length + i * 4);
            }
            return result;
        }
    }

    public class NBTNodeLongArray : NBTNodeDataArray<long>
    {
        public NBTNodeLongArray(string name, List<long> data) : base(name, data) { }
        public NBTNodeLongArray(string name) : this(name, new List<long>()) { }


        public override sealed byte TypeIndex => 12;

        public override byte[] GetBytesForList()
        {
            byte[] size;
            size = new NBTNodeInt("size", Value.Count).ToBytes();
            byte[] result = new byte[size.Length + Value.Count * 8];
            size.CopyTo(result, 0);
            for (int i = 0; i < Value.Count; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(result, size.Length + i * 8);
            }
            return result;
        }


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] size;
            size = new NBTNodeInt("size", Value.Count).ToBytes();
            byte[] result = new byte[pre.Length + size.Length + Value.Count * 8];
            pre.CopyTo(result, 0);
            size.CopyTo(result, pre.Length);
            for (int i = 0; i < Value.Count; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(result, pre.Length + size.Length + i * 8);
            }
            return result;
        }
    }


    internal enum NBTNodeType
    {
        End = 0,
        Byte = 1,
        Short = 2,
        Int = 3,
        Long = 4,
        Flote = 5,
        Double = 6,
        Byte_Array = 7,
        String = 8,
        List = 9,
        Compound = 10,
        Int_Array = 11,
        Long_Array = 12
    }

    public class WrongDataTypeException : Exception { }

    public class ManagedValueException : Exception { }
}
