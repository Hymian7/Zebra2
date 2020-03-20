using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Zebra.Library;

namespace ZebraDesktop
{
    /// <summary>
    /// Interaktionslogik für ConfigSelector.xaml
    /// </summary>
    public partial class ConfigSelector : Window
    {
        public ZebraConfig SelectedConfiguration { get; set; }

        public ConfigSelector()
        {
            InitializeComponent();
            SelectedConfiguration = null;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (lvConfigs.SelectedIndex != -1)
            {
                LoadConfiguration(lvConfigs.SelectedItem as FileInfo);                
            }
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void frmConfigSelector_Loaded(object sender, RoutedEventArgs e)
        {
            var configs = Directory.GetFiles("configs", "*.zebraconfig");
            List<FileInfo> files = new List<FileInfo>();

            foreach (var direntry in configs)
            {
                files.Add(new FileInfo(direntry));                
            }

            foreach (var file in files)
            {
                lvConfigs.Items.Add(file);
            }

        }

        private void lvConfigs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = lvConfigs.SelectedItem as FileInfo;
            if (item != null)
            {
                LoadConfiguration(item);
            }
        }

        private void LoadConfiguration(FileInfo file)
        {
            try
            {
                SelectedConfiguration = ZebraConfig.FromXML((lvConfigs.SelectedItem as FileInfo).FullName);
                this.DialogResult = true;
                this.Close();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler beim Laden der Konfiguration");
            }
        }
    }
}
