using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiTicket.Models
{
    public class Ab_Contract
    {
        /*RESUMEN*/
        public string proyecto { get; set; } = "SIN";
        public string codigo { get; set; } = "SINIESTRO";
        public string summary { get; set; } = ".";
        public string ramo { get; set; } = "14904";

        /*SOLICITANTE = CONTACTO*/
        public int tipoDocContac { get; set; }
        public string nroDocContac { get; set; }
        public string nombreContacto { get; set; }
        public string correoContac { get; set; }
        public string celContac { get; set; }
        public string dirContac { get; set; }

        /*AGRAVIADO = ASEGURADO*/
        public int tipoDocAseg { get; set; }
        public string nroDocAseg { get; set; }
        public string nombreAseg { get; set; }
        public string dirAseg { get; set; }
        public string placa { get; set; }
        public DateTime fechaSiniestro { get; set; }
        public DateTime fechaRecepcion { get; set; }
        public int cobertura { get; set; }
        public int tramiteSin { get; set; } = 16022;
        //public string correoAseg { get; set; }

        /*PAGO*/
        //public int metPayment { get; set; }
        public string banco { get; set; }
        public int tipoPago { get; set; }
        public string cuentaDestino { get; set; }
        public string cuentaCCI { get; set; }

        /*ADJUNTO*/
        public List<Ab_Adjunto> adjunto { get; set; }

        /*TICKET*/
        public string Descripcion { get; set; } = ".";
        public string tipo { get; set; } = "11002";
        public string producto { get; set; } = "14996";
    }
}
