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

namespace Glorfindel
{
    /// <summary>
    /// Lógica de interacción para Landing.xaml
    /// </summary>
    public partial class LandingAlumno : Page
    {
        public LandingAlumno()
        {
            InitializeComponent();
            this.Loaded += LandingAlumno_Loaded;
            
        }

        private void LandingAlumno_Loaded(object sender, RoutedEventArgs e)
        {
            var window = (MainWindow)Window.GetWindow(this);
            labelBienvenido.Content = "Bienvenido " + window.alumno.Nombre.ToString().ToUpper() + " " + window.alumno.Apellido.ToString().ToUpper() ;
        }
    }
}
