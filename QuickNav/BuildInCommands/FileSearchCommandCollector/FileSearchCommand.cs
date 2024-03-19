using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Collections.ObjectModel;

namespace QuickNav.BuildInCommands.WindowsFileSearch;

internal class FileSearchCommand : IUnknownCommandCollector
{
    public string Description => "Run this command to search files on windows";

    public Uri Icon => new Uri("ms-appx://App/Assets/commands/filesearch.png");

    public Priority Priority => Priority.Low;

    public string Name(string query)
    {
        if (query == "") return "Search in your files";
        return "Search for \"" + query + "\" in your files";
    }

    public bool RunCommand(string searchTerm, out ContentElement content)
    {
        content = null;
        var connection = new OleDbConnection(@"Provider=Search.CollatorDSO;Extended Properties=""Application=Windows""");

        try
        {
            connection.Open();

            // File name search (case insensitive), also searches sub directories
            var query = $"SELECT TOP 50 System.ItemName FROM SystemIndex WHERE scope ='file:' AND System.ItemName LIKE '%{searchTerm}%'";

            var command = new OleDbCommand(query, connection);

            var listViewElement = new ListViewElement();
            content = listViewElement;
            listViewElement.Orientation = Orientation.Vertical;
            listViewElement.Children.Clear();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    //string filePath = reader["System.ItemPathDisplay"].ToString();
                    string fileName = reader["System.ItemName"].ToString();

                    listViewElement.Children.Add(new LabelElement(fileName));
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