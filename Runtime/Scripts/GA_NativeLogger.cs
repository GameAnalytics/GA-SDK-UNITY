using System;
using System.Text;
using UnityEngine;
using System.Runtime.InteropServices;
using AOT;

namespace GameAnalyticsSDK
{
    /// <summary>
    /// Delegate matching the native C++ GALogHandler signature.
    /// The message is passed as IntPtr so we can decode it as UTF-8 — the
    /// default string marshalling (LPStr/ANSI) corrupts non-ASCII bytes on
    /// Windows since its ANSI code page is not UTF-8.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GANativeLogCallback(IntPtr message, int messageType);

    /// <summary>
    /// Routes native C++ SDK log messages to Unity's Debug logging system.
    /// Created and registered by GameAnalytics.cs during initialization,
    /// before the native SDK is initialized.
    /// </summary>
    internal static class GA_NativeLogger
    {
        private const string LogPrefix = "[GameAnalytics Native] ";

        // Must be stored as a static field to prevent GC while native code holds the pointer.
        private static GANativeLogCallback _callback;

        [MonoPInvokeCallback(typeof(GANativeLogCallback))]
        private static void OnNativeLog(IntPtr messagePtr, int messageType)
        {
            string formatted = LogPrefix + PtrToStringUTF8(messagePtr);
            var logType = (GANativeLogType)messageType;

            switch (logType)
            {
                case GANativeLogType.Error:
                    Debug.LogError(formatted);
                    break;
                case GANativeLogType.Warning:
                    Debug.LogWarning(formatted);
                    break;
                case GANativeLogType.Info:
                case GANativeLogType.Debug:
                case GANativeLogType.Verbose:
                default:
                    Debug.Log(formatted);
                    break;
            }
        }

        /// <summary>
        /// Returns a GC-safe callback suitable for passing to the native SDK.
        /// </summary>
        public static GANativeLogCallback GetCallback()
        {
            if (_callback == null)
                _callback = OnNativeLog;
            return _callback;
        }

        // Hand-rolled to avoid Marshal.PtrToStringUTF8, which is missing from
        // .NET Framework API compatibility levels in Unity. Native SDK emits UTF-8.
        private static string PtrToStringUTF8(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;

            int length = 0;
            while (Marshal.ReadByte(ptr, length) != 0)
                length++;

            if (length == 0)
                return string.Empty;

            byte[] buffer = new byte[length];
            Marshal.Copy(ptr, buffer, 0, length);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
