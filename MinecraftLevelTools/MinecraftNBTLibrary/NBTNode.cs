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

        public abstract byte GetTypeIndex();

        public abstract byte[] ToBytes();

        internal NBTNodeType Type { private init; get; }

        protected NBTNode() { Type = (NBTNodeType)GetTypeIndex(); }

        public NBTNode? Parent;



    }

    public class NBTNodeEnd : NBTNode
    {
        public NBTNodeEnd() : base()
        {
        }

        public override sealed byte GetTypeIndex() => 0;


        public override byte[] ToBytes()
        {
            return new byte[1] { 0 };
        }

        public static bool operator ==(NBTNodeEnd a, NBTNodeEnd b) => true;
        public static bool operator !=(NBTNodeEnd a, NBTNodeEnd b) => false;

    }

    public abstract class NBTNodeData : NBTNode
    {

        public string? Name;

        protected NBTNodeData() : base()
        {
            Name = null;
        }

        protected NBTNodeData(string? name) : base()
        {
            Name = name;
        }

        public abstract object Value
        {
            get;
            set;
        }

        protected byte[] PreBytes()
        {
            if (Name == null)
            {
                return new byte[2] { 0, 0 };

            }
            byte[] bytes = new byte[3 + Name.Length];
            bytes[0] = GetTypeIndex();
            bytes[1] = ((byte)(Name.Length >> 8));
            bytes[2] = ((byte)(Name.Length));
            Encoding.UTF8.GetBytes(Name).CopyTo(bytes, 3);
            return bytes;
        }

    }

    public class NBTNodeByte : NBTNodeData
    {
        public override sealed byte GetTypeIndex() => 1;

        public static bool operator ==(NBTNodeByte a, NBTNodeByte b) => (a.Name == b.Name && a.value == b.value);
        public static bool operator !=(NBTNodeByte a, NBTNodeByte b) => (a.Name != b.Name || a.value != b.value);

        internal byte value;

        public NBTNodeByte(string name, byte data) : base(name) => value = data;

        public override object Value
        {
            get
            {
                return value;
            }
            set
            {
                if (value is byte t)
                {
                    this.value = t;
                }
                else
                {
                    throw new WrongDataTypeException();
                }
            }
        }

        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] result = new byte[pre.Length + 1];
            pre.CopyTo(result, 0);
            result[result.Length - 1] = value;
            return result;
        }
    }

    public class NBTNodeShort : NBTNodeData
    {
        public override sealed byte GetTypeIndex() => 2;

        public NBTNodeShort(string name, short data) : base(name) => value = data;

        public static bool operator ==(NBTNodeShort a, NBTNodeShort b) => (a.Name == b.Name && a.value == b.value);
        public static bool operator !=(NBTNodeShort a, NBTNodeShort b) => (a.Name != b.Name || a.value != b.value);

        internal short value;
        public override object Value
        {
            get
            {
                return value;
            }
            set
            {
                if (value is short t)
                {
                    this.value = t;
                }
                else
                {
                    throw new WrongDataTypeException();
                }
            }
        }

        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] data = BitConverter.GetBytes(value);
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }

    }

    public class NBTNodeInt : NBTNodeData
    {
        internal int value;

        public NBTNodeInt(string name, int data) : base(name) => value = data;

        public static bool operator ==(NBTNodeInt a, NBTNodeInt b) => (a.Name == b.Name && a.value == b.value);
        public static bool operator !=(NBTNodeInt a, NBTNodeInt b) => (a.Name != b.Name || a.value != b.value);

        public override object Value
        {
            get { return value; }
            set
            {
                if (value is int t)
                {
                    this.value = t;
                }
                else
                {
                    throw new WrongDataTypeException();
                }
            }
        }

        public override sealed byte GetTypeIndex() => 3;


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] data = BitConverter.GetBytes(value);
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }
    }

    public class NBTNodeLong : NBTNodeData
    {
        internal long value;

        public NBTNodeLong(string name, long data) : base(name) => value = data;

        public static bool operator ==(NBTNodeLong a, NBTNodeLong b) => (a.Name == b.Name && a.value == b.value);
        public static bool operator !=(NBTNodeLong a, NBTNodeLong b) => (a.Name != b.Name || a.value != b.value);

        public override object Value
        {
            get { return value; }
            set
            {
                if (value is long t)
                {
                    this.value = t;
                }
                else
                {
                    throw new WrongDataTypeException();
                }
            }
        }

        public override sealed byte GetTypeIndex() => 4;


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] data = BitConverter.GetBytes(value);
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }
    }

    public class NBTNodeFloat : NBTNodeData
    {
        internal float value;

        public NBTNodeFloat(string name, float data) : base(name) => value = data;

        public static bool operator ==(NBTNodeFloat a, NBTNodeFloat b) => (a.Name == b.Name && a.value == b.value);
        public static bool operator !=(NBTNodeFloat a, NBTNodeFloat b) => (a.Name != b.Name || a.value != b.value);



        public override object Value
        {
            get { return value; }
            set
            {
                if (value is float t)
                {
                    this.value = t;
                }
                else
                {
                    throw new WrongDataTypeException();
                }
            }
        }

        public override sealed byte GetTypeIndex() => 5;


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] data = BitConverter.GetBytes(value);
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }
    }

    public class NBTNodeDouble : NBTNodeData
    {
        internal double value;

        public NBTNodeDouble(double data) => value = data;

        public static bool operator ==(NBTNodeDouble a, NBTNodeDouble b) => (a.Name == b.Name && a.value == b.value);
        public static bool operator !=(NBTNodeDouble a, NBTNodeDouble b) => (a.Name != b.Name || a.value != b.value);



        public override object Value
        {
            get { return value; }
            set
            {
                if (value is double t)
                {
                    this.value = t;
                }
                else
                {
                    throw new WrongDataTypeException();
                }
            }
        }

        public override sealed byte GetTypeIndex() => 6;


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] data = BitConverter.GetBytes(value);
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }
    }

    public abstract class NBTNodeDataCollection : NBTNodeData, ICollection<NBTNodeData>
    {
        public abstract int Count { get; }
        public abstract bool IsReadOnly { get; }

        public abstract void Add(NBTNodeData item);
        public abstract void Clear();
        public abstract bool Contains(NBTNodeData item);
        public abstract void CopyTo(NBTNodeData[] array, int arrayIndex);
        public abstract IEnumerator<NBTNodeData> GetEnumerator();
        public abstract bool Remove(NBTNodeData item);

        protected NBTNodeDataCollection() : base()
        {
        }

        protected NBTNodeDataCollection(string? name) : base(name)
        {
            
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

    public abstract class NBTNodeDataArray : NBTNodeDataCollection, IList<NBTNodeData>
    {
        public abstract NBTNodeData this[int index] { get; set; }

        public abstract int IndexOf(NBTNodeData item);
        public abstract void Insert(int index, NBTNodeData item);
        public abstract void RemoveAt(int index);

        protected NBTNodeDataArray() : base()
        {
        }

        protected NBTNodeDataArray(string? name) : base(name)
        {

        }
    }

    public class NBTNodeByteArray : NBTNodeDataArray
    {
        internal byte[]? value;

        public NBTNodeByteArray() : base() { }
        public NBTNodeByteArray(string name, byte[] data) : base(name) => value = data;

        public static bool operator ==(NBTNodeByteArray a, NBTNodeByteArray b)
        {

        }
        public static bool operator !=(NBTNodeDouble a, NBTNodeDouble b) => (a.Name != b.Name || a.value != b.value);



        public override object Value
        {
            get
            {
                if (value != null)
                    return value;
                else
                    return Array.Empty<byte>();
            }
            set
            {
                if (value != null)
                {
                    if (value is byte[] t)
                    {
                        this.value = t;
                    }
                    else
                    {
                        throw new WrongDataTypeException();
                    }
                }
                else
                    this.value = null;
            }
        }

        public override int Count => throw new NotImplementedException();

        public override bool IsReadOnly => throw new NotImplementedException();

        public override NBTNodeData this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override sealed byte GetTypeIndex() => 7;


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] size;
            if (value == null)
            {
                value = Array.Empty<byte>();
            }
            size = new NBTNodeInt("size", value.Length).ToBytes();
            byte[] result = new byte[pre.Length + size.Length + value.Length];
            pre.CopyTo(result, 0);
            size.CopyTo(result, pre.Length);
            value.CopyTo(result, pre.Length + size.Length);
            if (value.Length == 0)
            {
                value = null;
            }
            return result;
        }

        public override int IndexOf(NBTNodeData item)
        {
            throw new NotImplementedException();
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

    public class NBTNodeString : NBTNodeData
    {
        internal string? value;

        public NBTNodeString() { }
        public NBTNodeString(string data) => value = data;


        public override object Value
        {
            get
            {
                if (value != null)
                    return value;
                else
                    return string.Empty;
            }
            set
            {
                if (value != null)
                {
                    if (value is string t)
                    {
                        this.value = t;
                    }
                    else
                    {
                        throw new WrongDataTypeException();
                    }
                }
                else
                    this.value = null;
            }
        }

        public override sealed byte GetTypeIndex() => 8;


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] length;
            byte[] content;
            if (value == null)
            {
                content = Array.Empty<byte>();
            }
            else
            {
                content = Encoding.UTF8.GetBytes(value);
            }
            length = new NBTNodeByte("length", (byte)content.Length).ToBytes();
            byte[] result = new byte[pre.Length + length.Length + content.Length];
            pre.CopyTo(result, 0);
            length.CopyTo(result, pre.Length);
            content.CopyTo(result, pre.Length + length.Length);
            return result;
        }
    }

    public class NBTNodeIntArray : NBTNodeDataArray
    {
        internal int[]? value;

        public NBTNodeIntArray() : base() { }
        public NBTNodeIntArray(string name, int[] data) : base(name) => value = data;


        public override object Value
        {
            get
            {
                if (value != null)
                    return value;
                else
                    return Array.Empty<int>();
            }
            set
            {
                if (value != null)
                {
                    if (value is int[] t)
                    {
                        this.value = t;
                    }
                    else
                    {
                        throw new WrongDataTypeException();
                    }
                }
                else
                    this.value = null;
            }
        }

        public override sealed byte GetTypeIndex() => 11;


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] size;
            if (value == null)
            {
                value = Array.Empty<int>();
            }
            size = new NBTNodeInt("size", value.Length).ToBytes();
            byte[] result = new byte[pre.Length + size.Length + value.Length];
            pre.CopyTo(result, 0);
            size.CopyTo(result, pre.Length);
            value.CopyTo(result, pre.Length + size.Length);
            if (value.Length == 0)
            {
                value = null;
            }
            return result;
        }
    }

    public class NBTNodeLongArray : NBTNodeDataArray
    {
        internal long[]? value;

        public NBTNodeLongArray() : base() { }
        public NBTNodeLongArray(string name, long[] data) : base(name) => value = data;


        public override object Value
        {
            get
            {
                if (value != null)
                    return value;
                else
                    return Array.Empty<long>();
            }
            set
            {
                if (value != null)
                {
                    if (value is long[] t)
                    {
                        this.value = t;
                    }
                    else
                    {
                        throw new WrongDataTypeException();
                    }
                }
                else
                    this.value = null;
            }
        }

        public override sealed byte GetTypeIndex() => 12;


        public override byte[] ToBytes()
        {
            byte[] pre = PreBytes();
            byte[] size;
            if (value == null)
            {
                value = Array.Empty<long>();
            }
            size = new NBTNodeInt("size", value.Length).ToBytes();
            byte[] result = new byte[pre.Length + size.Length + value.Length];
            pre.CopyTo(result, 0);
            size.CopyTo(result, pre.Length);
            value.CopyTo(result, pre.Length + size.Length);
            if (value.Length == 0)
            {
                value = null;
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
