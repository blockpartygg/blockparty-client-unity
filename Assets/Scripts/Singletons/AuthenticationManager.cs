using UnityEngine;
using Firebase;
using Firebase.Auth;
using System;

public class AuthenticationManager : Singleton<AuthenticationManager> {
    FirebaseAuth auth;
    public FirebaseUser CurrentUser;

    void Awake() {
        #if UNITY_EDITOR
            auth = FirebaseAuth.GetAuth(FirebaseManager.Instance.App);
            SignIn("test@blockparty.gg", "testingtesting123", () => {}, error => {});
        #else
            auth = FirebaseAuth.DefaultInstance;
        #endif
        
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void OnDestroy() {
        auth.StateChanged -= AuthStateChanged;
    }

    void AuthStateChanged(object sender, EventArgs args) {
        CurrentUser = auth.CurrentUser;
    }

    public void SignIn(string email, string password, Action callback, Action<string> error) {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if(task.IsCanceled) {
                Debug.LogError("Sign in was canceled.");
                return;
            }
            if(task.IsFaulted) {
                foreach(Exception e in (task.Exception as AggregateException).InnerExceptions) {
                    Debug.LogError("Sign in encountered an error: " + (e as FirebaseException).Message);
                    error((e as FirebaseException).Message);
                }
                return;
            }
            FirebaseUser user = task.Result;
            callback();
        });
    }

    public void SignUp(string email, string password, string name, Action callback, Action<string> error) {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if(task.IsCanceled) {
                Debug.LogError("Sign up was canceled.");
                return;
            }
            if(task.IsFaulted) {
                foreach(Exception e in (task.Exception as AggregateException).InnerExceptions) {
                    Debug.LogError("Sign up encountered an error: " + (e as FirebaseException).Message);
                    error((e as FirebaseException).Message);
                }
                return;
            }
            FirebaseUser user = task.Result;
            Player player = new Player(name, 0, 0, false);
            DatabaseManager.Instance.SetPlayerValue(user.UserId, player);
            callback();
        });
    }

    public void SignOut(Action callback) {
        auth.SignOut();
        callback();
    }
}