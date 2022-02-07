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
    /// 此为抽象的NBTNode类，是所有NBT节点的父级
    /// The abstract NBTNode class, which is base of all NBTNodes.
    /// </summary>
    public abstract class NBTNode
    {

        /// <summary>
        /// 数值形式的标签类型
        /// </summary>
        internal abstract byte TypeIndex { get; }

        /// <summary>
        /// 将此节点输出为byte[]格式
        /// Return a byte array, which contains all data in this node using standard format.
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToBytes();

        /// <summary>
        /// 枚举形式的标签类型，与数值形式对应
        /// Represent the type of this node using an enum.
        /// Correspond to the TypeIndex.
        /// </summary>
        public NBTNodeType Type { private init; get; }

        protected NBTNode() { Type = (NBTNodeType)TypeIndex; }

        /// <summary>
        /// 表示此节点的父节点
        /// Point to the parent node of this node (if exists).
        /// </summary>
        public NBTNode? Parent;
        
        /// <summary>
        /// 输出不含前缀的byte[]类型节点，用于存储在list节点中
        /// Return a byte array which doesn't contain the pre bytes, used by NBTNodeList.
        /// </summary>
        /// <returns></returns>
        public abstract byte[] GetBytesForList();


    }

    /// <summary>
    /// End节点，仅一字节，为0x00
    /// Only a byte which is 0x00.
    /// </summary>
    public class NBTNodeEnd : NBTNode
    {
        public NBTNodeEnd() : base()
        {
        }

        internal override sealed byte TypeIndex => 0;

        public override byte[] GetBytesForList()
        {
            throw new NotImplementedException();
        }

        public override byte[] ToBytes()
        {
            return new byte[1] { 0 };
        }

        public static bool operator ==(NBTNodeEnd a, NBTNodeEnd b) => true;
        public static bool operator !=(NBTNodeEnd a, NBTNodeEnd b) => false;

    }

    /// <summary>
    /// 表示存有数据的节点，不应也不能再继承此类
    /// Represent a NBT node which contains data.
    /// </summary>
    /// <typeparam name="T">数据的类型，仅能填为支持的几种之一</typeparam>
    public abstract class NBTNodeData<T> : NBTNode
    {

        public string Name;


        public NBTNodeData(string name, T data) : base()
        {
            Value = data;
            Name = name;
        }

        public virtual T Value { get; set; }

        protected byte[] GetPreBytes()
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


    }

    /// <summary>
    /// Byte类型的节点
    /// </summary>
    public class NBTNodeByte : NBTNodeData<byte>
    {
        internal override sealed byte TypeIndex => 1;

        public static bool operator ==(NBTNodeByte a, NBTNodeByte b) => (a.Name == b.Name && a.Value == b.Value);
        public static bool operator !=(NBTNodeByte a, NBTNodeByte b) => (a.Name != b.Name || a.Value != b.Value);


        public NBTNodeByte(string name, byte data) : base(name, data) { }

        public override byte[] GetBytesForList() => BitConverter.GetBytes(Value).Reverse().ToArray();

        public override byte[] ToBytes()
        {
            byte[] pre = GetPreBytes();
            byte[] result = new byte[pre.Length + 1];
            pre.CopyTo(result, 0);
            result[result.Length - 1] = Value;
            return result;
        }
    }

    /// <summary>
    /// Short类型的节点
    /// </summary>
    public class NBTNodeShort : NBTNodeData<short>
    {
        internal override sealed byte TypeIndex => 2;

        public static bool operator ==(NBTNodeShort a, NBTNodeShort b) => (a.Name == b.Name && a.Value == b.Value);
        public static bool operator !=(NBTNodeShort a, NBTNodeShort b) => (a.Name != b.Name || a.Value != b.Value);

        public NBTNodeShort(string name, short data) : base(name, data) { }

        public override byte[] GetBytesForList() => BitConverter.GetBytes(Value).Reverse().ToArray();


        public override byte[] ToBytes()
        {
            byte[] pre = GetPreBytes();
            byte[] data = BitConverter.GetBytes(Value).Reverse().ToArray();
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }

    }

    /// <summary>
    /// Int类型的节点
    /// </summary>
    public class NBTNodeInt : NBTNodeData<int>
    {
        public static bool operator ==(NBTNodeInt a, NBTNodeInt b) => (a.Name == b.Name && a.Value == b.Value);
        public static bool operator !=(NBTNodeInt a, NBTNodeInt b) => (a.Name != b.Name || a.Value != b.Value);

        public NBTNodeInt(string name, int data) : base(name, data) { }
        public override byte[] GetBytesForList() => BitConverter.GetBytes(Value).Reverse().ToArray();


        public override byte[] ToBytes()
        {
            byte[] pre = GetPreBytes();
            byte[] data = BitConverter.GetBytes(Value).Reverse().ToArray();
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }


        internal override sealed byte TypeIndex => 3;

    }

    /// <summary>
    /// Long类型的节点
    /// </summary>
    public class NBTNodeLong : NBTNodeData<long>
    {
        public override byte[] GetBytesForList() => BitConverter.GetBytes(Value).Reverse().ToArray();

        public static bool operator ==(NBTNodeLong a, NBTNodeLong b) => (a.Name == b.Name && a.Value == b.Value);
        public static bool operator !=(NBTNodeLong a, NBTNodeLong b) => (a.Name != b.Name || a.Value != b.Value);

        internal override sealed byte TypeIndex => 4;

        public NBTNodeLong(string name, long data) : base(name, data) { }


        public override byte[] ToBytes()
        {
            byte[] pre = GetPreBytes();
            byte[] data = BitConverter.GetBytes(Value).Reverse().ToArray();
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }

    }

    /// <summary>
    /// 单精度浮点类型的节点
    /// </summary>
    public class NBTNodeFloat : NBTNodeData<float>
    {

        public override byte[] GetBytesForList() => BitConverter.GetBytes(Value).Reverse().ToArray();

        public static bool operator ==(NBTNodeFloat a, NBTNodeFloat b) => (a.Name == b.Name && a.Value == b.Value);
        public static bool operator !=(NBTNodeFloat a, NBTNodeFloat b) => (a.Name != b.Name || a.Value != b.Value);

        public NBTNodeFloat(string name, float data) : base(name, data) { }


        internal override sealed byte TypeIndex => 5;


        public override byte[] ToBytes()
        {
            byte[] pre = GetPreBytes();
            byte[] data = BitConverter.GetBytes(Value).Reverse().ToArray();
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }
    }

    /// <summary>
    /// 双精度浮点类型的节点
    /// </summary>
    public class NBTNodeDouble : NBTNodeData<double>
    {

        public override byte[] GetBytesForList() => BitConverter.GetBytes(Value).Reverse().ToArray();

        public static bool operator ==(NBTNodeDouble a, NBTNodeDouble b) => (a.Name == b.Name && a.Value == b.Value);
        public static bool operator !=(NBTNodeDouble a, NBTNodeDouble b) => (a.Name != b.Name || a.Value != b.Value);


        public NBTNodeDouble(string name, double data) : base(name, data) { }

        internal override sealed byte TypeIndex => 6;


        public override byte[] ToBytes()
        {
            byte[] pre = GetPreBytes();
            byte[] data = BitConverter.GetBytes(Value).Reverse().ToArray();
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }
    }



    /// <summary>
    /// 存有某数组类型数据的节点
    /// </summary>
    /// <typeparam name="T">数组中数据的类型</typeparam>
    public abstract class NBTNodeDataArray<T> : NBTNodeData<List<T>>, IList<T>
    {
        protected NBTNodeDataArray(string name, List<T> data) : base(name, data)
        {
        }
        protected NBTNodeDataArray(string name) : base(name, new List<T>())
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
        public int Count => Value.Count;

        public bool IsReadOnly => false;

        public virtual void Add(T item) => Value.Add(item);
        public void Clear() => Value.Clear();

        /// <summary>
        /// 不建议使用
        /// Not recommended.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item) => Value.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => Value.CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => Value.GetEnumerator();

        /// <summary>
        /// 不建议使用
        /// Not recommended.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item) => Value.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// 表示Byte数组类型的节点
    /// </summary>
    public class NBTNodeByteArray : NBTNodeDataArray<byte>
    {
        public NBTNodeByteArray(string name, List<byte> data) : base(name, data) { }
        public NBTNodeByteArray(string name) : this(name, new List<byte>()) { }



        internal override sealed byte TypeIndex => 7;


        public override byte[] ToBytes()
        {
            byte[] pre = GetPreBytes();
            byte[] result = new byte[pre.Length + 4 + Value.Count];
            pre.CopyTo(result, 0);
            result[pre.Length] = (byte)(Value.Count >> 24);
            result[pre.Length + 1] = (byte)(Value.Count >> 16);
            result[pre.Length + 2] = (byte)(Value.Count >> 8);
            result[pre.Length + 3] = (byte)(Value.Count);
            Value.CopyTo(result, pre.Length + 4);
            return result;
        }

        public override byte[] GetBytesForList()
        {
            byte[] result = new byte[4 + Value.Count];
            result[0] = (byte)(Value.Count >> 24);
            result[1] = (byte)(Value.Count >> 16);
            result[2] = (byte)(Value.Count >> 8);
            result[3] = (byte)(Value.Count);
            Value.CopyTo(result, 4);
            return result;
        }
    }

    /// <summary>
    /// 存有一个字符串的NBT节点
    /// </summary>
    public class NBTNodeString : NBTNodeData<string>
    {
        public NBTNodeString(string name, string data) : base(name, data) { }


        internal override sealed byte TypeIndex => 8;

        public override byte[] GetBytesForList()
        {
            byte[] content;
            content = Encoding.UTF8.GetBytes(Value);
            byte[] result = new byte[2 + content.Length];
            result[1] = (byte)content.Length;
            result[0] = (byte)(content.Length >> 8);
            content.CopyTo(result, 2);
            return result;
        }

        public override byte[] ToBytes()
        {
            byte[] pre = GetPreBytes();
            byte[] content;
            content = Encoding.UTF8.GetBytes(Value);
            byte[] result = new byte[pre.Length + 2 + content.Length];
            pre.CopyTo(result, 0);
            result[pre.Length + 1] = (byte)content.Length;
            result[pre.Length] = (byte)(content.Length >> 8);
            content.CopyTo(result, pre.Length + 2);
            return result;
        }
    }


    /// <summary>
    /// 表示Int数组类型的节点
    /// </summary>
    public class NBTNodeIntArray : NBTNodeDataArray<int>
    {


        public NBTNodeIntArray(string name, List<int> data) : base(name, data) { }
        public NBTNodeIntArray(string name) : this(name, new List<int>()) { }


        internal override sealed byte TypeIndex => 11;

        public override byte[] GetBytesForList()
        {
            byte[] result = new byte[4 + Value.Count * 4];
            result[0] = (byte)(Value.Count >> 24);
            result[1] = (byte)(Value.Count >> 16);
            result[2] = (byte)(Value.Count >> 8);
            result[3] = (byte)(Value.Count);
            for (int i = 0; i < Value.Count; i++)
            {
                BitConverter.GetBytes(Value[i]).Reverse().ToArray().CopyTo(result, 4 + i * 4);
            }
            return result;
        }

        public override byte[] ToBytes()
        {
            byte[] pre = GetPreBytes();
            byte[] result = new byte[pre.Length + 4 + Value.Count * 4];
            pre.CopyTo(result, 0);
            result[pre.Length] = (byte)(Value.Count >> 24);
            result[pre.Length + 1] = (byte)(Value.Count >> 16);
            result[pre.Length + 2] = (byte)(Value.Count >> 8);
            result[pre.Length + 3] = (byte)(Value.Count);
            for (int i = 0; i < Value.Count; i++)
            {
                BitConverter.GetBytes(Value[i]).Reverse().ToArray().CopyTo(result, pre.Length + 4 + i * 4);
            }
            return result;
        }
    }

    /// <summary>
    /// 表示Long数组类型的节点
    /// </summary>
    public class NBTNodeLongArray : NBTNodeDataArray<long>
    {
        public NBTNodeLongArray(string name, List<long> data) : base(name, data) { }
        public NBTNodeLongArray(string name) : this(name, new List<long>()) { }


        internal override sealed byte TypeIndex => 12;

        public override byte[] GetBytesForList()
        {
            byte[] result = new byte[4 + Value.Count * 8];
            result[0] = (byte)(Value.Count >> 24);
            result[1] = (byte)(Value.Count >> 16);
            result[2] = (byte)(Value.Count >> 8);
            result[3] = (byte)(Value.Count);
            for (int i = 0; i < Value.Count; i++)
            {
                BitConverter.GetBytes(Value[i]).Reverse().ToArray().CopyTo(result, 4 + i * 8);
            }
            return result;
        }


        public override byte[] ToBytes()
        {
            byte[] pre = GetPreBytes();
            byte[] result = new byte[pre.Length + 4 + Value.Count * 8];
            pre.CopyTo(result, 0);
            result[pre.Length] = (byte)(Value.Count >> 24);
            result[pre.Length + 1] = (byte)(Value.Count >> 16);
            result[pre.Length + 2] = (byte)(Value.Count >> 8);
            result[pre.Length + 3] = (byte)(Value.Count);
            for (int i = 0; i < Value.Count; i++)
            {
                BitConverter.GetBytes(Value[i]).Reverse().ToArray().CopyTo(result, pre.Length + 4 + i * 8);
            }
            return result;
        }
    }

    /// <summary>
    /// 枚举：NBT节点的所有类型
    /// An enum which represents the type of node.
    /// </summary>
    public enum NBTNodeType
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

    /// <summary>
    /// 所提供的NBTNode类型错误时引发此异常
    /// This exception will be thrown when the type of the NBTNode you provide is incorrect.
    /// </summary>
    public class WrongDataTypeException : Exception { }

}
