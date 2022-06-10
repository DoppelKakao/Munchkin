﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch; ;

public class Messenger : IMessenger
{
    private List<IReceiveMessages> m_receivers = new List<IReceiveMessages>();
    private const float k_durationToleranceMs = 10;

    private Queue<Action> m_pendingReceivers = new Queue<Action>();
    private int m_recurseCount = 0;

    public virtual void Subscribe(IReceiveMessages receiver)
	{
        m_pendingReceivers.Enqueue(() => { DoSubscribe(receiver); });

        void DoSubscribe(IReceiveMessages receiver)
		{
            if (receiver != null && !m_receivers.Contains(receiver))
                m_receivers.Add(receiver); ;
		}
	}

    public virtual void Unsubscribe(IReceiveMessages receiver)
	{
        m_pendingReceivers.Enqueue(() => { DoUnsubscribe(receiver); });

        void DoUnsubscribe(IReceiveMessages receive)
		{
            m_receivers.Remove(receive);
		}
	}

    public virtual void OnReceiveMessage(MessageType type, object msg)
	{
		if (m_recurseCount > 5)
		{
            Debug.LogError("OnReceiveMessage recursion detected! Is something calling OnReceiveMessage when it receives a message?");
            return;
		}

		if (m_recurseCount == 0)
		{
			while (m_pendingReceivers.Count > 0)
			{
                m_pendingReceivers.Dequeue()?.Invoke();
			}
		}

        m_recurseCount++;
        Stopwatch stopwatch = new Stopwatch();
		foreach (IReceiveMessages receiver in m_receivers)
		{
            stopwatch.Restart();
            receiver.OnReceiveMessage(type, msg);
            stopwatch.Stop();
			if (stopwatch.ElapsedMilliseconds > k_durationToleranceMs)
			{
                Debug.LogWarning($"Message recipient \"{receiver}\" took too long to process message \"{msg}\" of type {type}");
			}
		}
        m_recurseCount--;
	}

    public void OnReProvided(IMessenger previousProvider)
	{
		if (previousProvider is Messenger)
		{
            m_receivers.AddRange((previousProvider as Messenger).m_receivers);
		}
	}
}

public enum MessageType
{
    None = 0,
    RenameRequest = 1,
    JoinLobbyRequest = 2,
    CreateLobbyRequest = 3,
    QueryLobbies = 4,
    QuickJoin = 5,

    ChangeMenuState = 100,
    ConfirmInGameState = 101,
    LobbyUserStatus = 102,
    UserSetEmote = 103,
    ClientUserApproved = 104,
    ClientUserSeekingDisapproval = 105,
    EndGame = 106,

    StartCountdown = 200,
    CancelCountdown = 201,
    CompleteCountdown = 202,
    MinigameBeginning = 203,
    InstructionsShown = 204,
    MinigameEnding = 205,

    DisplayErrorPopup = 300,
}

public interface IReceiveMessages
{
	void OnReceiveMessage(MessageType type, object msg);
}
public interface IMessenger : IReceiveMessages, IProvidable<IMessenger>
{
	void Subscribe(IReceiveMessages receiver);
	void Unsubscribe(IReceiveMessages receiver);
}