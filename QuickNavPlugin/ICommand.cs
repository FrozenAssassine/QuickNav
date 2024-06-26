﻿using System;
using QuickNavPlugin.UI;

namespace QuickNavPlugin
{
    public interface ICommand : IPlugin
    {
        /// <summary>
        /// Describes the functionality of the command.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Icon for the command.
        /// </summary>
        /// <param name="query">Maybe your icon should depend on the given query?</param>
        /// <returns></returns>
        Uri Icon(string query);
        /// <summary>
        /// Priority of the command, should it be shown on top or at bottom?
        /// </summary>
        /// <param name="query">Maybe your command priority should depend on the given query?</param>
        /// <returns>The priority of the command.</returns>
        Priority Priority(string query);
        /// <summary>
        /// Command Trigger, for example if you have a command-line plugin use ">" as trigger. With that the application can find your command. Leave it empty to let the user just find your command via keywords.
        /// </summary>
        string CommandTrigger { get; }
        /// <summary>
        /// Keywords with which your command can be found.
        /// </summary>
        string[] Keywords { get; }
        /// <summary>
        /// Name/Headline of the command.
        /// </summary>
        /// <param name="query">Maybe your command name/headline should depend on the given query?</param>
        /// <returns>The name/headline of the command.</returns>
        string Name(string query);
        /// <summary>
        /// This function is called to run the command.
        /// </summary>
        /// <param name="parameters">The parameters of the command.</param>
        /// <param name="content">The result, shown to the user.</param>
        /// <returns>Returns true or false, whether execution were successfully or not.</returns>
        bool RunCommand(string parameters, out ContentElement content);
        /// <summary>
        /// This function will be called when the MainWindow opens.
        /// </summary>
        void OnWindowOpened();
    }
}
