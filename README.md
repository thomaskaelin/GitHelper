# GitHelper

## Introduction

GitHelper is a small CLI-tool that I wrote in C# as a coding kata. Its main goal is to cover my most often used Git-features. Of course, you can use any Git-Client such as [Sourcetree](https://www.sourcetreeapp.com/) to do so, but in my experience, I am much faster using my little helper.

## Building the app

Running the script `build.ps1` will create an "all in one"-executable at `build\GitHelper.exe`. 

## Running the app

The executable reads its configuration from a file called `settings.json`. When running the executable without such a file, it will create a dummy configuration-file in the execution directory. Please set the values according to your needs and re-run the executable afterwards.

## Dependencies

| Library                                        | Description                                                                                                                                                                                                                     |
|------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [CliWrap](https://github.com/Tyrrrz/CliWrap)   | CliWrap is a library for interacting with external command-line interfaces. It provides a convenient model for launching processes, redirecting input and output streams, awaiting completion, handling cancellation, and more. |
| [Json.NET](https://www.newtonsoft.com/json)    | Popular high-performance JSON framework for .NET.                                                                                                                                                                               |
| [Specter.Console](https://spectreconsole.net/) | A .NET library that makes it easier to create beautiful console applications.                                                                                                                                                   |

## Possible Improvements
* Feature: Push commits to remote
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