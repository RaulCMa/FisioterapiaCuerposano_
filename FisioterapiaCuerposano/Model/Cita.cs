using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisioterapiaCuerposano.Model
{
    public class Cita : INotifyPropertyChanged
    {
        public int IdCita { get; set; }

        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public int Aceptada { get; set; }
        public int Realizada { get; set; }

        //public int _aceptada;
        //public int Aceptada 
        //{
        //    get { return this._aceptada; }
        //    set
        //    {
        //        if (this._aceptada != value)
        //        {
        //            this._aceptada = value;
        //            this.NotifyPropertyChanged("Aceptada");
        //        }
        //    }
        //}
        //public int _realizada;
        //public int Realizada
        //{
        //    get { return this._realizada; }
        //    set
        //    {
        //        if (this._realizada != value)
        //        {
        //            this._realizada = value;
        //            this.NotifyPropertyChanged("Realizada");
        //        }
        //    }
        //}

        public Cita()
        {
        }

        public Cita(int idCita, int idCliente, DateTime fecha, int aceptada, int realizada)
        {
            IdCita = idCita;
            IdCliente = idCliente;
            Fecha = fecha;
            Aceptada = aceptada;
            Realizada = realizada;
        }
        public Cita(int idCliente, DateTime fecha, int aceptada, int realizada)
        {
            IdCliente = idCliente;
            Fecha = fecha;
            Aceptada = aceptada;
            Realizada = realizada;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string toString()
        {
            return $"IdCita: {IdCita}, IdCliente: {IdCliente}, Fecha: {Fecha}, Aceptada: {Aceptada}, Realizada: {Realizada}";
        }
    }
}
