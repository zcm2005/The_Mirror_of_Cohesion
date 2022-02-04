using MinecraftLevelToolCore;

using MinecraftNBTLibrary;

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

Console.WriteLine("本程序不是 Minecraft 官方产品、不是来自 Minecraft 且未经 Minecraft 认可。");
Console.WriteLine("本程序是与核心库配套发布的命令行工具，主要用于开发和调试，没有为本命令行工具制作说明文档的计划。");
Console.WriteLine("在完成核心库后，可能会制作配套的图形界面工具。");

string path=Console.ReadLine();
var a=File.OpenRead(path);
var b = new byte[a.Length];
a.Read(b);
var c=NBT.ParseFromBytes(b);
var d=File.Create(".\\out.nbt");
var e=c.ToBytes();
d.Write(e, 0, e.Length);
d.Close();
d.Dispose();
Console.WriteLine("完毕");
Console.ReadKey();