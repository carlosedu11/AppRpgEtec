using System;
using System.Collections.Generic;
using System.Text;

namespace AppRpgEtec.Models
{
    public class Email
    {       
        public string Remetente { get; set; }
        public string RemententePassword { get; set; }
        public string Destinatario { get; set; }
        public string DestinatarioCopia { get; set; }
        public string DominioPrimario { get; set; }
        public int PortaPrimaria { get; set; }
        public string Assunto { get; set; }
        public string Mensagem { get; set; }
    }
}


