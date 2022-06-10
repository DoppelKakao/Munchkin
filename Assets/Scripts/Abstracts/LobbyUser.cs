using System;

[Flags]
public enum UserStatus
{
    None = 0,
    Connecting = 1, // User has joined a lobby but has not yet connected to Relay.
    Lobby = 2,      // User is in a lobby and connected to Relay.
    Ready = 4,      // User has selected the ready button, to ready for the "game" to start.
    InGame = 8,     // User is part of a "game" that has started.
    Menu = 16       // User is not in a lobby, in one of the main menus.
}

[Serializable]
public class LobbyUser : Observed<LobbyUser>
{
    public LobbyUser(bool isHost = false, string displayName = null, string id = null, UserStatus userStatus = UserStatus.Menu, bool isApproved = false)
    {
        m_data = new UserData(isHost, displayName, id, userStatus, isApproved);
    }

    public struct UserData
	{
        public bool IsHost { get; set; }
        public string DisplayName { get; set; }
        public string ID { get; set; }
        public UserStatus UserStatus { get; set; }
        public bool IsApproved { get; set; }

        public UserData(bool isHost, string displayName, string id, UserStatus userStatus, bool isApproved)
		{
            IsHost = isHost;
            DisplayName = displayName;
            ID = id;
            UserStatus = userStatus;
            IsApproved = isApproved;
        }
	}

    private UserData m_data;

    public void ResetState()
	{
        m_data = new UserData(false, m_data.DisplayName, m_data.ID, UserStatus.Menu, false);
	}

    [Flags]
    public enum UserMembers
	{
        IsHost = 1,
        DisplayName = 2,
        ID = 8,
        UserStatus = 16,
        IsApproved = 32
    }

    private UserMembers m_lastChanged;
    public UserMembers LastChanged => m_lastChanged;

    public bool IsHost
	{
        get { return m_data.IsHost; }
        set
		{
			if (m_data.IsHost != value)
			{
                m_data.IsHost = value;
                m_lastChanged = UserMembers.IsHost;
                OnChanged(this);
				if (value)
				{
                    IsApproved = true;
				}
			}
		}
	}

    public string DisplayName
    {
        get => m_data.DisplayName;
        set
        {
            if (m_data.DisplayName != value)
            {
                m_data.DisplayName = value;
                m_lastChanged = UserMembers.DisplayName;
                OnChanged(this);
            }
        }
    }

    public string ID
    {
        get => m_data.ID;
        set
        {
            if (m_data.ID != value)
            {
                m_data.ID = value;
                m_lastChanged = UserMembers.ID;
                OnChanged(this);
            }
        }
    }

    UserStatus m_userStatus = UserStatus.Menu;
    public UserStatus UserStatus
    {
        get => m_userStatus;
        set
        {
            if (m_userStatus != value)
            {
                m_userStatus = value;
                m_lastChanged = UserMembers.UserStatus;
                OnChanged(this);
            }
        }
    }

    public bool IsApproved
	{
        get => m_data.IsApproved;
        set
		{
			if (!m_data.IsApproved && value)
			{
                m_data.IsApproved = value;
                m_lastChanged = UserMembers.IsApproved;
                OnChanged(this);
                Locator.Get.Messenger.OnReceiveMessage(MessageType.ClientUserApproved, null);
			}
		}
	}

    public override void CopyObserved(LobbyUser observed)
    {
        UserData data = observed.m_data;
        int lastChanged = // Set flags just for the members that will be changed.
            (m_data.IsHost == data.IsHost ? 0 : (int)UserMembers.IsHost) |
            (m_data.DisplayName == data.DisplayName ? 0 : (int)UserMembers.DisplayName) |
            (m_data.ID == data.ID ? 0 : (int)UserMembers.ID) |
            (m_data.UserStatus == data.UserStatus ? 0 : (int)UserMembers.UserStatus);

        if (lastChanged == 0) // Ensure something actually changed.
            return;

        m_data = data;
        m_lastChanged = (UserMembers)lastChanged;

        OnChanged(this);
    }
}