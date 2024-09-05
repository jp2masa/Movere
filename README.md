[![Build Status](https://github.com/jp2masa/Movere/actions/workflows/build.yml/badge.svg?branch=master)](https://github.com/jp2masa/Movere/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/Movere.svg)](https://www.nuget.org/packages/Movere/)
[![MyGet](https://img.shields.io/myget/jp2masa/vpre/Movere.svg?label=myget)](https://www.myget.org/feed/jp2masa/package/nuget/Movere)

# Movere

![Movere](Icon.png)

Movere is an implementation of managed dialogs for Avalonia. Currently there are message dialogs, as well as open and save file dialogs, and a print dialog (based on `System.Drawing.Printing`) is WIP.

## Getting Started

### Registering file dialogs with Avalonia

To use Avalonia system dialog APIs, it's possible to simply register Movere dialogs with `AppBuilder`:

1. Import `Movere` namespace:

```cs
using Movere;
```

2. Add `UseMovere` to `AppBuilder` configuration. Example:

```cs
AppBuilder.Configure<App>()
    .UsePlatformDetect()
    .UseMovere();
```

3. Then use Avalonia system dialog APIs. Example:

```cs
var dialog = new OpenFileDialog();
var result = await dialog.ShowAsync(parent);
```

### Using dialog services

To simply use the dialogs (this example is for message dialogs, but it's similar for others):

1. Create a dialog service for `Window` (owner):

```cs
var messageDialogService = new MessageDialogService(owner);
```

2. Pass the service to View Model:

```cs
window.DataContext = new ViewModel(messageDialogService);
```

3. Show dialog from View Model when you need to:

```cs
private Task ShowInfoAsync() =>
    _messageDialogService.ShowMessageDialogAsync(
        new MessageDialogOptions(
            DialogIcon.Info,
            "Message Dialog",
            "Some info",
            DialogResultSet.OK));
```

Available icons are:

- `DialogIcon.None`
- `DialogIcon.Info`
- `DialogIcon.Warning`
- `DialogIcon.Error`

To add your own icon, just create an instance of `DialogIcon` and pass the resource string, e.g `avares://My.App/Resources/Icons/MyIcon.png`.

Dialog results are extensible as well, and support localization.

## Roadmap

- Maybe separate file explorer view into separate project.
- Improve styles for dialogs.
- Add tests.
- Print dialog.
- Eventually move file explorer logic to a separate project and create a file explorer application.

## Credits

### Icons

#### [flaticon.com](https://www.flaticon.com)
- File - [Kiranshastry](https://www.flaticon.com/authors/kiranshastry)
- Folder - [Smashicons](https://www.flaticon.com/authors/smashicons)

### _The project logo is from [linea.io](http://linea.io)._
