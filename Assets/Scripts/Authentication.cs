using UnityEngine;
using Firebase;
using Firebase.Auth;
using System;

public class Authentication : Singleton<Authentication> {
    FirebaseAuth auth;
    public FirebaseUser CurrentUser;

    void Awake() {
        #if UNITY_EDITOR
            auth = FirebaseAuth.GetAuth(FirebaseManager.Instance.App);
        #else
            auth = FirebaseAuth.DefaultInstance;
        #endif
        
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
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
            Debug.LogFormat("Sign in was successful: {0}", user.UserId);
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
            Database.Instance.SetPlayerValue(user.UserId, player);
            Debug.LogFormat("Sign up was successful: {0}", user.UserId);
            callback();
        });
    }

    public void SignOut(Action callback) {
        auth.SignOut();
        callback();
    }
}