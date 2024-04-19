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
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace CURSE.View
{
    /// <summary>
    /// Логика взаимодействия для ApplicantView.xaml
    /// </summary>
    public partial class ApplicantView : Page
    {
        public ApplicantView(ViewModel.MainVM mainVM)
        {
            InitializeComponent();
            var vm = DataContext as ListVacancy;
            vm?.SetMainVM(mainVM);
            //((ListDrinksVM)DataContext).SetMainVM(mainVM);
            //DataContext = new ListDrinksVM(mainVM);
        }
        
    }
}
