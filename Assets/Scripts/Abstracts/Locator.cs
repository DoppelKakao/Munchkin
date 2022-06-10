using System;
using System.Collections.Generic;

public class Locator : LocatorBase
{
	private static Locator s_isntance;

	public static Locator Get
	{
		get
		{
			if (s_isntance == null)
			{
				s_isntance = new Locator();
			}
			return s_isntance;
		}
	}

	protected override void FinishConstruction()
	{
		s_isntance = this;
	}
}

public interface IProvidable<T>
{
	void OnReProvided(T previousProvider);
}

public class LocatorBase
{
	private Dictionary<Type, object> m_provided = new Dictionary<Type, object>();

	public LocatorBase()
	{
		Provide(new Messenger());
		Provide(new UpdateSlowNoop());
		Provide(new IdentityNoop());
		Provide(new InGameInputHandlerNoop());
	}

	protected virtual void FinishConstruction() { }

	private void ProvideAny<T>(T instance) where T : IProvidable<T>
	{
		Type type = typeof(T);
		if (m_provided.ContainsKey(type))
		{
			var previousProvision = (T)m_provided[type];
			instance.OnReProvided(previousProvision);
			m_provided.Remove(type);
		}

		m_provided.Add(type, instance);
	}

	private T Locate<T>() where T : class
	{
		Type type = typeof(T);
		if (!m_provided.ContainsKey(type))
			return null;
		return m_provided[type] as T;
	}

	public IMessenger Messenger => Locate<IMessenger>();
	public void Provide(IMessenger messenger) { ProvideAny(messenger); }

	public IUpdateSlow UpdateSlow => Locate<IUpdateSlow>();
	public void Provide(IUpdateSlow updateSlow) { ProvideAny(updateSlow); }

	public IIdentity Identity => Locate<IIdentity>();
	public void Provide(IIdentity identity) { ProvideAny(identity); }

	public IInGameInputHandler InGameInputHandler => Locate<IInGameInputHandler>();
	public void Provide(IInGameInputHandler inputHandler) { ProvideAny(inputHandler); }
}