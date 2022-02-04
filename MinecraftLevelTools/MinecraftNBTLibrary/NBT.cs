using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// NBT操作的静态工具类
    /// </summary>
    public static class NBT
    {
        public static bool TypeIsRight(byte type, NBTNode node) => TypeIsRight((NBTNodeType)type, node);

        public static bool TypeIsRight(NBTNodeType type, NBTNode node)
        {
            bool flag = false;
            switch (type)
            {
                case NBTNodeType.End:
                    flag = node is NBTNodeEnd;
                    break;
                case NBTNodeType.Byte:
                    flag = node is NBTNodeByte;
                    break;
                case NBTNodeType.Short:
                    flag = node is NBTNodeShort;
                    break;
                case NBTNodeType.Int:
                    flag = node is NBTNodeInt;
                    break;
                case NBTNodeType.Long:
                    flag = node is NBTNodeLong;
                    break;
                case NBTNodeType.Flote:
                    flag = node is NBTNodeFloat;
                    break;
                case NBTNodeType.Double:
                    flag = node is NBTNodeDouble;
                    break;
                case NBTNodeType.Byte_Array:
                    flag = node is NBTNodeByteArray;
                    break;
                case NBTNodeType.String:
                    flag = node is NBTNodeString;
                    break;
                case NBTNodeType.List:
                    flag = node is NBTNodeList;
                    break;
                case NBTNodeType.Compound:
                    flag = node is NBTNodeCompound;
                    break;
                case NBTNodeType.Int_Array:
                    flag = node is NBTNodeIntArray;
                    break;
                case NBTNodeType.Long_Array:
                    flag = node is NBTNodeLongArray;
                    break;
            }
            return flag;
        }

        public static NBTNode ParseFromBytes(byte[] origin) => ParseFromBytes(origin, 0);

        public static NBTNode ParseFromBytes(byte[] origin, int pos) => ParseFromBytes(origin, pos, null, out int len);

        public static NBTNode ParseFromBytes(byte[] origin, int startindex, NBTNode? parent, out int length)
        {
            NBTNode result;
            try
            {
                switch (origin[startindex])
                {
                    case 0:
                        result = new NBTNodeEnd();
                        length = 1;
                        break;

                    case 1:
                        {
                            string name;
                            name = GetNameFromPreBytes(origin, startindex, out int len);
                            result = new NBTNodeByte(name, origin[startindex + len]);
                            length = len + 1;
                            break;
                        }
                    case 2:
                        {
                            string name;
                            name = GetNameFromPreBytes(origin, startindex, out int len);
                            result = new NBTNodeShort(name, (short)((origin[startindex + len] << 8) | origin[startindex + len + 1]));
                            length = len + 2;
                            break;
                        }
                    case 3:
                        {
                            string name;
                            name = GetNameFromPreBytes(origin, startindex, out int prelen);
                            result = new NBTNodeInt(name, (int)
                                (
                                (origin[startindex + prelen] << 24) |
                                (origin[startindex + prelen + 1] << 16) |
                                (origin[startindex + prelen + 2] << 8) |
                                (origin[startindex + prelen + 3])
                                ));
                            length = prelen + 4;
                            break;
                        }
                    case 4:
                        {
                            string name;
                            name = GetNameFromPreBytes(origin, startindex, out int prelen);
                            result = new NBTNodeLong(name,
                                ((long)origin[startindex + prelen] << 56) |
                                ((long)origin[startindex + prelen + 1] << 48) |
                                ((long)origin[startindex + prelen + 2] << 40) |
                                ((long)origin[startindex + prelen + 3] << 32) |
                                ((long)origin[startindex + prelen + 4] << 24) |
                                ((long)origin[startindex + prelen + 5] << 16) |
                                ((long)origin[startindex + prelen + 6] << 8) |
                                ((long)origin[startindex + prelen + 7])
                                );
                            length = prelen + 8;
                            break;
                        }
                    case 5:
                        {
                            string name;
                            name = GetNameFromPreBytes(origin, startindex, out int prelen);
                            byte[] reverse = new byte[4];
                            for (int i = 0; i < 4; i++)
                            {
                                reverse[i] = origin[startindex + prelen + 3 - i];
                            }
                            result = new NBTNodeFloat(name, BitConverter.ToSingle(reverse));
                            length = prelen + 4;
                            break;
                        }
                    case 6:
                        {
                            string name;
                            name = GetNameFromPreBytes(origin, startindex, out int prelen);
                            byte[] reverse = new byte[8];
                            for (int i = 0; i < 8; i++)
                            {
                                reverse[i] = origin[startindex + prelen + 7 - i];
                            }
                            result = new NBTNodeDouble(name, BitConverter.ToDouble(reverse));
                            length = prelen + 8;
                            break;
                        }
                    case 7:
                        {
                            string name;
                            int size;
                            name = GetNameFromPreBytes(origin, startindex, out int prelen);
                            size = (origin[startindex + prelen] << 24) | (origin[startindex + prelen + 1] << 16) | (origin[startindex + prelen + 2] << 8) | (origin[startindex + prelen + 3]);
                            byte[] load = new byte[size];
                            for (int i = 0; i < size; i++)
                            {
                                load[i] = origin[startindex + prelen + 4 + i];
                            }
                            result = new NBTNodeByteArray(name, new List<byte>(load));
                            length = prelen + 4 + size;
                            break;
                        }
                    case 8:
                        {
                            string name, load;
                            name = GetNameFromPreBytes(origin, startindex, out int prelen);
                            ushort len;
                            len = (ushort)(origin[startindex + prelen] << 8 | origin[startindex + prelen + 1]);
                            load = Encoding.UTF8.GetString(origin, startindex + prelen + 2, len);
                            result = new NBTNodeString(name, load);
                            length = prelen + 2 + len;
                            break;
                        }
                    case 9:
                        {
                            string name;
                            name = GetNameFromPreBytes(origin, startindex, out int prelen);
                            byte tagid = origin[startindex + prelen];
                            int size = (origin[startindex + prelen + 1] << 24) |
                                (origin[startindex + prelen + 2] << 16) |
                                (origin[startindex + prelen + 3] << 8) |
                                (origin[startindex + prelen + 4]);
                            int v = startindex + prelen + 1 + 4;
                            var t = new NBTNodeList(name);
                            int pos = v;
                            for (int i = 0; i < size; i++)
                            {
                                byte[] temp = new byte[3 + origin.Length - pos];
                                temp[0] = tagid; temp[1] = 0; temp[2] = 0;
                                for (i = pos; i < origin.Length; i++)
                                {
                                    temp[3 + i - pos] = origin[i];
                                }
                                t.Add(ParseFromBytes(temp, 0, null, out int a));
                                pos += a - 3;
                            }
                            length = pos - startindex;
                            result = t;
                            break;
                        }
                    case 10:
                        {
                            string name;
                            name = GetNameFromPreBytes(origin, startindex, out int prelen);
                            int v = startindex + prelen;
                            var t = new NBTNodeCompound(name);
                            int pos = v;
                            while (origin[pos] != 0)
                            {
                                t.Add(ParseFromBytes(origin, pos, null, out int a));
                                pos += a;
                            }
                            length = pos - startindex;
                            result = t;
                            break;
                        }
                    case 11:
                        {
                            string name;
                            int size;
                            name = GetNameFromPreBytes(origin, startindex, out int prelen);
                            size = (origin[startindex + prelen] << 24) | (origin[startindex + prelen + 1] << 16) | (origin[startindex + prelen + 2] << 8) | (origin[startindex + prelen + 3]);
                            int[] load = new int[size];
                            int v = startindex + prelen + 4;
                            for (int i = 0; i < size; i++)
                            {
                                load[i] = (origin[v + i * 4] << 24) | (origin[v + i * 4 + 1] << 16) | (origin[v + i * 4 + 2] << 8) | (origin[v + i * 4 + 3]);
                            }
                            result = new NBTNodeIntArray(name, new List<int>(load));
                            length = prelen + 4 + size;
                            break;
                        }
                    case 12:
                        {
                            string name;
                            name = GetNameFromPreBytes(origin, startindex, out int prelen);
                            int size = (origin[startindex + prelen] << 24) | (origin[startindex + prelen + 1] << 16) | (origin[startindex + prelen + 2] << 8) | (origin[startindex + prelen + 3]);
                            long[] load = new long[size];
                            int v = startindex + prelen + 4;
                            for (int i = 0; i < size; i++)
                            {
                                load[i] = ((long)origin[v + i * 8] << 56) |
                                ((long)origin[v + i * 8 + 1] << 48) |
                                ((long)origin[v + i * 8 + 2] << 40) |
                                ((long)origin[v + i * 8 + 3] << 32) |
                                ((long)origin[v + i * 8 + 4] << 24) |
                                ((long)origin[v + i * 8 + 5] << 16) |
                                ((long)origin[v + i * 8 + 6] << 8) |
                                ((long)origin[v + i * 8 + 7]);
                            }
                            result = new NBTNodeLongArray(name, new List<long>(load));
                            length = prelen + 4 + size;
                            break;
                        }
                    default:
                        throw new WrongSyntaxException();

                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new WrongSyntaxException();
            }
            result.Parent = parent;
            return result;
        }


        private static string GetNameFromPreBytes(byte[] origin, out int length) => GetNameFromPreBytes(origin, 0, out length);

        /// <summary>
        /// 从byte数组的特定位置按前缀读取
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="pos">从0开始，表示某个标签第一个字节(ID)的位置</param>
        /// <param name="length">返回整个前缀的字节数</param>
        /// <returns>名称</returns>
        private static string GetNameFromPreBytes(byte[] origin, int pos, out int length)
        {
            int len;
            len = origin[pos + 1];
            len <<= 8;
            len |= origin[pos + 2];
            length = len + 3;
            return Encoding.UTF8.GetString(origin, pos + 3, len);
        }


    }

    /// <summary>
    /// 无法正确解析给定的数组时，抛出此异常
    /// </summary>
    public class WrongSyntaxException : Exception { }

}
