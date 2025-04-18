// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		OpenHarmonyImpl.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/10/25 16:10:SS
// *******************************************

#if UNITY_OPENHARMONY

namespace Bridge.Common
{
    using UnityEngine;

    public class OpenHarmonyImpl : IBridge
    {
        private const string ManagerClassName = "CommonApi";
        private static OpenHarmonyJSObject api;
        private OpenHarmonyJSCallback messageCallback;

        private BridgeCallback initCallback;
        private BridgeCallback vibratorCallback;
        private BridgeCallback getCountryInfoCallback;

        void IBridge.Init(IBridgeListener listener)
        {
            initCallback = new BridgeCallback(listener);
            var harmonyBridgeClass = new OpenHarmonyJSClass(ManagerClassName);
            api = harmonyBridgeClass.CallStatic<OpenHarmonyJSObject>("getInstance");
            api.Call("init", new OpenHarmonyJSCallback(initCallback.Callback));
        }

        void IBridge.Vibrator(VibratorEffectType effectType, IBridgeListener listener)
        {
            vibratorCallback = new BridgeCallback(listener);
            api.Call("vibrator", new OpenHarmonyJSCallback(vibratorCallback.Callback));
        }

        void IBridge.GetCountryInfo(IBridgeListener listener)
        {
            getCountryInfoCallback = new BridgeCallback(listener);
            api.Call("getCountryInfo", new OpenHarmonyJSCallback(getCountryInfoCallback.Callback));
        }
    }
}

#endif
