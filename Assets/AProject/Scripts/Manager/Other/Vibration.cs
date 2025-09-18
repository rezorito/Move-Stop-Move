using UnityEngine;

public static class Vibration {

#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaObject vibrator = null;
    private static AndroidJavaObject Vibrator
    {
        get
        {
            if (vibrator == null)
            {
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
            }
            return vibrator;
        }
    }
#endif

    /// <summary>
    /// Rung mặc định (giống Handheld.Vibrate)
    /// </summary>
    public static void VibrateButton() {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Vibrator != null) Vibrator.Call("vibrate", 50L); // 50ms
#elif UNITY_IOS && !UNITY_EDITOR
        Handheld.Vibrate(); // iOS chỉ hỗ trợ default
#else
        Debug.Log("VibrateButton() called - Editor không rung được");
#endif
    }

    /// <summary>
    /// Rung tùy chỉnh thời gian (ms)
    /// </summary>
    public static void Vibrate(long milliseconds) {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Vibrator != null) Vibrator.Call("vibrate", milliseconds);
#elif UNITY_IOS && !UNITY_EDITOR
        Handheld.Vibrate();
#else
        Debug.Log("Vibrate(" + milliseconds + ") called - Editor không rung được");
#endif
    }

    /// <summary>
    /// Rung kiểu Pop (ngắn, nhẹ)
    /// </summary>
    public static void VibratePop() {
        Vibrate(50);
    }

    /// <summary>
    /// Rung kiểu Peek (trung bình)
    /// </summary>
    public static void VibratePeek() {
        Vibrate(100);
    }

    /// <summary>
    /// Rung kiểu Nope (dài hơn)
    /// </summary>
    public static void VibrateNope() {
        Vibrate(200);
    }
}
