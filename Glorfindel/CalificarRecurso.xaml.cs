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
using System.IO;
using System.Windows.Automation.Provider;
using System.Globalization;
using MySqlConnector;
using System.Data;


namespace Glorfindel
{
    /// <summary>
    /// Lógica de interacción para CalificarRecurso.xaml
    /// </summary>
    public partial class CalificarRecurso : Page
    {
        public CalificarRecurso()
        {
            InitializeComponent();
            this.Loaded += CalificarRecurso_Loaded;
            
        }

        private void CalificarRecurso_Loaded(object sender, RoutedEventArgs e)
        {
            fillData();
            fillalumnos();
        }

        public void fillData()
        {
            string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT * FROM recursoacademico";
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            VistaDatos.ItemsSource = table.DefaultView;
            
            connection.Close();
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
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT materia.Nombre " +
                                "FROM materia " +
                                "INNER JOIN materia_profesor ON materia.CodigoMateria = materia_profesor.CodigoMateria " +
                                "WHERE materia_profesor.LegajoProfesor = @LegajoProfesor";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LegajoProfesor", LegajoProfesor);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Materia.Content = reader.GetString(0);
                        }
                    }
                }
                connection.Close();
            }
        }


        private void GuardarCambios_Click(object sender, RoutedEventArgs e)
        {

            if (VistaAlumnos.SelectedItem is DataRowView alumnoRow && VistaDatos.SelectedItem is DataRowView recursoRow)
            {
                int LegajoAlumno = (int)alumnoRow["LegajoAlumno"];
                int CodRA = (int)recursoRow["CodRA"];
                string Calificacion = NotaBox.Text;

                string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO nota (LegajoAlumno, CodRA, Calificacion) VALUES (@LegajoAlumno, @CodRA, @Calificacion) " +
                                   "ON DUPLICATE KEY UPDATE Calificacion = @Calificacion";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LegajoAlumno", LegajoAlumno);
                        command.Parameters.AddWithValue("@CodRA", CodRA);
                        command.Parameters.AddWithValue("@Calificacion", Calificacion);
                        if (command.ExecuteNonQuery() > 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Recurso Calificado", "Operacion Correcta", MessageBoxButton.OK);
                        }

                    }
                    connection.Close();

                }
            }
        }  
    }
}
