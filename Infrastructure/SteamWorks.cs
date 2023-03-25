using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class SteamWorks
    {
        private const int L4D2AppId = 550;
        private static bool initialized = false;
        private static bool initSuccess = false;

        /// <summary>
        /// SteamClient初始化是否成功
        /// </summary>
        public static bool IsInitSuccess => initSuccess;

        /// <summary>
        /// 初始化SteamClient
        /// </summary>
        public static void Initialize()
        {
            TryInitializeSteamClient();
        }

        /// <summary>
        /// 释放SteamClient
        /// </summary>
        public static void Release()
        {
            if(initSuccess)
                SteamClient.Shutdown();
            initSuccess = false;
            initialized = false;
        }

        private static void TryInitializeSteamClient()
        {
            if (initialized)
                throw new InvalidOperationException("SteamClient已经初始化完毕，不能重复初始化");
            initialized = true;
            try
            {
                SteamClient.Init(L4D2AppId);
                initSuccess = true;
            }
            catch
            {
                initSuccess = false;
            }
        }
    }
}
