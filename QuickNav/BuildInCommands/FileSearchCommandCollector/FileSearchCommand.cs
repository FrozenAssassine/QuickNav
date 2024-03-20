using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.Diagnostics;
using System.Data.OleDb;
using QuickNav.Helper;
using System.Windows;
using QuickNav.Models;

namespace QuickNav.BuildInCommands.WindowsFileSearch;

internal class FileSearchCommand : IUnknownCommandCollector, ITriggerCommand
{
    public string Description => "Run this command to search files on windows";

    public Uri Icon => new Uri("ms-appx://App/Assets/commands/filesearch.png");

    public Priority Priority => Priority.Low;

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
                MenuFlyoutElement flyout = new MenuFlyoutElement();
                flyout.AddItem("Copy Path", "", (sender) =>
                {
                    Clipboard.SetText((sender.Tag as FileSearchResultItem).FilePath);
                });
                flyout.AddItem("Copy FileName", "", (sender) =>
                {
                    Clipboard.SetText((sender.Tag as FileSearchResultItem).FileName);
                });
                flyout.AddItem("Show in explorer", "", (sender) =>
                {
                    FileExplorerHelper.OpenExplorer((sender.Tag as FileSearchResultItem).FilePath);
                });

                while (reader.Read())
                {
                    string fileName =reader["System.ItemName"].ToString();
                    listViewElement.Children.Add(new LabelElement(fileName, flyout, new FileSearchResultItem(fileName, reader["System.ItemPathDisplay"].ToString())));
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