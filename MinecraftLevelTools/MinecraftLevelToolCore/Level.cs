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



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLevelToolCore
{
    /// <summary>
    /// 此类抽象一个存档文件，其中可包含多个世界（World），以及其他附属信息
    /// </summary>
    public class Level
    {
        /// <summary>
        /// 存档锁
        /// </summary>
        FileStream? sessionlock;


        /// <summary>
        /// 存档的路径
        /// </summary>
        string? levelpath;
        public string? LevelPath
        {
            get { return levelpath; }
            private set {
                if (sessionlock == null)
                {
                    levelpath = value;
                }
                else
                {
                    Close();
                    levelpath = value;
                }
            }
        }

        public Level()
        {

        }

        /// <summary>
        /// 使用此方式构造，将自动打开存档
        /// </summary>
        /// <param name="path">存档路径</param>
        public Level(string path)
        {
            this.LevelPath = path;
            Open();
        }

        /// <summary>
        /// 手动打开存档
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <exception cref="LevelNotFoundException">在给定的位置找不到存档</exception>
        public void Open()
        {

            try
            {
                if (LevelPath != null)
                    sessionlock = new(Path.Combine(LevelPath, "session.lock"), FileMode.Open, FileAccess.Read, FileShare.None, 1, false);
                else throw new Exception("未提供存档路径");
            }
            catch (FileNotFoundException)
            {
                throw new LevelNotFoundException();
            }
        }

        /// <summary>
        /// 手动关闭存档
        /// </summary>
        public void Close()
        {
            if (sessionlock != null)
            {
                sessionlock.Close();
                sessionlock.Dispose();
                sessionlock = null;
            }

        }

        ~Level()
        {
            Close();
        }

        /// <summary>
        /// 若未打开存档，则返回错误
        /// </summary>
        /// <exception cref="LevelNotOpenException"></exception>
        private void IsOpen()
        {
            if(sessionlock == null)
                throw new LevelNotOpenException();
        }


        public string LevelName
        {
            get;
            set;
        }
    }
}
