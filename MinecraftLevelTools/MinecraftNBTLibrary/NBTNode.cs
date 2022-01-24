using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 * Copyright (c) 2022 ZCM
 * MinecraftLevelTools is licensed under Mulan PubL v2.
 * You can use this software according to the terms and conditions of the Mulan PubL v2.
 * You may obtain a copy of Mulan PubL v2 at:
 *          http://license.coscl.org.cn/MulanPubL-2.0
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND,
 * EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT,
 * MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.
 * See the Mulan PubL v2 for more details.
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
