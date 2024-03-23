using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.Diagnostics;
using System.Data.OleDb;
using QuickNav.Helper;
using System.Windows;
using System.Threading;

namespace QuickNav.BuildInCommands.WindowsFileSearch;

internal class FileSearchCommand : ICommand
{
    public string Description => "Run this command to search files on windows";

    public Uri Icon => new Uri("ms-appx://App/Assets/commands/filesearch.png");

    public Priority Priority(string query)
    {
        return QuickNavPlugin.Priority.Low;
    }

    public string CommandTrigger => "file:";

    public string[] Keywords => new string[] { "file", "explorer", "search" };

    public string Name(string query)
    {
        if (query == "") return "Search in your files";
        return "Search for \"" + query + "\" in your files";
    }

    public bool RunCommand(string searchTerm, out QuickNavPlugin.UI.ContentElement content)
    {
        content = null;

        if (searchTerm == "")
            return false;

        var connection = new OleDbConnection(@"Provider=Search.CollatorDSO;Extended Properties=""Application=Windows""");

        try
        {
            connection.Open();
            var query = $"SELECT TOP 10 System.ItemName, System.ItemPathDisplay FROM SystemIndex WHERE scope ='file:' AND System.ItemName LIKE '%{searchTerm}%'";

            var command = new OleDbCommand(query, connection);

            var searchedFilesView = new SearchedFilesViewElement();
            content = searchedFilesView;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string fileName = reader["System.ItemName"].ToString();
                    string filePath = reader["System.ItemPathDisplay"].ToString();

                    searchedFilesView.Files.Add((fileName, filePath));
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
        finally
        {
            connection.Close();
        }
        return true;
    }
}