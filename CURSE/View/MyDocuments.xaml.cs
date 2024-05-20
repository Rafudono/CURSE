using CURSE.ViewModel;
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

namespace CURSE.View
{
    /// <summary>
    /// Логика взаимодействия для MyDocuments.xaml
    /// </summary>
    public partial class MyDocuments : Window
    {
        public MyDocuments()
        {
            InitializeComponent();
            ((MyDocumentsVM)DataContext).SetClose(Close);
        }
    }
}
