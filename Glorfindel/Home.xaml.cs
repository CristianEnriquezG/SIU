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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DatosAlumno alumno { get; set; }
        public MainWindow(DatosAlumno alumno)
        {
            InitializeComponent();
            Placeholder.Navigate(new LandingAlumno());
            this.alumno = alumno;
            this.Loaded += Landing_Loaded;
        }
        private void Landing_Loaded(object sender, RoutedEventArgs e)
        {
            var window = (MainWindow)Window.GetWindow(this);
            Nombre.Content = alumno.Apellido.ToUpper() + " " + alumno.Nombre.ToUpper();
            Carrera.Content = alumno.Carrera.ToUpper();
        }
        
        private void Reportes_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Notificaciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Notificaciones.SelectedItem == Nueva)
            {
                Placeholder.Navigate(new Notificaciones());
            }
            if (Notificaciones.SelectedItem == Existente)
            {
                Placeholder.Navigate(new Existente());
            }
        }

        private void SalirA_Click(object sender, RoutedEventArgs e)
        {
            Login start = new();
            start.Show();
            this.Close();
        }
    }
}
