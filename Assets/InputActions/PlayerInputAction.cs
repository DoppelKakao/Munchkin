//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/InputActions/PlayerInputAction.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputAction : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputAction"",
    ""maps"": [
        {
            ""name"": ""PlayerCamera"",
            ""id"": ""c866d3a2-af6a-4780-814b-1a5ec3512fbe"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""312bd7e7-cc70-44fb-a12b-799188c3cd77"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ZoomCamera"",
                    ""type"": ""Value"",
                    ""id"": ""144bf589-321b-4baf-b81b-509f4bdd210d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""a25c6732-3e60-4915-a828-3439b52a0f9a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fcff963c-441d-4c66-96c1-3f0852c7b352"",
                    ""path"": ""<Keyboard>/#(W)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""83deeb6c-e8da-40c7-bd09-1d5b4cdf06ad"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""671d64af-3928-4aae-8e3d-f65bba9765d7"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b55d296d-cc2a-4049-be6d-d8d8974f693e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""dd554956-8423-4835-beab-763808450ee5"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerCamera
        m_PlayerCamera = asset.FindActionMap("PlayerCamera", throwIfNotFound: true);
        m_PlayerCamera_Movement = m_PlayerCamera.FindAction("Movement", throwIfNotFound: true);
        m_PlayerCamera_ZoomCamera = m_PlayerCamera.FindAction("ZoomCamera", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerCamera
    private readonly InputActionMap m_PlayerCamera;
    private IPlayerCameraActions m_PlayerCameraActionsCallbackInterface;
    private readonly InputAction m_PlayerCamera_Movement;
    private readonly InputAction m_PlayerCamera_ZoomCamera;
    public struct PlayerCameraActions
    {
        private @PlayerInputAction m_Wrapper;
        public PlayerCameraActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerCamera_Movement;
        public InputAction @ZoomCamera => m_Wrapper.m_PlayerCamera_ZoomCamera;
        public InputActionMap Get() { return m_Wrapper.m_PlayerCamera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerCameraActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerCameraActions instance)
        {
            if (m_Wrapper.m_PlayerCameraActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnMovement;
                @ZoomCamera.started -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnZoomCamera;
                @ZoomCamera.performed -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnZoomCamera;
                @ZoomCamera.canceled -= m_Wrapper.m_PlayerCameraActionsCallbackInterface.OnZoomCamera;
            }
            m_Wrapper.m_PlayerCameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @ZoomCamera.started += instance.OnZoomCamera;
                @ZoomCamera.performed += instance.OnZoomCamera;
                @ZoomCamera.canceled += instance.OnZoomCamera;
            }
        }
    }
    public PlayerCameraActions @PlayerCamera => new PlayerCameraActions(this);
    public interface IPlayerCameraActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnZoomCamera(InputAction.CallbackContext context);
    }
}