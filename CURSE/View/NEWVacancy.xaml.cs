using CURSE.Model;
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
    /// Логика взаимодействия для NEWVacancy.xaml
    /// </summary>
    public partial class NEWVacancy : Window
    {
        public NEWVacancy()
        {
            InitializeComponent();
            var vm = ((NEWVacancyVM)DataContext);
            vm.SetMainVM(listFoa);
            ((NEWVacancyVM)DataContext).SetClose(Close);
        }
    }
}
