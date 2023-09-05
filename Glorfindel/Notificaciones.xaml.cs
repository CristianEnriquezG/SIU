using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Glorfindel
{
    /// <summary>
    /// Lógica de interacción para Notificaciones.xaml
    /// </summary>
    public partial class Notificaciones : Page
    {
        public Notificaciones()
        {
            InitializeComponent();
            this.Loaded += Pagina_Loaded;

        }

        private void CrearButton_Click(object sender, RoutedEventArgs e)
        {
            var window = (MainWindow)Window.GetWindow(this);
            int LegajoAlumno = window.alumno.NumeroLegajo;
            String tipo = "";

            List<CheckBox> listaCheckbox = new List<CheckBox> { ParcialCheck, TrabajosCheck, TalleresCheck, FinalCheck };
            if (TodoCheck.IsChecked == true)
            {
                tipo = TrabajosCheck.Content.ToString()+","+ ParcialCheck.Content.ToString()+","+ TalleresCheck.Content.ToString()+ "," + FinalCheck.Content.ToString();
            }
            else
            {
                foreach (CheckBox checkbox in listaCheckbox)
                {
                    switch (checkbox.IsChecked)
                    {
                        case true:
                            tipo += checkbox.Content + ", ";
                            break;
                        default:
                            break;
                    }
                }
            }
            
            String intervalo = "";

            List<RadioButton> radioButtons= new List<RadioButton> { TodosDias, SieteDias, catorceDias };
            foreach (RadioButton radioButton in radioButtons)
            {
                switch (radioButton.IsChecked)
                {
                    case true:
                        intervalo = radioButton.Content.ToString();
                        break;
                    default:
                        break;
                }
                if (TodosDias.IsChecked == true)
                {
                    intervalo += ", " + HoraBox.Value.ToString();
                    break;
                }
            }
            DataRowView elementoSeleccionado = (DataRowView)VistaDatos.SelectedItem;
            
            int codMateria = (int)elementoSeleccionado.Row[0];
      
            
            string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani;Allow User Variables = True;";
            
            using (MySqlConnection connection = new MySqlConnection(connectionString))

            {
                connection.Open();
                try
                {
                    string query = "INSERT INTO notificacion (Tipo,Fecha)" +
                        " VALUES (@Tipo,@intervalo)";
                    using (MySqlCommand cmd = new MySqlCommand(query,connection))
                    {
                        cmd.Parameters.AddWithValue("@Tipo",tipo);
                        cmd.Parameters.AddWithValue("@intervalo", intervalo);
                        
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            int ultimoagregado = (int)cmd.LastInsertedId;
                            query = "INSERT INTO materia_notificacion (CodigoMateria, IdNotificacion)" +
                            "Values (@codMateria,@ultimoagregado)";
                            using (MySqlCommand cmd2 = new MySqlCommand(query, connection))
                            {
                                cmd2.Parameters.AddWithValue("@codMateria", codMateria);
                                cmd2.Parameters.AddWithValue("@ultimoagregado", ultimoagregado);
                                if (cmd2.ExecuteNonQuery() > 0)
                                {
                                    MessageBoxResult result = MessageBox.Show("Notificacion Creada", "Operacion Correcta", MessageBoxButton.OK);
                                }
                            }
                        }
                        
                    }

                

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                connection.Close();

            }
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

                string query = "SELECT materia.CodigoMateria, materia.Nombre FROM alumno INNER " +
                    "JOIN alumno_materia " +
                    "ON alumno.LegajoAlumno = alumno_materia.LegajoAlumno " +
                    "INNER JOIN materia ON alumno_materia.CodigoMateria = materia.CodigoMateria " +
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ParcialCheck.IsChecked= true;
            TrabajosCheck.IsChecked= true;
            TalleresCheck.IsChecked= true;
            FinalCheck.IsChecked= true;

        }

        private void TodoCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            ParcialCheck.IsChecked = false;
            TrabajosCheck.IsChecked = false;
            TalleresCheck.IsChecked = false;
            FinalCheck.IsChecked = false;
        }
    }
}
