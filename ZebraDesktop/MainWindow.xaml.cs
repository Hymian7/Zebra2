﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Zebra.Library;

namespace ZebraDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly App currentApp;
        private readonly ZebraDBManager manager;

        public MainWindow()
        {
            InitializeComponent();

            currentApp = (App)Application.Current;
            manager = currentApp.Manager;
        }

        private void frmMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (currentApp.ZebraConfig == null)
            {
                //SelectConfig();
            }
        }

        private void mitm_Exit_Click(object sender, RoutedEventArgs e) => Close();

        private void mitm_LoadConfig_Click(object sender, RoutedEventArgs e)
        {
            SelectConfig();
        }

        private void SelectConfig()
        {
            ConfigSelector frmConfigSelector = new ConfigSelector();
            frmConfigSelector.ShowDialog();

            if (frmConfigSelector.DialogResult == true)
            {
                currentApp.ZebraConfig = frmConfigSelector.SelectedConfiguration;

                Binding b = new Binding
                {
                    Source = currentApp.ZebraConfig.ConfigName,
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };

                lbl_ConfigName.SetBinding(Label.ContentProperty, b);


                //Load Pieces Page
                PiecesPage pagePieces = new PiecesPage(currentApp.Manager.GetAllPieces());
                tiPiecesFrame.Content = pagePieces;
                tcViews.IsEnabled = true;

                MessageBox.Show($"Konfigruation '{currentApp.ZebraConfig.ConfigName}' erfolgreich geladen");
            }
        }

        private void mitm_UnloadConfig_Click(object sender, RoutedEventArgs e)
        {
            currentApp.ZebraConfig = null;

            lbl_ConfigName.Content = "Keine Konfiguration geladen.";

            MessageBox.Show("Konfiguration erfolgreich geschlossen", "Konfiguration geschlossen");
        }
    }
}