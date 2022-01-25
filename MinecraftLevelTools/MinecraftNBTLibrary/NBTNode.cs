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

        public NBTNodeType Type { private init; get; }

        protected NBTNode() { Type = (NBTNodeType)GetTypeIndex(); }
    }

    public class NBTNodeEnd : NBTNode
    {
        public override byte GetTypeIndex() => 0;


        public override byte[] ToBytes()
        {
            return new byte[1] { 0 };
        }
    }

    public abstract class NBTNodeData : NBTNode
    {

        public string? Name;
        public abstract object Value
        {
            get;
            set;
        }

        protected byte[] NameBytes()
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
        public override byte GetTypeIndex() => 1;


        internal byte value;

        public NBTNodeByte(byte data) => value = data;

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
            byte[] pre = NameBytes();
            byte[] result = new byte[pre.Length + 1];
            pre.CopyTo(result, 0);
            result[result.Length - 1] = value;
            return result;
        }
    }

    public class NBTNodeShort : NBTNodeData
    {
        public override byte GetTypeIndex() => 2;

        public NBTNodeShort(short data) => value = data;


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
            byte[] pre = NameBytes();
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

        public NBTNodeInt(int data) => value = data;

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

        public override byte GetTypeIndex() => 3;


        public override byte[] ToBytes()
        {
            byte[] pre = NameBytes();
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

        public NBTNodeLong(long data) => value = data;


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

        public override byte GetTypeIndex() => 4;


        public override byte[] ToBytes()
        {
            byte[] pre = NameBytes();
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

        public NBTNodeFloat(float data) => value = data;


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

        public override byte GetTypeIndex() => 5;


        public override byte[] ToBytes()
        {
            byte[] pre = NameBytes();
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

        public override byte GetTypeIndex() => 6;


        public override byte[] ToBytes()
        {
            byte[] pre = NameBytes();
            byte[] data = BitConverter.GetBytes(value);
            byte[] result = new byte[pre.Length + data.Length];
            pre.CopyTo(result, 0);
            data.CopyTo(result, pre.Length);
            return result;
        }
    }

    public class NBTNodeByteArray : NBTNodeData
    {
        internal byte[]? value;

        public NBTNodeByteArray() { }
        public NBTNodeByteArray(byte[] data) => value = data;


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

        public override byte GetTypeIndex() => 7;


        public override byte[] ToBytes()
        {
            byte[] pre = NameBytes();
            byte[] size;
            if (value == null)
            {
                value = Array.Empty<byte>();
            }
            size = new NBTNodeInt(value.Length).ToBytes();
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
                if(value != null)
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

        public override byte GetTypeIndex() => 8;


        public override byte[] ToBytes()
        {
            byte[] pre = NameBytes();
            byte[] length;
            byte[] content;
            if (value == null)
            {
                content=Array.Empty<byte>();
            }
            else
            {
                content=Encoding.UTF8.GetBytes(value);
            }
            length = new NBTNodeByte((byte)content.Length).ToBytes();
            byte[] result = new byte[pre.Length + length.Length + content.Length];
            pre.CopyTo(result, 0);
            length.CopyTo(result, pre.Length);
            content.CopyTo(result, pre.Length + length.Length);
            return result;
        }
    }

    


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

    public class WrongDataTypeException : Exception { }
}
