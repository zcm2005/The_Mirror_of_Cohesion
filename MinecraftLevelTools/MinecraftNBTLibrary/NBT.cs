using System.Text;
using System.IO.Compression;

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
        /// <summary>
        /// 检验某NBTNode是否为给定的类型
        /// </summary>
        /// <param name="tagid">以数值形式表示的类型</param>
        /// <param name="node">给定一个NBTNode</param>
        /// <returns></returns>
        [Obsolete("请直接使用Types属性和TypeIndex来判断")]
        public static bool TypeIsRight(byte tagid, NBTNode node) => TypeIsRight((NBTNodeType)tagid, node);

        /// <summary>
        /// 检验某NBTNode是否为给定的类型
        /// </summary>
        /// <param name="type">以枚举形式表示的类型</param>
        /// <param name="node">给定一个NBTNode</param>
        /// <returns></returns>
        [Obsolete("请直接使用Types属性和TypeIndex来判断")]
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

        /// <summary>
        /// 从byte[]形式的NBT中解析出NBTNode
        /// Parse from a byte array which has standard syntax.
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static NBTNode ParseFromBytes(byte[] origin) => ParseFromBytes(origin, 0);

        /// <summary>
        /// 从byte[]形式的NBT中解析出NBTNode
        /// Parse from a byte array which has standard syntax.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="pos">起始的位置</param>
        /// <returns></returns>
        public static NBTNode ParseFromBytes(byte[] origin, int pos) => ParseFromBytes(origin, pos, null, out int len);

        /// <summary>
        /// 从byte[]形式的NBT中解析出NBTNode
        /// Parse from a byte array which has standard syntax.
        /// </summary>
        /// <param name="origin">从此解析</param>
        /// <param name="startindex">起始的位置</param>
        /// <param name="parent">初始的父级节点，可空</param>
        /// <param name="length">返回解析得到的节点的长度</param>
        /// <returns>此为该节点</returns>
        /// <exception cref="WrongSyntaxException">当提供的origin内容格式错误、无法正确解析时，引发此异常</exception>
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
                            var t = new NBTNodeList(name);
                            int pos = startindex + prelen + 1 + 4;
                            for (int i = 0; i < size; i++)
                            {
                                byte[] temp = new byte[3 + origin.Length - pos];
                                temp[0] = tagid; temp[1] = 0; temp[2] = 0;
                                for (int j = pos; j < origin.Length; j++)
                                {
                                    temp[3 + j - pos] = origin[j];
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
                            length = pos - startindex + 1;
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
                            length = prelen + 4 + size * 4;
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
                            length = prelen + 4 + size * 8;
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

        /// <summary>
        /// 从流解析出NBTNode
        /// Parse from stream which has standard syntax.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static NBTNode ParseFromStream(Stream s)
        {
            List<byte> temp = new();
            int t;
            t = s.ReadByte();
            while (t != -1)
            {
                temp.Add((byte)t);
                t = s.ReadByte();
            }
            return ParseFromBytes(temp.ToArray());
        }

        /// <summary>
        /// 从GZip压缩文件解析出NBTNode。一般来说，Minecraft中的.dat文件都是GZip压缩文件。
        /// Parse from a GZip file. The files named *.dat are such file in general.
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static NBTNode ParseFromCompressedNBTFile(string path)
        {
            var t = File.OpenRead(path);
            var g = new GZipStream(t, CompressionMode.Decompress);
            t.Close();
            return ParseFromStream(g);
        }

        /// <summary>
        /// 写为GZip压缩的文件
        /// </summary>
        /// <param name=""></param>
        public static void WriteToCompressedNBTFile(NBTNode t, string path)
        {
            var f = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None);
            byte[] data = t.ToBytes();
            f.Write(data, 0, data.Length);
            f.Close();
        }
    }

    /// <summary>
    /// 无法正确解析给定的数据时，抛出此异常
    /// This exception will be thrown when the data you provide can not be parsed correctly.
    /// </summary>
    public class WrongSyntaxException : Exception { }

}
