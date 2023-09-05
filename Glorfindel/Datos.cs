using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glorfindel
{
    class Datos
    {
    }
    public class DatosAlumno
    {
        public int NumeroLegajo { get; set; }
        public String Nombre { get; set; }
        public String Apellido { get; set; }        
        public String Carrera { get; set; }
    }
    public class DatosProfesor
    {
        public int NumeroLegajoProfesor { get; set; }
        public String Nombre { get; set; }
        public String Apellido { get; set; }
        public String Titulo { get; set; }
    }
    class DatosEstadistica
    {
        public int IDEstadistica { get; set; }
        public String Fecha { get; set; }
        public String Tipo { get; set; }
    }
    class DatosMateria
    {
        public int codMateria { get; set; }
        public String Nombre { get; set; }
        public String FechaInicio { get; set; }
        public String Tipo { get; set; }

    }
    class RecursoAcademico
    {
        public int codRecurso { get; set; }
        public object nota { get; set; }
        public String tipo { get; set; }
        public String nombre { get; set; }
        public String fecha { get; set; }

        public int CodMateria { get; set; }

    }


    class Notificacion
    {
        public int idNotificacion { get; set; }
        public int tipo { get; set; }
        public String Fecha { get; set; }
    }
}

