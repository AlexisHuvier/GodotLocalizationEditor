using GodotLocalizationEditor.Utils;
using GodotLocalizationEditor.Widgets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace GodotLocalizationEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly List<CSVFile> Files = new();
        public static string Dir = null!;

        public MainWindow()
        {
            InitializeComponent();
        }

        void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "Sélectionnez le dossier où se trouver les .csv",
                UseDescriptionForTitle = true,
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                ShowNewFolderButton = true
            };
            
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Dir = dialog.SelectedPath;
                Files.Clear();
                var directories = new List<string> { dialog.SelectedPath };
                while (directories.Count > 0)
                {
                    foreach(var dir in Directory.GetDirectories(directories[0]))
                        directories.Add(dir);
                    foreach (var file in Directory.GetFiles(directories[0]))
                        if(file.EndsWith(".csv"))
                            Files.Add(new CSVFile(file));
                    directories.RemoveAt(0);
                }
                FileBox.Items.Clear();
                foreach (var file in Files)
                    FileBox.Items.Add(file.Name);
            }
        }

        void QuitItem_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Dir;
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Files.Add(new CSVFile(saveFileDialog.FileName, new []{"en", "fr"}));
                FileBox.Items.Add(Files[^1].Name);
            }
        }

        void FileBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTranslationList();
        }

        void UpdateTranslationList()
        {
            if (KeyList != null)
            {
                KeyList.Items.Clear();
                foreach (var translation in Files.Where(file => file.Name == FileBox.SelectedItem as string)
                             .SelectMany(file => file.Translations))
                    KeyList.Items.Add(translation.Key);
            }
        }

        void AddTranslationButton_OnClick(object sender, RoutedEventArgs e)
        {
            var askKey = new AskKey();
            if (askKey.ShowDialog() == true)
            {
                Files.Where(file => file.Name == FileBox.SelectedItem as string).ToList()[0].Translations.Add(new Translation(askKey.Answer, new List<string>() {"", ""}));
                KeyList.Items.Add(askKey.Answer);
            }
        }

        void SortButton_OnClick(object sender, RoutedEventArgs e)
        {
            Files.Where(file => file.Name == FileBox.SelectedItem as string).ToList()[0].Translations.Sort((translation, translation1) => translation.Key.CompareTo(translation1.Key));
            UpdateTranslationList();
        }

        void KeyList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var askKey = new AskKey();
            if (askKey.ShowDialog() == true)
            {
                Files.Where(file => file.Name == FileBox.SelectedItem as string).ToList()[0].Translations
                    .Where(x => x.Key == KeyList.SelectedItem as string).ToList()[0].Key = askKey.Answer;
                UpdateTranslationList();
            }
        }

        void DelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Files.Where(file => file.Name == FileBox.SelectedItem as string).ToList()[0].Translations.Remove(
                Files.Where(file => file.Name == FileBox.SelectedItem as string).ToList()[0].Translations
                    .Where(x => x.Key == KeyList.SelectedItem as string).ToList()[0]);
            UpdateTranslationList();
        }

        void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Files.Where(file => file.Name == FileBox.SelectedItem as string).ToList()[0].Save();
        }

        void EnBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (KeyList.SelectedItem != null)
            {
                var translation = Files.Where(file => file.Name == FileBox.SelectedItem as string).ToList()[0]
                    .Translations
                    .Where(x => x.Key == KeyList.SelectedItem as string).ToList()[0];
                translation.Translations[0] = EnBox.Text;
            }
        }

        void FrBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (KeyList.SelectedItem != null)
            {
                var translation = Files.Where(file => file.Name == FileBox.SelectedItem as string).ToList()[0]
                    .Translations
                    .Where(x => x.Key == KeyList.SelectedItem as string).ToList()[0];
                translation.Translations[1] = FrBox.Text;
            }
        }

        void KeyList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KeyList.SelectedItem != null)
            {
                var translation = Files.Where(file => file.Name == FileBox.SelectedItem as string).ToList()[0]
                    .Translations
                    .Where(x => x.Key == KeyList.SelectedItem as string).ToList()[0];
                EnBox.Text = translation.Translations[0];
                FrBox.Text = translation.Translations[1];
            }
            else
            {
                EnBox.Text = "";
                FrBox.Text = "";
            }
        }
    }
}
