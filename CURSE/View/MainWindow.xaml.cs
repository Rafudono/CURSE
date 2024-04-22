using CURSE.Model;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CURSE.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                var hz = VacancyRepository.Instance.Search("");
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

        private void TextBlock_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}