using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarCustomChecker.Models;

public class BaseViewModel : INotifyPropertyChanged
{
	bool isBusy = false;
	public bool IsBusy
	{
		get => isBusy;
		set => SetProperty(ref isBusy, value, onChanged: () => OnPropertyChanged(nameof(ShowLoadingIndicator)));
	}

	bool isRefreshing = false;
	/// <summary>
	/// Bind RefreshView.IsRefreshing here, not to IsBusy: setting IsRefreshing to true
	/// runs the RefreshView's Command, so tying it to IsBusy re-triggers a reload on every load.
	/// </summary>
	public bool IsRefreshing
	{
		get => isRefreshing;
		set => SetProperty(ref isRefreshing, value, onChanged: () => OnPropertyChanged(nameof(ShowLoadingIndicator)));
	}

	/// <summary>
	/// True while data is loading, except during pull-to-refresh where the RefreshView shows its own spinner.
	/// </summary>
	public bool ShowLoadingIndicator => IsBusy && !IsRefreshing;

	protected bool SetProperty<T>(ref T backingStore, T value,
		[CallerMemberName] string propertyName = "",
		Action onChanged = null)
	{
		if (EqualityComparer<T>.Default.Equals(backingStore, value))
			return false;

		backingStore = value;
		onChanged?.Invoke();
		OnPropertyChanged(propertyName);
		return true;
	}

	#region INotifyPropertyChanged
	public event PropertyChangedEventHandler PropertyChanged;
	protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		var changed = PropertyChanged;
		if (changed == null)
			return;

		changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	#endregion
}