using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using QuickNav.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.ApplicationModel;

namespace QuickNav.Views
{
    public sealed partial class AboutPage : Page
    {
        public string AppVersion => AppVersionHelper.GetAppVersion();
        public string DeveloperName => Package.Current.PublisherDisplayName;
        public AboutPage()
        {
            this.InitializeComponent();
        }

        private async void NavigateToLink_Click(Controls.SettingsControl sender)
        {
            if (sender.Tag == null)
                return;

            await Windows.System.Launcher.LaunchUriAsync(new Uri(sender.Tag.ToString()));
        }

        private void SetChangelog()
        {
            //Simple parser to make headlines bigger and add paragraphs
            string filePath = Path.Combine(Package.Current.InstalledLocation.Path, "Assets", "changelog.txt");
            var data = File.ReadAllLines(filePath);
            List<Paragraph> paragraphs = new List<Paragraph> { new Paragraph() };
            for (int i = 0; i < data.Length; i++)
            {
                string currentLine = data[i];
                Run line = new Run();

                //Headline:
                if (currentLine.StartsWith("#"))
                {
                    currentLine = currentLine.Remove(0, 1);
                    line.FontSize = 24;
                }
                //Paragraph
                else if (currentLine.StartsWith("---"))
                {
                    currentLine = currentLine.Remove(0, 3);
                    paragraphs.Add(new Paragraph());
                }

                line.Text = currentLine + "\n";
                paragraphs[paragraphs.Count - 1].Inlines.Add(line);
            }
            foreach (var paragraph in paragraphs)
            {
                ChangelogDisplay.Blocks.Add(paragraph);
            }
        }
    }
}
