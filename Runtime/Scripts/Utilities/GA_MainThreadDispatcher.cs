using System;
using System.Threading;
using UnityEngine;

namespace GameAnalyticsSDK.Utilities
{
    // Marshals work onto Unity's main thread so SDK entry points can be called from any
    // thread. Unity engine APIs are main-thread-only and crash IL2CPP players if called
    // off-thread (e.g. from iOS' AppTrackingTransparency completion handler).
    public static class GA_MainThreadDispatcher
    {
        private static SynchronizationContext _mainThreadContext;
        private static int _mainThreadId = -1;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Capture()
        {
            _mainThreadContext = SynchronizationContext.Current;
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public static bool IsInitialized
        {
            get { return _mainThreadId != -1; }
        }

        public static bool IsMainThread
        {
            get { return IsInitialized && Thread.CurrentThread.ManagedThreadId == _mainThreadId; }
        }

        // Runs inline if already on the main thread, otherwise posts (non-blocking).
        public static void RunOnMainThread(Action action)
        {
            if (action == null)
            {
                return;
            }

            if (_mainThreadContext == null || IsMainThread)
            {
                action();
            }
            else
            {
                _mainThreadContext.Post(_ => action(), null);
            }
        }
    }
}
