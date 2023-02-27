using System;
using System.Collections.Generic;
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
using ZebraDesktop.ViewModels;

namespace ZebraDesktop.Views
{
    /// <summary>
    /// Interaktionslogik für frmPartDetail.xaml
    /// </summary>
    public partial class frmPartDetail : Window
    {
        public frmPartDetail(PartDTO part)
        {
            InitializeComponent();
            this.DataContext = new PartDetailViewModel(part);
        }
    }
}
