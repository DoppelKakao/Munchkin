using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInGameInputHandler : IProvidable<IInGameInputHandler>
{
    void OnPlayerInput(ulong playerId, SymbolObject selectedSymbol);
}

public class InGameInputHandlerNoop : IInGameInputHandler
{
    public void OnPlayerInput(ulong playerId, SymbolObject selectedSymbol) { }
    public void OnReProvided(IInGameInputHandler previousProvider) { }
}
