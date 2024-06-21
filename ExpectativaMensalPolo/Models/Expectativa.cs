using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpectativaMensalPolo.Models
{
    public class Expectativa
    {
        public string Indicador { get; set; }
        public DateTime Data { get; set; }
        public string DataReferencia { get; set; }
        public double Media { get; set; }
        public double Mediana { get; set; }
        public double DesvioPadrao { get; set; }
        public double Minimo { get; set; }
        public double Maximo { get; set; }
        public int NumeroRespondentes { get; set; }
        public int BaseCalculo { get; set; }
    }
}


