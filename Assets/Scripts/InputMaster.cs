//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/PlayerInput.inputactions
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

public partial class @PlayerInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Character"",
            ""id"": ""3b6287cb-1dc8-4e85-871b-6053f5499442"",
            ""actions"": [
                {
                    ""name"": ""move"",
                    ""type"": ""Value"",
                    ""id"": ""0384b021-5eca-4f9a-844d-9cae45bb6764"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""weak"",
                    ""type"": ""Button"",
                    ""id"": ""8d275b5c-f2c0-4b48-b39b-9f3b1d481488"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""strong"",
                    ""type"": ""Button"",
                    ""id"": ""95100852-8086-4f60-a5e5-21b382a20513"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""dodge"",
                    ""type"": ""Button"",
                    ""id"": ""9c5f2f9c-62a2-4fc2-9076-dced25be4ab0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""spellQ"",
                    ""type"": ""Button"",
                    ""id"": ""ca49ac5a-d0bd-47d9-b2cf-c3db01c0fe16"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""spellE"",
                    ""type"": ""Button"",
                    ""id"": ""7ebedafa-9522-4d17-8081-306a0dd35e6c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""spellR"",
                    ""type"": ""Button"",
                    ""id"": ""b9573044-b826-4e17-a01e-bac56330e499"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""f26a247a-904c-469d-81b6-3dffd99fa96d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""06c1bfdb-35d7-48cf-adae-63a1e946d358"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MnK"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d8c682b9-8423-4e8c-8084-f5a2c6eaecb6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MnK"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""82e650cc-c126-4cc0-9c91-5a522615543c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MnK"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""28a6d034-5ce7-466f-94f2-09fc2dfa6a11"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MnK"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3c49dc90-533b-49f4-a544-031c9f5aa71e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""MnK"",
                    ""action"": ""weak"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2cf7c759-11cf-48d7-a5b1-91cabbd1403c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""MnK"",
                    ""action"": ""dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""976635fe-2aa1-4c12-90f7-2d2075e25558"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""MnK"",
                    ""action"": ""strong"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ba42ec8-6834-4fe9-b56b-5ff0ca09977f"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""MnK"",
                    ""action"": ""spellQ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2e88c723-a01f-4d15-ac1c-61167e54c6bf"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""MnK"",
                    ""action"": ""spellE"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""744fa491-6d51-47db-a6f7-2c43677d309c"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""MnK"",
                    ""action"": ""spellR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""MnK"",
            ""bindingGroup"": ""MnK"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Character
        m_Character = asset.FindActionMap("Character", throwIfNotFound: true);
        m_Character_move = m_Character.FindAction("move", throwIfNotFound: true);
        m_Character_weak = m_Character.FindAction("weak", throwIfNotFound: true);
        m_Character_strong = m_Character.FindAction("strong", throwIfNotFound: true);
        m_Character_dodge = m_Character.FindAction("dodge", throwIfNotFound: true);
        m_Character_spellQ = m_Character.FindAction("spellQ", throwIfNotFound: true);
        m_Character_spellE = m_Character.FindAction("spellE", throwIfNotFound: true);
        m_Character_spellR = m_Character.FindAction("spellR", throwIfNotFound: true);
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

    // Character
    private readonly InputActionMap m_Character;
    private List<ICharacterActions> m_CharacterActionsCallbackInterfaces = new List<ICharacterActions>();
    private readonly InputAction m_Character_move;
    private readonly InputAction m_Character_weak;
    private readonly InputAction m_Character_strong;
    private readonly InputAction m_Character_dodge;
    private readonly InputAction m_Character_spellQ;
    private readonly InputAction m_Character_spellE;
    private readonly InputAction m_Character_spellR;
    public struct CharacterActions
    {
        private @PlayerInput m_Wrapper;
        public CharacterActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @move => m_Wrapper.m_Character_move;
        public InputAction @weak => m_Wrapper.m_Character_weak;
        public InputAction @strong => m_Wrapper.m_Character_strong;
        public InputAction @dodge => m_Wrapper.m_Character_dodge;
        public InputAction @spellQ => m_Wrapper.m_Character_spellQ;
        public InputAction @spellE => m_Wrapper.m_Character_spellE;
        public InputAction @spellR => m_Wrapper.m_Character_spellR;
        public InputActionMap Get() { return m_Wrapper.m_Character; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterActions set) { return set.Get(); }
        public void AddCallbacks(ICharacterActions instance)
        {
            if (instance == null || m_Wrapper.m_CharacterActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CharacterActionsCallbackInterfaces.Add(instance);
            @move.started += instance.OnMove;
            @move.performed += instance.OnMove;
            @move.canceled += instance.OnMove;
            @weak.started += instance.OnWeak;
            @weak.performed += instance.OnWeak;
            @weak.canceled += instance.OnWeak;
            @strong.started += instance.OnStrong;
            @strong.performed += instance.OnStrong;
            @strong.canceled += instance.OnStrong;
            @dodge.started += instance.OnDodge;
            @dodge.performed += instance.OnDodge;
            @dodge.canceled += instance.OnDodge;
            @spellQ.started += instance.OnSpellQ;
            @spellQ.performed += instance.OnSpellQ;
            @spellQ.canceled += instance.OnSpellQ;
            @spellE.started += instance.OnSpellE;
            @spellE.performed += instance.OnSpellE;
            @spellE.canceled += instance.OnSpellE;
            @spellR.started += instance.OnSpellR;
            @spellR.performed += instance.OnSpellR;
            @spellR.canceled += instance.OnSpellR;
        }

        private void UnregisterCallbacks(ICharacterActions instance)
        {
            @move.started -= instance.OnMove;
            @move.performed -= instance.OnMove;
            @move.canceled -= instance.OnMove;
            @weak.started -= instance.OnWeak;
            @weak.performed -= instance.OnWeak;
            @weak.canceled -= instance.OnWeak;
            @strong.started -= instance.OnStrong;
            @strong.performed -= instance.OnStrong;
            @strong.canceled -= instance.OnStrong;
            @dodge.started -= instance.OnDodge;
            @dodge.performed -= instance.OnDodge;
            @dodge.canceled -= instance.OnDodge;
            @spellQ.started -= instance.OnSpellQ;
            @spellQ.performed -= instance.OnSpellQ;
            @spellQ.canceled -= instance.OnSpellQ;
            @spellE.started -= instance.OnSpellE;
            @spellE.performed -= instance.OnSpellE;
            @spellE.canceled -= instance.OnSpellE;
            @spellR.started -= instance.OnSpellR;
            @spellR.performed -= instance.OnSpellR;
            @spellR.canceled -= instance.OnSpellR;
        }

        public void RemoveCallbacks(ICharacterActions instance)
        {
            if (m_Wrapper.m_CharacterActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICharacterActions instance)
        {
            foreach (var item in m_Wrapper.m_CharacterActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CharacterActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CharacterActions @Character => new CharacterActions(this);
    private int m_MnKSchemeIndex = -1;
    public InputControlScheme MnKScheme
    {
        get
        {
            if (m_MnKSchemeIndex == -1) m_MnKSchemeIndex = asset.FindControlSchemeIndex("MnK");
            return asset.controlSchemes[m_MnKSchemeIndex];
        }
    }
    public interface ICharacterActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnWeak(InputAction.CallbackContext context);
        void OnStrong(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnSpellQ(InputAction.CallbackContext context);
        void OnSpellE(InputAction.CallbackContext context);
        void OnSpellR(InputAction.CallbackContext context);
    }
}
