using Microsoft.Win32;
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

namespace ZebraDesktop
{
    /// <summary>
    /// Interaktionslogik für frmNewConfig.xaml
    /// </summary>
    public partial class frmNewConfig : Window
    {
        public frmNewConfig()
        {
            InitializeComponent();
        }

        private void btnBrowseDBPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            if (!(ofd.FileName == null))
            {
                tbDBPath.Text = ofd.FileName;
            }

            
        }

        private void btnBrowseArchivePath_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Einfach Pfad in die Textbox kopieren");
        }
        private void btnBrowseTempDirPath_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Einfach Pfad in die Textbox kopieren");
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!(tbConfigName.Text =="" || tbDBPath.Text == "" || tbArchivePath.Text == ""))
            {
                Zebra.Library.ZebraConfig newconf = new Zebra.Library.ZebraConfig(tbConfigName.Text, Zebra.Library.DatabaseProvider.SQLite, new Zebra.Library.SQLiteCredentials(tbDBPath.Text), Zebra.Library.ArchiveType.Local, new Zebra.Library.LocalArchiveCredentials(tbArchivePath.Text), tbTempDirPath.Text);

                if (newconf.Serialize(@"configs"))
                {
                    this.Close();
                }
            }
        }

    }
}
