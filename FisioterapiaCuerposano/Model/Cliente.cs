using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisioterapiaCuerposano.Model
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Dni { get; set; }
        public int Telefono { get; set; }
        public string FechaNacimiento { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public int PrimeraEntrada { get; set; }
        public string UidToken { get; set; }
        
        public Cliente()
        {
        }

        public Cliente(int idCliente, string nombre, string apellidos, string dni, int telefono, string fechaNacimiento, string correo, string contraseña, int primeraEntrada, string uidToken)
        {
            IdCliente = idCliente;
            Nombre = nombre;
            Apellidos = apellidos;
            Dni = dni;
            Telefono = telefono;
            FechaNacimiento = fechaNacimiento;
            Correo = correo;
            Contraseña = contraseña;
            PrimeraEntrada = primeraEntrada;
            UidToken = uidToken;
        }

        public Cliente(string nombre, string apellidos, string dni, int telefono, string fechaNacimiento, string correo, string contraseña, int primeraEntrada, string uidToken)
        {
            Nombre = nombre;
            Apellidos = apellidos;
            Dni = dni;
            Telefono = telefono;
            FechaNacimiento = fechaNacimiento;
            Correo = correo;
            Contraseña = contraseña;
            PrimeraEntrada = primeraEntrada;
            UidToken = uidToken;
        }

        public string toString()
        {
            return IdCliente + " " + Nombre + " " + Apellidos + " " + UidToken;
        }
    }
}
