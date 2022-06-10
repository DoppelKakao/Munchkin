using System;

[Flags]
public enum GameState
{
	Menu = 1,
	Lobby = 2,
	JoinMenu = 4,
}

[System.Serializable]
public class LocalMenuState : Observed<LocalMenuState>
{
	GameState m_State = GameState.Menu;

	public GameState State
	{
		get => m_State;
		set
		{
			if (m_State != value)
			{
				m_State = value;
				OnChanged(this);
			}
		}
	}

	public override void CopyObserved(LocalMenuState oldObserved)
	{
		if (m_State == oldObserved.State)
		{
			return;
		}
		m_State = oldObserved.State;
		OnChanged(this);
	}
}