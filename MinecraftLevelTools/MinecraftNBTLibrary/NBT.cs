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
    /// NBT操作的静态工具类
    /// </summary>
    public static class NBT
    {
        public static bool TypeIsRight(byte type,NBTNode node)=>TypeIsRight((NBTNodeType)type,node);
        
        internal static bool TypeIsRight(NBTNodeType type,NBTNode node)
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
                    Type t = node.GetType();
                    while (t != typeof(object))
                    {
                        if (t.GetType().GetGenericTypeDefinition() == typeof(NBTNodeList<>))
                        {
                            flag = true;
                            break;
                        }
                        t = t.BaseType;
                    }
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


    }

}
