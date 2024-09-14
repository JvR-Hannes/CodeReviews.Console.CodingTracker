﻿using System.Configuration;
using System.Data.SQLite;
using Spectre.Console;
namespace Coding_Tracker_JVR_Hannes
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbFolderPath = ConfigurationManager.AppSettings["DatabasePath"];
            string fullDbPath = Path.Combine(dbFolderPath, "coding-Tracker.db");
            string connectionString = $"Data Source={fullDbPath}; Version=3;";

            var codingController = new CodingController(connectionString);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS coding_session (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT,
                        EndTime TEXT,
                        Duration REAL
                    )";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            GetUserInput(codingController);
        }

        static void GetUserInput(CodingController codingController)
        {
            Console.Clear();
            bool closeApp = false;
            
            while (closeApp == false)
            {
                AnsiConsole.Markup("\n [purple]MAIN MENU[/]");
                AnsiConsole.MarkupLine("\n[lime]++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++[/]");
                AnsiConsole.MarkupLine("\n[bold]What would you like to do?[/]");
                AnsiConsole.MarkupLine("\nType [bold]0[/] to Close Application");
                AnsiConsole.MarkupLine("Type [bold]1[/] to View All Time Records.");
                AnsiConsole.MarkupLine("Type [bold]2[/] to Insert Time.");
                AnsiConsole.MarkupLine("Type [bold]3[/] to Delete Time.");
                AnsiConsole.MarkupLine("Type [bold]4[/] to Clear the Database.");
                AnsiConsole.MarkupLine("\n[lime]++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++[/]\n");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        AnsiConsole.Markup("\n\n[yellow]Goodbye![/]\n\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        var sessions = codingController.GetAllCodingSessions();

                        if (sessions.Count > 0)
                        {
                            foreach (var codingSession in sessions)
                            {
                                AnsiConsole.MarkupLine($"{codingSession.Id}: [bold green]Start - {codingSession.StartTime}[/], [bold red]End - {codingSession.EndTime}[/], [bold aqua]Duration - {codingSession.Duration}[/]");
                            }
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]No Records Found!");
                        }

                        break;
                    case "2":
                        DateTime startTime, endTime;

                        try
                        {
                            startTime = AnsiConsole.Ask<DateTime>("[blue]Enter start time[/] [bold yellow](yyyy-MM-dd HH:mm:ss): [/]");
                        }
                        catch (FormatException)
                        {
                            AnsiConsole.MarkupLine("[red]Invalid start time format! Please use yyyy-MM-dd HH:mm:ss.[/]");
                            break;
                        }

                        try
                        {
                            endTime = AnsiConsole.Ask<DateTime>("[blue]Enter end time[/] [bold yellow](yyyy-MM-dd HH:mm:ss): [/]");
                        }
                        catch (FormatException)
                        {
                            AnsiConsole.MarkupLine("[red]Invalid end time format! Please use yyyy-MM-dd HH:mm:ss.[/]");
                            break;
                        }

                        var session = new CodingSession { StartTime = startTime, EndTime = endTime };
                        codingController.InsertCodingSession(session);

                        break;
                    case "3":
                        AnsiConsole.Markup("[blue]Enter the ID of the session to delete: [/]");
                        int id;
                        if (int.TryParse(Console.ReadLine(), out id))
                        {
                            codingController.DeleteCodingSession(id);
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID format.");
                        }
                        break;
                    case "4":
                        if (AnsiConsole.Confirm("[yellow]Are you sure you want to delete all records? This action cannot be undone. Proceed with deletion?[/]"))
                        {
                            codingController.ClearDatabase();
                            AnsiConsole.MarkupLine("[green]All records have been deleted.[/]");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[green]Operation cancelled.[/]");
                        }
                        break;
                    default:
                        AnsiConsole.Markup("\n[bold red]Invalid Command.[/] Please type a number from 0 to 3");
                        break;
                }
            }
        }
    }
}
