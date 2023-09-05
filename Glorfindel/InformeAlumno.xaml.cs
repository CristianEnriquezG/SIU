using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Lógica de interacción para InformeAlumno.xaml
    /// </summary>
    public partial class InformeAlumno : Page
    {
        public InformeAlumno()
        {
            InitializeComponent();
            this.Loaded += InformeAlumno_Loaded;
        }

        private void InformeAlumno_Loaded(object sender, RoutedEventArgs e)
        {
            fillalumnos();
        }
        public void fillalumnos()
        {
            var window = (HomeProfesor)Window.GetWindow(this);
            int LegajoProfesor = window.profesor.NumeroLegajoProfesor;
            string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani;Allow User Variables = True;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT alumno.LegajoAlumno, alumno.Nombre, alumno.Apellido, alumno.Carrera " +
                                "FROM alumno " +
                                "INNER JOIN alumno_materia ON alumno.LegajoAlumno = alumno_materia.LegajoAlumno " +
                                "INNER JOIN materia_profesor ON alumno_materia.CodigoMateria = materia_profesor.CodigoMateria " +
                                "WHERE materia_profesor.LegajoProfesor = @LegajoProfesor";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.SelectCommand.Parameters.AddWithValue("@LegajoProfesor", LegajoProfesor);
                    adapter.Fill(table);
                    VistaAlumnos.ItemsSource = table.DefaultView;
                }
                connection.Close();
            }
                                       
        }

        public void fillrecursos()
        {
            try
            {
                var selectedRow = (DataRowView)VistaAlumnos.SelectedItem;
                int LegajoAlumno = (int)selectedRow.Row.ItemArray[0];
                string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani;Allow User Variables = True;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                String Query = "SELECT recursoacademico.nombre, recursoacademico.Tipo, recursoacademico.Fecha, nota.Calificacion " +
                    "FROM alumno " +
                    "INNER JOIN nota ON nota.LegajoAlumno = alumno.LegajoAlumno " +
                    "INNER JOIN recursoacademico ON nota.CodRA = recursoacademico.CodRA " +
                    "WHERE alumno.LegajoAlumno = @LegajoAlumno"
                    ;
                connection.Open();
                using (MySqlDataAdapter actualizar = new MySqlDataAdapter(Query, connection))
                {
                    DataTable table = new DataTable();
                    actualizar.SelectCommand.Parameters.AddWithValue("@LegajoAlumno", LegajoAlumno);
                    actualizar.Fill(table);
                    Vistarecursos.ItemsSource = table.DefaultView;
                }
                connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show( ex.ErrorCode.ToString());
                throw;
            }
            
        }

        private void Vistarecursos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fillrecursos();
        }
    }
}
