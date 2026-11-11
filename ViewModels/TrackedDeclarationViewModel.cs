using CarCustomChecker.Models;

namespace CarCustomChecker.ViewModels;

/// <summary>One tracked declaration row: what the user saved plus the latest result from /vpto.</summary>
public class TrackedDeclarationViewModel : BaseViewModel
{
	private VptoDeclaration? result;
	private bool notFound;

	public TrackedDeclarationViewModel(TrackedDeclaration declaration)
	{
		Declaration = declaration;
	}

	public TrackedDeclaration Declaration { get; }

	public string RegNumber => Declaration.RegNumber;

	public string PlaceName => Declaration.PlaceName;

	public VptoDeclaration? Result
	{
		get => result;
		set
		{
			if (SetProperty(ref result, value))
			{
				OnPropertyChanged(nameof(StatusText));
				OnPropertyChanged(nameof(StatusColor));
				OnPropertyChanged(nameof(HasNote));
				OnPropertyChanged(nameof(Note));
			}
		}
	}

	public bool NotFound
	{
		get => notFound;
		set
		{
			if (SetProperty(ref notFound, value))
			{
				OnPropertyChanged(nameof(StatusText));
			}
		}
	}

	public string StatusText => NotFound
		? Constants.NotFound
		: Result == null ? Constants.NoData : VptoDeclaration.GetStatusText(Result.DeclStatus);

	public Color StatusColor => VptoDeclaration.GetStatusColor(Result?.DeclStatus);

	/// <summary>Why release was refused, or what the customs office requires.</summary>
	public string Note => FirstNonEmpty(Result?.RefusalReason, Result?.RequirementText);

	public bool HasNote => Note.Length > 0;

	private static string FirstNonEmpty(params string?[] values) =>
		values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v))?.Trim() ?? string.Empty;
}
