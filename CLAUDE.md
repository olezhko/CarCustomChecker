# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Unofficial .NET MAUI mobile app (Android + iOS) for checking vehicles at Belarusian customs terminals (БЕЛТАМОЖСЕРВИС). UI strings are in Russian. Single-project MAUI app targeting `net10.0-android` and `net10.0-ios`; there are no tests and no lint configuration.

## Commands

```bash
dotnet build -f net10.0-android                  # build Android
dotnet build -f net10.0-android -t:Run           # build + deploy to connected device/emulator
dotnet publish -f net10.0-android -c Release     # Android release package
dotnet build -f net10.0-ios                      # iOS (requires a paired Mac / Xcode)
```

The iOS Release config signs with the `CarVimChecker_ios_store` provisioning profile; Debug uses `CarVimChecker_dev`.

## Versioning

`ApplicationVersion` / `ApplicationDisplayVersion` are duplicated in `CarCustomChecker.csproj` — in the main `PropertyGroup` **and** in each per-configuration/per-platform `PropertyGroup` (Debug/Release × android/ios). When bumping the version, update every occurrence.

## Architecture

- **Navigation**: `AppShell.xaml` defines a `TabBar` with three tabs, each a page with its own view model:
  - `MainPage` / `MainPageViewModel` — paged vehicle search by VIN and customs point (ПТО); infinite scroll via `LoadNewItemsCommand`, reload triggered from code-behind on appear, picker change, or search text ≥ 4 chars.
  - `Views/VimsControl` / `VimsControlViewModel` — user's tracked VIN list, persisted as a comma-joined string in `Preferences` via `SettingsService`. Each row is a `CarItemViewModel` built from `FieldValueViewModel<T>` wrappers.
  - `Views/ElectronicQueue` / `ElectronicQueueViewModel` — border checkpoint electronic queue data.
- **MVVM without a framework**: view models derive from the hand-rolled `Models/BaseViewModel` (`SetProperty`, `IsBusy`), and are instantiated directly in XAML via `<ContentPage.BindingContext>` — not via constructor injection.
- **Data access**: `Services/DataService` implements `Models/IDataService` and is registered with the legacy `DependencyService.Register<DataService>()` in `App.xaml.cs`; view models resolve it with `DependencyService.Get<IDataService>()` (not `MauiProgram` DI). It uses RestSharp + Newtonsoft.Json against two external APIs:
  - `https://api.mon.declarant.by/api/vehicle` (POST, paged vehicle search)
  - `https://belarusborder.by/info/checkpoint?token=...` (GET, electronic queue)
  
  Service methods swallow exceptions and return `null` on failure.
- **Ads**: Yandex Mobile Ads via the `LATec.MAUI.Yandex.Ads` NuGet (`.UseYandexAds()` in `MauiProgram`). Each page has an `AdBanner` whose `AdUnitId` is chosen per platform in code-behind from `Models/Constants.cs` (separate Android/iOS unit IDs per page). Customs point codes (ПТО) also live in `Constants`.
