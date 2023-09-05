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
    /// Lógica de interacción para Existente.xaml
    /// </summary>
    public partial class Existente : Page
    {
        public Existente()
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
            var window = (MainWindow)Window.GetWindow(this);
            int LegajoAlumno = window.alumno.NumeroLegajo;
            string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani;Allow User Variables = True;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT materia.Nombre, notificacion.* " +
                    "FROM alumno " +
                    "INNER JOIN alumno_materia ON alumno.LegajoAlumno = alumno_materia.LegajoAlumno " +
                    "INNER JOIN materia ON alumno_materia.CodigoMateria = materia.CodigoMateria " +
                    "INNER JOIN materia_notificacion ON materia.CodigoMateria = materia_notificacion.CodigoMateria " +
                    "INNER JOIN notificacion ON materia_notificacion.IdNotificacion = notificacion.IdNotificacion " +
                    "WHERE alumno.LegajoAlumno = @LegajoAlumno";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable table = new DataTable();
                    adapter.SelectCommand.Parameters.AddWithValue("@LegajoAlumno", LegajoAlumno);
                    adapter.Fill(table);
                    VistaDatos.ItemsSource = table.DefaultView;
                }
                connection.Close();
            }
        }
        private void EliminarNotificacion_Click(object sender, RoutedEventArgs e)
        {
            EliminarGroupBox.IsEnabled = true;
            ModificarGroupBox.IsEnabled = false;
        }

        private void GuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            if (VistaDatos.SelectedItem is DataRowView row)
            {
                int IDNotificacion = (int)row[1];
                String Tipo = parseStringCheckBoxes();
                String Fecha = FechaPicker.Text;
                string query = "UPDATE notificacion SET Tipo = @Tipo , Fecha = @Fecha WHERE IDNotificacion = @IDNotificacion";
                string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {

                            command.Parameters.AddWithValue("@Tipo", Tipo);
                            command.Parameters.AddWithValue("@Fecha", Fecha);
                            command.Parameters.AddWithValue("@IDNotificacion", IDNotificacion);
                            
                            if (command.ExecuteNonQuery() > 0)
                            {
                                MessageBoxResult result = MessageBox.Show("Cambios Guardados", "Operacion Correcta", MessageBoxButton.OK);
                            }

                        }
                        connection.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void UpdateCheckBoxes(string text)
        {
            string[] words = text.Split(',');
            
            TrabajoPracticoCheckBox.IsChecked = words.Contains("Trabajo Practico");
            ParcialCheckBox.IsChecked = words.Contains("Parcial");
            TallerCheckBox.IsChecked = words.Contains("Taller");
            FinalCheckBox.IsChecked = words.Contains("Final");

        }

        private String parseStringCheckBoxes()
        {
            String Tipo = "";
            
            List<CheckBox> checkBoxes= new List<CheckBox>() { TrabajoPracticoCheckBox, ParcialCheckBox, TallerCheckBox, FinalCheckBox };
            
            foreach (CheckBox checkBox in checkBoxes)
            {
                if(checkBox.IsChecked == true)
                {
                    Tipo += checkBox.Content + ", ";
                }
            }

            return Tipo;

        }
        private void ModificarNotificacion_Click(object sender, RoutedEventArgs e)
        {
            EliminarGroupBox.IsEnabled = false;
            ModificarGroupBox.IsEnabled = true;
            var selectedRow = (DataRowView)VistaDatos.SelectedItem;
            UpdateCheckBoxes((String)selectedRow.Row.ItemArray[2]);
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de que deseas continuar?", "Confirmación", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var selectedRow = (DataRowView)VistaDatos.SelectedItem;
                int IDNotificacion  = (int)selectedRow.Row.ItemArray[1];
                string query = "DELETE FROM notificacion WHERE IDNotificacion = @IDNotificacion";
                string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteMateriaNotificacionQuery = "DELETE FROM materia_notificacion WHERE IDNotificacion = @IDNotificacion";
                    using (MySqlCommand deleteMateriaNotificacionCommand = new MySqlCommand(deleteMateriaNotificacionQuery, connection))
                    {
                        deleteMateriaNotificacionCommand.Parameters.AddWithValue("@IDNotificacion", IDNotificacion);
                        deleteMateriaNotificacionCommand.ExecuteNonQuery();
                    }
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {

                        try
                        {

                            command.Parameters.AddWithValue("@IDNotificacion", IDNotificacion);                            
                            command.ExecuteNonQuery();                              
                            fillData();                            

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                    connection.Close();
                }
            }
            
        }
    }
    public class StringToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateString)
            {
                int lastCommaIndex = dateString.LastIndexOf(',');
                if (lastCommaIndex >= 0 && lastCommaIndex < dateString.Length - 1)
                {
                    dateString = dateString.Substring(lastCommaIndex + 1).Trim();
                }

                if (DateTime.TryParse(dateString, out DateTime date))
                {
                    return date;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

