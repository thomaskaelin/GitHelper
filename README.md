# GitHelper

## Introduction

GitHelper is a small CLI-tool that I wrote in C# as a coding kata. Its main goal is to cover my most often used Git-features. Of course, you can use any Git-Client such as [Sourcetree](https://www.sourcetreeapp.com/) to do so, but in my experience, I am much faster using my little helper.

## Building the app

Running the script `build.ps1` will create an "all in one"-executable at `build\GitHelper.exe`. 

## Running the app

The executable requires a single parameter `-p`, containing the path to a Git-repository cloned on your hard drive.
```
GitHelper.exe -p C:\Repos\MyRepo
```

## Dependencies

| Library                                                               | Description                                                                                                                                                                                                                                                                                                                           |
|-----------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [CliWrap](https://github.com/Tyrrrz/CliWrap)                          | CliWrap is a library for interacting with external command-line interfaces. It provides a convenient model for launching processes, redirecting input and output streams, awaiting completion, handling cancellation, and more.                                                                                                       |
| [CommandLineParser](https://github.com/commandlineparser/commandline) | The Command Line Parser Library offers CLR applications a clean and concise API for manipulating command line arguments and related tasks, such as defining switches, options and verb commands. It allows you to display a help screen with a high degree of customization and a simple way to report syntax errors to the end user. |
| [Specter.Console](https://spectreconsole.net/)                        | A .NET library that makes it easier to create beautiful console applications.                                                                                                                                                                                                                                                         |

## Possible Improvements
* Features
  * Reset local working copy
  * Push commits to remote
* Replace hardcoded constants with parameters
  * Path to the Git-Executable (see `GitCliWrapper`)
  * Name of the remote-repository (see `GitFlows`)
  * Encoding of the console (see `UserInteraction`)
  * Color of the user input (see `UserInteraction`)
* Improve configuration of parameters, either by:
  * Replace `CommandLineParser`-library with [Specter.Console.Cli](https://spectreconsole.net/cli/getting-started)
  * Read parameter(s) from a configuration file
* Extract hardcoded translations to `RESX`-files
* Move actions in `UserInteraction` into separate classes

## License

This project is licensed under the MIT License.

```
MIT License

Copyright (c) 2024 Thomas Kälin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

```