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

namespace ZebraDesktop
{
    /// <summary>
    /// Interaktionslogik für frmNewPiece.xaml
    /// </summary>
    public partial class frmNewPiece : Window
    {
        private readonly App currentApp;
        private readonly ZebraDBManager manager;

        public frmNewPiece()
        {
            InitializeComponent();            
        }
    }
}
