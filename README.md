# DulcisX

<a href="https://www.nuget.org/packages/DulcisX"><img alt="Nuget" src="https://img.shields.io/nuget/v/DulcisX"></a> <a href="https://www.nuget.org/packages/DulcisX"><img alt="Nuget" src="https://img.shields.io/nuget/dt/DulcisX"></a> <a href="https://github.com/TwentyFourMinutes/DulcisX/issues"><img alt="GitHub issues" src="https://img.shields.io/github/issues-raw/TwentyFourMinutes/DulcisX"></a> <a href="https://github.com/TwentyFourMinutes/DulcisX/blob/master/LICENSE"><img alt="GitHub" src="https://img.shields.io/github/license/TwentyFourMinutes/DulcisX"></a> <a href="https://discordapp.com/invite/EYKxkce"><img alt="Discord" src="https://discordapp.com/api/guilds/275377268728135680/widget.png"></a>

DulcisX translates to _Sweet Extension_ and this is the goal for this Project, making the experience of developing Visual Studio Extensions sweat. It allows you to get your extension done faster and doesn't bother you with ugly or missing API's.

## About

This package aims to wrap around the current Visual Studio SDK. In order to fully achieve this goal, your help is really apricated, since this is everything else than a small Task to do. It also tries to document already existing, but hidden or hard to find API's, which are provided by Microsoft.

How you can help? This can be done in numerous ways, over on the issue section, such as:

- Creating feature requests

- Reporting bugs

- Creating pull requests

- Informing about missing documentation/API's

### Collaboration

If you want to collaborate on this project more, than creating issues and PR's, feel free to contact me on any of the mentioned contacts at the bottom of the file.

## Installation

You can either get this package by downloading it from the NuGet Package Manager built in Visual Studio or from the official [nuget.org](https://www.nuget.org/packages/DulcisX) website. 

Also, DulcisX targets `.Net Framework 4.8`, so make sure that your projects targets the correct Framework version. 

Also you can install it via the **P**ackage **M**anager **C**onsole:

```
PM> Install-Package DulcisX
```

Please note, that the package is currently in the pre-release state. This indicates that the version is not yet production ready.

For a more detailed description on how to install this package please take [this guide](https://twentyfourminutes.github.io/DulcisX/guides/getting_started/installation.html?tabs=property-page-edit%2Cvisualstudio-install) from the packages wiki.

### Getting Started

There is a simple guide over on the [Documentation](https://twentyfourminutes.github.io/DulcisX/guides/getting_started/first-extension.html) which provides a step by step tutorial, on how to get started with Extension development and DulcisX.

## Documentation

DuclisX build upon a solid Documentation, which is very important for such a huge topic. Therefor the project hosts its own Documentation site, which can be found [here]().

Moreover DulcisX tries to provide a more detailed overview over the Visual Studio SDK. It hoists things such as Utility classes and other interesting stuff, which is hard to find on the official Microsoft Documentation.  

## Coverage

Currently DulcisX only supports *Visual Studio 2019*. 

Now how does it differ from the current DTE? This package provides a cleaner abstraction than the DTE does, additionally it is simpler to use and more powerful and most importantly open-source. However as it currently stands DuclisX doesn't cover all parts which the DTE does. See the table below for more detailed information.

| Status | Name                   |
| :----: | ---------------------- |
|   ❌    | ActiveDocument         |
|   ✅    | ActiveSolutionProjects |
|   ❌    | ActiveWindow           |
|   ❌    | AddIns                 |
|   ❌    | CommandBars            |
|   ❌    | CommandLineArguments   |
|   ❌    | Commands               |
|   ❌    | Debugger               |
|   ✅    | Documents              |
|   ✅    | Edition                |
|   🌀    | Events                 |
|   ❌    | Globals                |
|   ❌    | LocaleID               |
|   ❌    | MainWindow             |
|   ❌    | Mode                   |
|   ✅    | SelectedItems          |
|   🌀    | Solution               |
|   🌀    | SourceControl          |
|   🌀    | StatusBar              |
|   ❌    | ToolWindows            |
|   ❌    | UndoContext            |
|   ❌    | UserControl            |
|   ✅    | Version                |
|   ❌    | WindowConfigurations   |
|   ❌    | Windows                |

## Stay up to date

If you want to see what is currently planned and being worked on, head over to the projects [Trello](https://trello.com/b/wHTa9Vb8/dulcisx) board. 

## Notes

### Disclaimer

The project is not affiliated, associated, authorized, endorsed by, or in any way officially connected with Microsoft, nor do they provide support for it. This project is build and maintained by you, the community. 

### Contact information

If you feel like something is not working as intended or you are experiencing issues, feel free to create an issue. Also for feature requests just create an issue. For further information feel free to send me a [mail](mailto:office@twenty-four.dev) to `office@twenty-four.dev` or message me on Discord `24_minutes#7496`.

## Sponsors

I wanna thank [JetBrains](https://www.jetbrains.com/?from=DulcisX) for providing me and the project with a free Open Source license for their whole JetBrains suite. Their Tools greatly improve the development speed of this Project. If you want to get a free Open Source license for your own project and their collaborators, visit their [Open Source page](https://www.jetbrains.com/opensource/).

<a href="https://www.jetbrains.com/?from=DulcisX"><img width="350px" src="images/jetbrains_logo.png"></a>