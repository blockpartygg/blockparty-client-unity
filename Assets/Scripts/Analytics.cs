using UnityEngine;
using Firebase;
using Firebase.Analytics;

public class Analytics : Singleton<Analytics> {
    public void LogAppOpenEvent() {
        #if !UNITY_EDITOR
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen);
        #endif

        // Events to add:
        // FirebaseAnalytics.EventEarnVirtualCurrency
        // FirebaseAnalytics.EventEcommercePurchase
        // FirebaseAnalytics.EventLevelStart
        // FirebaseAnalytics.EventLevelEnd
        // FirebaseAnalytics.EventLogin
        // FirebaseAnalytics.EventSignUp
        // FirebaseAnalytics.EventSpendVirtualCurrency
    }
}