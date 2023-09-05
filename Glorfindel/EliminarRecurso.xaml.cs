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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Glorfindel
{
    /// <summary>
    /// Lógica de interacción para EliminarRecurso.xaml
    /// </summary>
    public partial class EliminarRecurso : Page
    {
        public EliminarRecurso()
        {
            InitializeComponent();
            fillData();
        }

        private void BotonEliminar_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = (DataRowView)VistaDatos.SelectedItem;
            int CodRA = (int)selectedRow.Row.ItemArray[0];
            string query = "DELETE FROM recursoacademico WHERE CodRA = @CodRA";
            string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using(MySqlCommand command = new MySqlCommand(query, connection))
                {

                    try
                    {
                    command.Parameters.AddWithValue("@CodRA", CodRA);

                        
                        MessageBoxResult result = MessageBox.Show("¿Desea eliminar el recurso seleccionado ?" + "\n Nombre del recurso academico: " + selectedRow.Row.ItemArray[1] + " \nTipo de recurso academico: "+ selectedRow.Row.ItemArray[2] + "\nFecha: " + selectedRow.Row.ItemArray[4], "Confirmación", MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.Yes)
                        {                      
                            command.ExecuteNonQuery();
                            MessageBoxResult Mensaje = MessageBox.Show("Eliminación Correcta","Resultado",MessageBoxButton.OK);
                            fillData();
                        } 
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                connection.Close();
            }
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

        private void ModificarRecurso_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EliminarRecurso_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GuardarCambios_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
