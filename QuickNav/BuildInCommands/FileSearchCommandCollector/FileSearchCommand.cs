﻿using QuickNavPlugin.UI;
using QuickNavPlugin;
using System;
using System.Diagnostics;
using System.Data.OleDb;
using QuickNav.Helper;
using System.Windows;
using System.Threading;
using QuickNav.Models;
using Microsoft.UI.Xaml.Controls;
using QuickNav.Views;
using System.Collections.Generic;

namespace QuickNav.BuildInCommands.WindowsFileSearch;

internal class FileSearchCommand : ICommand, IBuildInCommand
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
        return false;
    }

    public bool RunCommand(string parameters, out Page content, out double addWidth, out double addHeight)
    {
        content = null;
        addWidth = 0;
        addHeight = 0;

        if (parameters == "")
            return false;

        var connection = new OleDbConnection(@"Provider=Search.CollatorDSO;Extended Properties=""Application=Windows""");

        try
        {
            connection.Open();
            var query = $"SELECT TOP {CommandSettings.AmountOfFiles} System.ItemName, System.ItemPathDisplay FROM SystemIndex WHERE scope ='file:' AND System.ItemName LIKE '%{parameters}%'";

            var command = new OleDbCommand(query, connection);

            var searchedFilesView = new SearchedFilesView();
            content = searchedFilesView;

            List<(string name, string path)> files = new List<(string name, string path)>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string fileName = reader["System.ItemName"].ToString();
                    string filePath = reader["System.ItemPathDisplay"].ToString();

                    files.Add((fileName, filePath));
                }
            }

            searchedFilesView.ShowFiles(files);
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