using System;

public abstract class Observed<T>
{
	public abstract void CopyObserved(T aldObserved);

	public Action<T> onChanged { get; set; }
	public Action<T> onDestroyed { get; set; }

	protected void OnChanged(T observed)
	{
		onChanged?.Invoke(observed);
	}

	protected void OnDestroyed(T observed)
	{
		onDestroyed?.Invoke(observed);
	}

}