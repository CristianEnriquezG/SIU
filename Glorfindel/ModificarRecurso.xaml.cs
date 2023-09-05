using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    /// Lógica de interacción para ModificarRecurso.xaml
    /// </summary>
    public partial class ModificarRecurso : Page
    {
        public ModificarRecurso()
        {
            InitializeComponent();
            this.Loaded += Pagina_Loaded;
        }
        private void Pagina_Loaded(object sender, RoutedEventArgs e)
        {
            fillData();
        }
        public void fillData()
        {
            var window = (HomeProfesor)Window.GetWindow(this);
            int LegajoProfesor = window.profesor.NumeroLegajoProfesor;
            string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani;Allow User Variables = True;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT recursoacademico.* " +
                                    "FROM recursoacademico " +
                                    "INNER JOIN materia ON recursoacademico.CodigoMateria = materia.CodigoMateria " +
                                    "INNER JOIN materia_profesor ON materia.CodigoMateria = materia_profesor.CodigoMateria " +
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
        }
        private void EliminarRecurso_Click(object sender, RoutedEventArgs e)
        {
            EliminarGroupBox.IsEnabled = true;
            ModificarGroupBox.IsEnabled = false;
        }

        private void GuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            if (VistaDatos.SelectedItem is DataRowView row)
            {
                int CodRA = (int)row[0];                
                String Fecha = FechaPicker.Text;
                string query = "UPDATE recursoacademico SET Fecha = @Fecha WHERE CodRA = @CodRA";
                string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {                            
                            command.Parameters.AddWithValue("@Fecha", Fecha);
                            command.Parameters.AddWithValue("@CodRA", CodRA);

                            if (command.ExecuteNonQuery() > 0)
                            {
                                MessageBoxResult result = MessageBox.Show("Cambios Guardados", "Operacion Correcta", MessageBoxButton.OK);
                            }

                        }
                        connection.Close();
                        fillData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }


       
        private void ModificarRecurso_Click(object sender, RoutedEventArgs e)
        {
            EliminarGroupBox.IsEnabled = false;
            ModificarGroupBox.IsEnabled = true;
            var selectedRow = (DataRowView)VistaDatos.SelectedItem;
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de que deseas continuar?", "Confirmación", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var selectedRow = (DataRowView)VistaDatos.SelectedItem;
                    int CodRA = (int)selectedRow.Row.ItemArray[0];
                    string query = "DELETE FROM recursoacademico WHERE CodRA = @CodRA";
                    string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani;";
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        using (MySqlCommand deleteCommand = new MySqlCommand(query, connection))
                        {
                            deleteCommand.Parameters.AddWithValue("@CodRA", CodRA);
                            deleteCommand.ExecuteNonQuery();
                            fillData();
                        }
                        connection.Close();
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                
                    
                
            }

        }
    }
    

}
