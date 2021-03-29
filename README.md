# DotnetCliApp

This is me playing around with some ideas combining StarFruit and Spectre.Console. It is a prototype using an interim semantic model.

## Building

Let me know if it is not just clone and build successfully.

## Demo actions

This is a prototype, so not everything works. Here are my demo notes.

The assets are in the artifacts directory of the repo. 

If you would like to leave `--ux-level` out of your demos, set the environment variable `DOTNETCLI_UXLEVEL`.

### Display Installed templates

```C#
.\dotnet-cliapp.exe --ux-level 4 new list
```

To see a different layout (see how to define your own below):

```C#
.\dotnet-cliapp.exe --ux-level 5 new list
```

To Specify columns to display:

```C#
.\dotnet-cliapp.exe --ux-level 4 new list --columns Author
```

This will remove the default columns and add the Author column (at the moment, this is case sensitive).

To output the list to a JSON file. Note this will work with any table using this tech:

```C#
.\dotnet-cliapp.exe --render-to json --ux-level 4 new list > temp.json
```

If you don't pipe it to a file, it will output to the console.

### Select a template

```C#
.\dotnet-cliapp.exe --ux-level 4 new
```

Feedback on this is welcome. Reminder - this will not be the only experience and may not be the default experience. 

### Display Help

```C#
.\dotnet-cliapp.exe --ux-level 4 --help2 new
```

## Play with the output!!!

Since this is a prototype, we haven't worked out the right knobs and dials for the experience, and also I would really like to see what beautiful layouts you create. So, if you want to play, pick an integer and expand the switch statements in TerminalRender/Terminal.cs. Instructions at the top of that file. 

If want to share your ideas, just use a constant and associate your github handle so I can find you and do a PR into the repo. 

## See the StarFruit prototype in action!

The CLI for this prototype is defined with the StarFruit app model which is a prototype for easy access to System.CommandLine. Check it out in the DotnetCommand.cs and NewCommand.cs classes in dotnet-cliapp. 

**Note: the intent for this is to be source generation, however, that is buggy at present, so I am explicitly creating the files that shoudl be generated. Because of this, any changes you make will not change the CLI.

## Explanation of project layout

### dotnet-cliapp

This project contains the definition of the CLI. This is where all CLI specific code should live.

The files `CommandSource.cs` and `CommandSourceResult.cs` should be generated but are currently manually maintained. Because of this

* These files may be ugly and confusing.
* Adding parameters or properties should result in more features in the CLI, this doesn't happen unless those features are added to these files, which is tricky.

### Common

This project contains the semantic model definitions for rendering.

### CliSuport

This project contains the bridge between application specific code and the different output mechanisms

### JsonRender

Contains the code to render to JSON

### TerminalRender

Contains the code to render to the terminal.

