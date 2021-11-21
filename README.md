This is evolving into a tool to generate markdown documentation for .NET C# code, including links to source code and types. It uses Roslyn to capture solution metadata in an abstraction layer, where it can be projected into a presentation form of choice. In my case, I'm targeting GitHub-flavored markdown as the output.

The Roslyn heavy lift was done by [Andriy Rebrin](https://www.upwork.com/o/profiles/users/_~01f302b2d51f8153bd/)/[dr-o-ne](https://github.com/dr-o-ne). He is amazing. His work is mainly on the [RoslynDoc.Library project](https://github.com/adamosoftware/RoslynSyntaxTreeAnalyzer/tree/master/RoslynDoc.Library). My portion is mainly the [MarkdownViewer](https://github.com/adamosoftware/RoslynSyntaxTreeAnalyzer/tree/master/MarkdownViewer.RazorPages), which is really just a shell for executing Razor views that present the documentation I'm looking for. (For a time, I though this would be a .NET Razor Pages app, but I imagined that hosting msbuild online would be tricky, so I shifted it back to a desktop model.)

You can see sample output of this in my Postulate Wiki pages for [SQL Server](https://github.com/adamosoftware/Postulate.Lite/wiki/SQL-Server-CRUD-Methods) and [MySQL](https://github.com/adamosoftware/Postulate.Lite/wiki/MySQL-CRUD-Methods). This gives me a way to document public methods with links to source code and types that is very easy to keep updated as the source evolves.

## Samples in use
- [DbCache](https://github.com/adamfoneil/DbCache#dbcachelibrarydbcache-dbcachecs)
- [DataTables.Library](https://github.com/adamfoneil/DataTables.Library#datatableslibrarydatatableextensions-datatableextensionscs)
- [Dapper.Repository wiki](https://github.com/adamfoneil/Dapper.Repository/wiki)
- [Dapper.CX wiki](https://github.com/adamfoneil/Dapper.CX/wiki/Crud-method-reference)
- [Dapper.QX wiki](https://github.com/adamfoneil/Dapper.QX/wiki/Reference)

## WinForms app
Nobody likes WinForms anymore (for understandable reasons), but I'm still very productive in it. This is what I'm prototyping/experimenting with to demonstrate core functionality.

![img](https://adamosoftware.blob.core.windows.net/images/markdowner-winform.png)

## WPF app
I have a [WPF version](https://github.com/adamosoftware/RoslynMarkdowner/tree/master/RoslynMarkdowner.WPF) of the desktop client done by [Vadim A](https://www.upwork.com/o/profiles/users/~01a778def0bc56bf99/), who is awesome. At the moment it looks very similar to the WinForm version, so I decided against showing a picture.

## Background

I like to build class libraries (i.e. [Postulate.Lite](https://github.com/adamosoftware/Postulate.Lite) and [TestDataGen](https://github.com/adamosoftware/TestDataGen)), and I like to document them with links to source code line numbers and relevant top-level method info so that people have at least a chance of finding them useful for something. The problem is documentation is pretty hard to build manually much less keep updated -- especially as line numbers shift as a project evolves. There are lots of .NET documentation generators out there, but they don't do quite what I want. I've found that they build content that's hard to reuse elsewhere as well as being too voluminous, not lending itself to quick scanning -- something that would fit on a single page readme.

The purpose of this repo is to offer a library for analyzing C# code and returning an [abstraction layer of model classes](https://github.com/adamosoftware/RoslynSyntaxTreeAnalyzer/tree/master/RoslynDoc.Library/Models) that can be used with any template mechanism. Ultimately, I want to generate GitHub markdown in a particular way for use in my own class library readmes, but to do that I need to get the Roslyn code analysis working.

I started looking at Roslyn for the first time seriously very recently, and I found it pretty forbidding. Yes it's amazingly cool, but I got stuck pretty quickly. This repo began as a sample project using [this](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis) as a starting point. This where I got Andriy's help when I realized I was lost. He got the hard part working, and I followed up with the markdown generation using MVC Razor.

Razor is really powerful and I love it, but it turns out to be a little difficult to use with markdown generation. Markdown is sensitive to indentation in a way that HTML is not, so I had to back out all the indentation that would normally make sense in HTML. You can see the markdown Razor view [here](https://github.com/adamosoftware/RoslynSyntaxTreeAnalyzer/blob/master/MarkdownViewer/Views/Markdown/Index.cshtml)
