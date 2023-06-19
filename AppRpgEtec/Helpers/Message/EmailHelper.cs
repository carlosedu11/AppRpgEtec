using AppRpgEtec.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.Helpers.Message
{
    public class EmailHelper
    {
        public async Task EnviarEmail(Models.Email email)
        {
            try
            {
                string toEmail = email.Destinatario;

                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(email.Remetente, "App Rpg Etec")
                };

                mailMessage.To.Add(new MailAddress(toEmail));

                if (!string.IsNullOrEmpty(email.DestinatarioCopia))
                    mailMessage.CC.Add(new MailAddress(email.DestinatarioCopia));

                mailMessage.Subject = "App RPG Etec - " + email.Assunto;
                mailMessage.Body = GerarCorpoEmail(email.Mensagem);
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;

                //Outras opções --> Anexo:
                //byte[] arquivoAnexo = null; string contenType = "application/pdf"; //image/png ou //image/jpeg
                //mailMessage.Attachments.Add(new Attachment(new MemoryStream(arquivoAnexo), "nomeArquivo", contenType));

                using (SmtpClient smtp = new SmtpClient(email.DominioPrimario, email.PortaPrimaria))
                {
                    smtp.Credentials = new NetworkCredential(email.Remetente, email.RemententePassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GerarCorpoEmail(string mensagem)
        {
            try
            {
                String sb =
                "<html>"
                + "<body>"
                + "    <div style='text-align:center'>"
                + "        <div style='text-align:left'>"
                + "        <table style='width: 600px; border:1px solid #0089cf;' border='0' cellspacing='0' cellpadding='0'>"
                + "        <tbody>"
                + "            <tr style='background- color: #0089cf;'>"
                + "                <td>"
                + "                    <table style='width: 100%;' border='0' cellspacing='0' cellpadding='0'>"
                + "                        <tbody>"
                + "                            <tr>"
                + "                                <td style='width: 224px;'><img src='https://etechoracio.com.br/imagens/logo_selo_positivo.png' alt='Etec Professor Horacio Augusto da Silveira' width='200px' height='200px' /></td>"
                + "                                <td style='font-family: Arial; font-size: 40px; color: #fff; text-align: center;'> Etec HAS </td>"
                + "                            </tr>"
                + "                        </tbody>"
                + "                    </table>"
                + "                </td>"
                + "            </tr>"
                + "            <tr  "
                + "                <td style='padding:5px; height: 400px; font-size: 1.2rem; line-height: 1.467; font-family: ''''Segoe UI'''',''''Segoe WP'''',Arial,Sans-Serif; color: #333; vertical-align:top'>"
                + mensagem
                + "                </td>"
                + "            </tr>"
                + "            <tr style='background- color: #0089cf; color: #fff;'>"
                + "                <td style='padding:5px'>"
                + "                 Etec Professor Horácio Augusto da Silveira <br />"
                + "                 Curso Técnico em Desenvolvimento de Sistemas <br />"
                + "                 Rua Alcântara, 113 - Vila Guilherme <br/> São Paulo/SP CEP: 02110-010 "
                + "                </td>"
                + "            </tr>"
                + "        </tbody>"
                + "    </table>"
                + "</div>"
                + "</div>"
                + "</body>"
                + "</html>";

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



    }
}
