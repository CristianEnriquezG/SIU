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
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
using MySqlConnector;
using System.Runtime.Intrinsics.Arm;

namespace Glorfindel
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            

        }

        private void Ingresar_Click(object sender, RoutedEventArgs e)
        {
            String tipo = determinar_Tipo();
            switch (tipo)
            {
                case "alumno":
                    {
                        if (contraseñaAlumno())
                        {
                            this.Hide();
                            DatosAlumno alumno = leerAlumno();
                            Window actual = new MainWindow(alumno);
                            actual.Show();
                        }
                        else
                        {
                            MessageBoxResult Error = MessageBox.Show("Datos Incorrectos", "Datos Incorrectos", MessageBoxButton.OK);
                        }
                    }
                    break;
                case "profesor":
                    {
                        if (contraseñaProfesor())
                        {
                            this.Hide();
                            DatosProfesor profesor = leerProfesor();
                            Window actual = new HomeProfesor(profesor);
                            actual.Show();
                        }
                        else
                        {
                            MessageBoxResult Error = MessageBox.Show("Datos Incorrectos", "Datos Incorrectos", MessageBoxButton.OK);
                        }
                    }
                    break;
            }
        }

        private bool contraseñaAlumno()
        {
            bool match = false;
            int contador = 0;
            String sha = generarSha(Contraseña.Password.ToCharArray());
            string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani";
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {   
                connection.Open();
                string query = @"SELECT COUNT(*) FROM alumno WHERE alumno.contraseña = @sha";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@sha", sha);

                    contador = Convert.ToInt32(cmd.ExecuteScalar());
                    if(contador > 0)
                    {
                        match=true;
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            return match;
        }

        private bool contraseñaProfesor()
        {
            bool match = false;
            int contador = 0;
            String sha = generarSha(Contraseña.Password.ToCharArray());
            string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani";
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = @"SELECT COUNT(*) FROM profesor WHERE profesor.contraseña = @sha";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@sha", sha);

                    contador = Convert.ToInt32(cmd.ExecuteScalar());
                    if (contador > 0)
                    {
                        match = true;
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            return match;
        }

        private String determinar_Tipo()
        {
            String tipo = "";
            
            String Usuario = UsuarioBox.Text;

            string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani;Allow User Variables = true;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            int contador;
            try
            {
                connection.Open();

                string query = @"SELECT * FROM alumno WHERE UsuarioAlumno = @Usuario";                


                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {                    
                     cmd.Parameters.AddWithValue("@Usuario", Usuario);                                                   
                     contador = Convert.ToInt32(cmd.ExecuteScalar());

                    if(contador > 0) 
                    {
                        tipo = "alumno";
                                
                    }
                    else
                    {
                        string UsuarioProfesor = Usuario;
                        query = @"SELECT COUNT(*) FROM profesor WHERE UsuarioProfesor = @UsuarioProfesor";
                        using (MySqlCommand consulta = new MySqlCommand(query, connection))
                        {
                            consulta.Parameters.AddWithValue("@UsuarioProfesor", UsuarioProfesor);
                            contador = Convert.ToInt32(consulta.ExecuteScalar());
                        }

                        if (contador > 0)
                        {
                            tipo = "profesor";
                        }
                    }                                           
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }

            return tipo;
        }

        private DatosAlumno leerAlumno()
        {
            String usuario = UsuarioBox.Text;
            DatosAlumno alumno= new DatosAlumno();
            string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani";
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                String query = @"SELECT * FROM alumno WHERE usuarioAlumno = @usuario";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    using(MySqlDataReader lector = cmd.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            
                            alumno.NumeroLegajo = int.Parse(lector["LegajoAlumno"].ToString());
                            alumno.Nombre = lector["Nombre"].ToString();
                            alumno.Apellido = lector["Apellido"].ToString();
                            alumno.Carrera = lector["Carrera"].ToString();
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
            return alumno;
        }
        private DatosProfesor leerProfesor() 
        {
            String usuario = UsuarioBox.Text;
            DatosProfesor profesor = new DatosProfesor();
            string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani";
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                String query = @"SELECT * FROM profesor WHERE usuarioProfesor = @usuario";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    using (MySqlDataReader lector = cmd.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            if (int.TryParse(lector["LegajoProfesor"].ToString(), out int LegajoProfesor))
                            {
                                profesor.NumeroLegajoProfesor= LegajoProfesor;
                            }
                            profesor.Nombre = lector["Nombre"].ToString();
                            profesor.Apellido = lector["Apellido"].ToString();
                            
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            return profesor;
        }    
        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private static string generarSha(char[] texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(texto);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();

                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
            
        }
       
        private bool chequearContraseña(String sha, String tipo)
        {                        

            bool coincidencia = false;
            string connectionString = "server=localhost;uid=GuaraniG;password=159357;database=guarani";
            MySqlConnection connection = new MySqlConnection(connectionString);
            
            try
            {
                connection.Open();

                String query = "SELECT COUNT(*) FROM { tipo } WHERE contraseña = @sha";
                using(MySqlCommand cmd = new MySqlCommand(query, connection)) 
                {
                    
                    cmd.Parameters.AddWithValue("@sha", sha);
                    
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        coincidencia = true;
                    }
                }

                connection.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return coincidencia;
        }
        
    }
}
