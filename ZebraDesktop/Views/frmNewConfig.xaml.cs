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

namespace ZebraDesktop.Views
{
    /// <summary>
    /// Interaktionslogik für frmNewConfig.xaml
    /// </summary>
    public partial class frmNewConfig : Window, IClosable
    {
        public frmNewConfig()
        {
            InitializeComponent();
            (this.DataContext as ZebraDesktop.ViewModels.NewConfigViewModel).ParentContainer = this;
        }

        void IClosable.Close() => this.Close();

    }
}
