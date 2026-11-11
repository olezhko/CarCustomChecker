namespace CarCustomChecker.Models;

/// <summary>A declaration the user tracks: which ВПТО it was filed at and its registration number.</summary>
public record TrackedDeclaration(int PlaceId, string PlaceName, string RegNumber);
