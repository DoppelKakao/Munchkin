using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubIdentity : Observed<SubIdentity>
{
	protected Dictionary<string, string> m_contents = new Dictionary<string, string>();

	public string GetContent(string key)
	{
		if (!m_contents.ContainsKey(key))
		{
			m_contents.Add(key, null);
		}
		return m_contents[key];
	}

	public void SetContent(string key, string value)
	{
		if (!m_contents.ContainsKey(key))
		{
			m_contents.Add(key, value);
		}
		else
		{
			m_contents[key] = value;
		}
		OnChanged(this);
	}

	public override void CopyObserved(SubIdentity oldObserved)
	{
		m_contents = oldObserved.m_contents;
	}
}

public enum IIdentityType { Local = 0, Auth}

public interface IIdentity :IProvidable<IIdentity>
{
	SubIdentity GetSubIdentity(IIdentityType identityType);
}

public class IdentityNoop : IIdentity
{
	public SubIdentity GetSubIdentity(IIdentityType identityType) { return null; }
	public void OnReProvided(IIdentity other) { }
}

public class Identity : IIdentity, IDisposable
{
	private Dictionary<IIdentityType, SubIdentity> m_subIdentities = new Dictionary<IIdentityType, SubIdentity>();

	public Identity(Action callbackOnAuthLogin)
	{
		m_subIdentities.Add(IIdentityType.Local, new SubIdentity());
		m_subIdentities.Add(IIdentityType.Auth, new SubIdentity_Authentication(callbackOnAuthLogin));
	}

	public SubIdentity GetSubIdentity(IIdentityType identityType)
	{
		return m_subIdentities[identityType];
	}

	public void OnReProvided(IIdentity prev)
	{
		if (prev is Identity)
		{
			Identity prevIdentity = prev as Identity;
			foreach (var entry in prevIdentity.m_subIdentities)
				m_subIdentities.Add(entry.Key, entry.Value);
		}
	}

	public void Dispose()
	{
		foreach (var sub in m_subIdentities)
			if (sub.Value is IDisposable)
				(sub.Value as IDisposable).Dispose();
	}
}

