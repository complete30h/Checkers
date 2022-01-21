using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CheckersWPF
{
    /// <summary>
    /// Логика взаимодействия для SubWindow.xaml
    /// </summary>
    public partial class SubWindow : Window
    {
        public SubWindow()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)W.IsChecked)
            {
                MainWindow main = this.Owner as MainWindow;
                main.white = true;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MainWindow main = this.Owner as MainWindow;
                main.white = false;
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
