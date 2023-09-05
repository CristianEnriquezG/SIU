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
    public partial class LandingProfesor : Page
    {
        public LandingProfesor()
        {
            InitializeComponent();
            this.Loaded += LandingProfesor_Loaded;
            
        }

        private void LandingProfesor_Loaded(object sender, RoutedEventArgs e)
        {
            var window = (HomeProfesor)Window.GetWindow(this);
            labelBienvenido.Content = "Bienvenido " + window.profesor.Nombre.ToString().ToUpper() + " " + window.profesor.Apellido.ToString().ToUpper();
        }
    }
}
