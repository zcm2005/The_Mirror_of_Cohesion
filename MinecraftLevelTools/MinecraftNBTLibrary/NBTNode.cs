using System;
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

        public abstract byte[] Tobytes();
    }

    public class NBTNodeEnd : NBTNode
    {
        public override byte GetTypeIndex()
        {
            return 0;
        }

        public override byte[] Tobytes()
        {
            return new byte[1] { 0 };
        }
    }

    public abstract class NBTNodeData : NBTNode
    {

        public char[] Name;
        public object Value;

        public override byte[] Tobytes()
        {
            if(Name == null)
            {
                return new byte[2] {0,0};
               
            }
            byte[] bytes = new byte[2 + Name.Length];
            bytes[0] = ((byte)(Name.Length >> 8));
            bytes[1] = ((byte)(Name.Length));
            Name.CopyTo(bytes, 2);
            return bytes;
        }
    }

    public class NBTNodeByte : NBTNodeData
    {
        public override byte GetTypeIndex()
        {
            return 1;
        }

        public override byte[] Tobytes()
        {

        }
    }


    //enum NBTNodeType
    //{
    //    End = 0,
    //    Byte = 1,
    //    Short = 2,
    //    Int = 3,
    //    Long = 4,
    //    Flote = 5,
    //    Double = 6,
    //    Byte_Array = 7,
    //    String = 8,
    //    List = 9,
    //    Compound = 10,
    //    Int_Array = 11,
    //    Long_Array = 12
    //}
}
