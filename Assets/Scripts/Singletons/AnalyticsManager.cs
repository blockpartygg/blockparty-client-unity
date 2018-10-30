using UnityEngine;
using Firebase;
using Firebase.Analytics;
using UnityEngine.Analytics;

public class AnalyticsManager : Singleton<AnalyticsManager> {
    public void LogAppOpenEvent() {
        #if !UNITY_EDITOR
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen);
        #endif
    }

    public void LogScreenVisited(string name) {
        AnalyticsEvent.ScreenVisit(name);
    }

    public void LogGameStarted() {
        AnalyticsEvent.GameStart();
    }

    public void LogGameEnded() {
        AnalyticsEvent.GameOver();
    }

    public void LogRoundStarted(string name) {
        AnalyticsEvent.LevelStart(name);
    }

    public void LogRoundEnded(string name) {
        AnalyticsEvent.LevelComplete(name);
    }

    public void LogChatMessageSent() {
        AnalyticsEvent.ChatMessageSent();
    }

    public void LogAdOffered() {
        AnalyticsEvent.AdOffer(true, AdvertisingNetwork.UnityAds);
    }

    public void LogAdStarted() {
        AnalyticsEvent.AdStart(true, AdvertisingNetwork.UnityAds);
    }

    public void LogAdCompleted() {
        AnalyticsEvent.AdComplete(true, AdvertisingNetwork.UnityAds);
    }
}