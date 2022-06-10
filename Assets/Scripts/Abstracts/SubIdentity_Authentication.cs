using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class SubIdentity_Authentication : SubIdentity, IDisposable
{
    private bool m_hasDisposed = false;

    /// <summary>
    /// This will kick off a login.
    /// </summary>
    public SubIdentity_Authentication(Action onSigninComplete = null)
    {
        DoSignIn(onSigninComplete);
    }
    ~SubIdentity_Authentication()
    {
        Dispose();
    }
    public void Dispose()
    {
        if (!m_hasDisposed)
        {
            AuthenticationService.Instance.SignedIn -= OnSignInChange;
            AuthenticationService.Instance.SignedOut -= OnSignInChange;
            m_hasDisposed = true;
        }
    }

    private async void DoSignIn(Action onSigninComplete)
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += OnSignInChange;
        AuthenticationService.Instance.SignedOut += OnSignInChange;

        try
        {
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync(); // Don't sign out later, since that changes the anonymous token, which would prevent the player from exiting lobbies they're already in.
            onSigninComplete?.Invoke();
        }
        catch
        {
            UnityEngine.Debug.LogError("Failed to login. Did you remember to set your Project ID under Services > General Settings?");
            throw;
        }
    }

    private void OnSignInChange()
    {
        SetContent("id", AuthenticationService.Instance.PlayerId);
    }
}
