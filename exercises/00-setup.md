# Welcome!

We're going to get set up for our MCP Server. Hopefully Rob told you all about MCP by now...

## Git Repo

You can find everything for today at:

 - https://github.com/robconery/mcp

## The Story, Today

The Cassini spacecraft was a joint NASA-ESA mission that studied Saturn and its moons from 2004 to 2017, providing unprecedented insights into the ringed planet's complex system. During its 13-year mission, Cassini made groundbreaking discoveries including evidence of subsurface oceans on moons Enceladus and Titan, detailed observations of Saturn's rings and atmospheric dynamics, and thousands of high-resolution images that revolutionized our understanding of the Saturnian system before deliberately plunging into Saturn's atmosphere in September 2017.

You get to work with the _actual_ Cassini mission plan today!

## Get the Data

There are two data files you can find in the zip file in this project:

 - The CSV with the data
 - The SQLite file

## Ensure SQLite is installed

You should have SQLite installed on your machine:

 - Open a terminal and type `sqlite` or `sqlite3` and see what happens.
 - Install the `SQLite Viewer` extension in VS Code

## Create Your Project

We'll be using ASP.NET Minimal API for this project, which you should get setup now:

```
dotnet new webapi -n cassini
```

Create a `/Data` directory and pull in the CSV and SQLite files