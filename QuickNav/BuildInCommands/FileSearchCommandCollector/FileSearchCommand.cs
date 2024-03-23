using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.Diagnostics;
using System.Data.OleDb;
using QuickNav.Helper;
using System.Windows;

namespace QuickNav.BuildInCommands.WindowsFileSearch;

internal class FileSearchCommand : IUnknownCommandCollector, ITriggerCommand
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

            // File name search (case insensitive), also searches sub directories
            var query = $"SELECT TOP 50 System.ItemName, System.ItemPathDisplay FROM SystemIndex WHERE scope ='file:' AND System.ItemName LIKE '%{searchTerm}%'";

            var command = new OleDbCommand(query, connection);

            var listViewElement = new ListViewElement();
            content = listViewElement;
            listViewElement.Orientation = QuickNavPlugin.UI.Orientation.Vertical;
            listViewElement.Children.Clear();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    //string filePath = reader["System.ItemPathDisplay"].ToString();
                    string fileName = reader["System.ItemName"].ToString();
                    string filePath = reader["System.ItemPathDisplay"].ToString();

                    FlyoutElement flyout = new FlyoutElement();
                    flyout.AddButton("Copy Path", (sender) =>
                    {
                        Clipboard.SetText(filePath);
                    });
                    flyout.AddButton("Copy FileName", (sender) =>
                    {
                        Clipboard.SetText(fileName);
                    });
                    flyout.AddButton("Show in explorer", (sender) =>
                    {
                        FileExplorerHelper.OpenExplorer(filePath);
                    });

                    listViewElement.Children.Add(new LabelElement(fileName, flyout));
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