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
using MySqlConnector;

namespace Glorfindel
{
    /// <summary>
    /// Lógica de interacción para CrearRecurso.xaml
    /// </summary>
    public partial class CrearRecurso : Page
    {
        public CrearRecurso()
        {
            InitializeComponent();
        }

        private void CrearR_Click(object sender, RoutedEventArgs e)
        {
            
            Random ran = new Random();
            RecursoAcademico dato = new RecursoAcademico();
            if (int.TryParse(CodmateriaBox.Text, out int codMateria))
            {
                dato.CodMateria = codMateria;
            }
            dato.codRecurso = ran.Next(0, 101);

            if ((bool)Trabajo_Practico.IsChecked)
            {
                dato.tipo = "Trabajo Practico";
            }
            else
                if ((bool)Parcial.IsChecked)
                {
                dato.tipo = "Parcial";
                }
                else
                    if((bool)Taller.IsChecked)
            {
                dato.tipo = "Taller";
            }
            dato.nombre = NombreRecurso.Text;
            dato.fecha = datePicker.SelectedDate.ToString();
            try
            {
            string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "INSERT INTO recursoacademico (CodRA, nombre, Tipo, Fecha, CodigoMateria) VALUES (@CodRA, @nombre ,@Tipo, @fecha ,@CodigoMateria)";
            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@CodRA", dato.codRecurso);
            command.Parameters.AddWithValue("@nombre", dato.nombre);
            command.Parameters.AddWithValue("@Tipo", dato.tipo);
            command.Parameters.AddWithValue("@Fecha", dato.fecha);
            command.Parameters.AddWithValue("@CodigoMateria", dato.CodMateria);
            command.ExecuteNonQuery();
            connection.Close();
            MessageBoxResult resultado = MessageBox.Show("Recurso Academico Creado", "Exito", MessageBoxButton.OK);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

            

        }
    }
}
