//using UnityEngine;
//using Firebase.Analytics;

//public class FireBaseAnalyticsLogBroadcaster : MonoBehaviour
//{
//    void Awake()
//    {
//        DontDestroyOnLoad(gameObject);
//    }

//    void OnEnable()
//    {
//        Application.logMessageReceived += HandlelogMessageReceived;
//    }

//    void OnDisable()
//    {
//        Application.logMessageReceived -= HandlelogMessageReceived;
//    }

//    void HandlelogMessageReceived(string condition, string stackTrace, LogType type)
//    {
//        FirebaseAnalytics.LogEvent("Firebase", "Log", condition + " : " + stackTrace);
//    }
//}