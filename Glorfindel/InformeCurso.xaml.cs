using MySqlConnector;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
namespace Glorfindel
{
    /// <summary>
    /// Lógica de interacción para Informes.xaml
    /// </summary>
    public partial class InformesCurso : Page
    {
        public InformesCurso()
        {
            InitializeComponent();
            this.Loaded += InformesCurso_Loaded;
        }

        private void InformesCurso_Loaded(object sender, RoutedEventArgs e)
        {
            crearGraficoTorta();
            //  filldata();

        }

        /*private void filldata()
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
                    VistaDatos.ItemsSource = table.DefaultView;
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
                            Materia.Text = reader.GetString(0);
                        }
                    }
                }
                connection.Close();
            }
        }*/

        public void crearGraficoTorta()
        {
            string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani;Allow User Variables = True;";
            string sql = "SELECT COUNT(*) FROM nota WHERE Calificacion > 50";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand(sql, connection);
            int cantidadNotasSuperioresA50 = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();

            sql = "SELECT COUNT(*) FROM nota";
            connection.Open();
            command = new MySqlCommand(sql, connection);
            int cantidadTotalNotas = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();

            int cantidadNotasInferioresA50 = cantidadTotalNotas - cantidadNotasSuperioresA50;

            var model = new PlotModel { Title = "Porcentaje de Aprobados" };
            var series = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0
            };
            series.Slices.Add(new PieSlice("Aprobados", cantidadNotasSuperioresA50) { IsExploded = true });
            series.Slices.Add(new PieSlice("Desaprobados", cantidadNotasInferioresA50));
            model.Series.Add(series);
            plotView1.Model = model;
        }

        private void BuscarCurso_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
