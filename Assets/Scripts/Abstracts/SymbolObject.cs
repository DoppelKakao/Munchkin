using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SymbolObject : NetworkBehaviour
{
    [SerializeField] private SymbolData m_symbolData;
    [SerializeField] private SpriteRenderer m_renderer;
    [SerializeField] private Animator m_animator;

    public bool Clicked { get; private set; }
    [HideInInspector] public NetworkVariable<int> symbolIndex; // The index into SymbolData, not the index of this object.

    public override void OnNetworkSpawn()
    {
        symbolIndex.OnValueChanged += OnSymbolIndexSet;
    }

    private void OnSymbolIndexSet(int prevValue, int newValue)
    {
        m_renderer.sprite = m_symbolData.GetSymbolForIndex(symbolIndex.Value);
        symbolIndex.OnValueChanged -= OnSymbolIndexSet;
    }

    public void SetPosition_Server(Vector3 newPosition)
    {
        SetPosition_ClientRpc(newPosition);
    }

    [ClientRpc]
    void SetPosition_ClientRpc(Vector3 newPosition)
    {
        transform.localPosition = newPosition;
    }


    [ServerRpc]
    public void ClickedSequence_ServerRpc(ulong clickerPlayerId)
    {
        Clicked = true;
        Clicked_ClientRpc(clickerPlayerId);
        StartCoroutine(HideSymbolAnimDelay());
    }

    [ClientRpc]
    public void Clicked_ClientRpc(ulong clickerPlayerId)
    {
        if (this.NetworkManager.LocalClientId == clickerPlayerId)
            m_animator.SetTrigger("iClicked");
        else
        {
            m_animator.SetTrigger("theyClicked");
        }
    }

    [ServerRpc]
    public void HideSymbol_ServerRpc()
    {
        this.transform.localPosition += Vector3.forward * 500;
    }

    IEnumerator HideSymbolAnimDelay()
    {
        yield return new WaitForSeconds(0.3f);
        HideSymbol_ServerRpc();
    }
}
