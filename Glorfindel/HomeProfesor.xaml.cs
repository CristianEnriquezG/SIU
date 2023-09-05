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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Glorfindel
{
    /// <summary>
    /// Lógica de interacción para HomeProfesor.xaml
    /// </summary>

    
    public partial class HomeProfesor : Window
    {
        public DatosProfesor profesor { get; set; }
        public HomeProfesor(DatosProfesor profesor)
        {
            InitializeComponent();
            ProfesorFrame.Navigate(new LandingProfesor());
            this.profesor = profesor;
            this.Loaded += Landing_Loaded;
        }

        private void Landing_Loaded(object sender, RoutedEventArgs e)
        {
            var window = (HomeProfesor)Window.GetWindow(this);
            Nombre.Content = profesor.Apellido.ToUpper() + " " + profesor.Nombre.ToUpper();
            
        }
        private void Boton_Salir_Click(object sender, RoutedEventArgs e)
        {
            Login salida = new();
            salida.Show();
            this.Close();
        }

        private void RecursoAcademico_SelectionChanged(object sender, SelectionChangedEventArgs e)

        {
            int indice = RecursoAcademico.SelectedIndex;
            switch (indice)
            {
                case 1:
                    ProfesorFrame.Navigate(new CrearRecurso());
                    break;
                case 2:
                    ProfesorFrame.Navigate(new ModificarRecurso());
                    break;
                case 3:
                    ProfesorFrame.Navigate(new CalificarRecurso());
                    break;
            }
        }

        private void Informes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int indice = Informes.SelectedIndex;
            switch (indice)
            {
                case 1:
                    ProfesorFrame.Navigate(new InformesCurso());
                    break;
                case 2:
                    ProfesorFrame.Navigate(new InformeAlumno());
                    break;
                
            }
        }
    
    }
}
