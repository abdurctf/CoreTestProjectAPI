using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Utilities.Common
{
    public class LSEmailLibrary
    {
        private static string _ERRMSG = string.Empty;
        public static string SendMail(string ToDisplayName, string ToAdr, string FromDisplayName, string FromAdr, string CcDisplayName, string CcAdr, string BccAdr, string Subject, string BodyText, string AttachmentFileName, string Smtp_IP_Address, int? SmtpPort, string Frompassword)
        {
            MailMessage mes = new MailMessage();
            char[] sepr = new char[] { ',', ';' };
            string _fromadd = "";
            string _fromdisp = "";
            string[] _toadd = new string[50];
            string[] _todisp = new string[50];
            string[] _ccadd = new string[50];
            string[] _ccdisp = new string[50];
            string[] _bccadd = new string[50];
            string[] _bccdisp = new string[50];
            string[] _attachFile = new string[50];

            #region FROM
            if (!String.IsNullOrEmpty(FromAdr))
                _fromadd = FromAdr;
            if (!String.IsNullOrEmpty(FromDisplayName))
                _fromdisp = FromDisplayName;
            mes.From = new MailAddress(_fromadd, _fromdisp);
            #endregion

            #region TO
            if (!String.IsNullOrEmpty(ToAdr))
            {
                _toadd = ToAdr.Split(sepr);
            }
            else
            {
                return "Mail reciever cannot be empty!!!";
            }
            if (!String.IsNullOrEmpty(ToDisplayName))
            {
                _todisp = ToDisplayName.Split(sepr);
            }
            for (int i = 0; i < _toadd.Length; i++)
            {
                if (_todisp.Length > i)
                {
                    if (String.IsNullOrEmpty(_toadd[i]))
                        continue;
                    MailAddress to_i = new MailAddress(_toadd[i], _todisp[i]);
                    mes.To.Add(to_i);
                }
                else
                {
                    if (!String.IsNullOrEmpty(_toadd[i]))
                        mes.To.Add(new MailAddress(_toadd[i]));
                }
            }

            #endregion

            #region CC
            if (!String.IsNullOrEmpty(CcAdr))
            {
                _ccadd = CcAdr.Split(sepr);
            }
            if (!String.IsNullOrEmpty(CcDisplayName))
            {
                _ccdisp = CcDisplayName.Split(sepr);
            }
            else
            {
                _ccdisp = new string[0]; ;
            }

            for (int i = 0; i < _ccadd.Length; i++)
            {
                if (_ccdisp.Length > i)
                {
                    if (String.IsNullOrEmpty(_ccadd[i]))
                        continue;

                    MailAddress cc_i = new MailAddress(_toadd[i], _ccdisp[i]);
                    mes.CC.Add(cc_i);
                }
                else
                {
                    if (!String.IsNullOrEmpty(_ccadd[i]))
                        mes.CC.Add(new MailAddress(_ccadd[i]));
                }
            }

            #endregion

            #region BCC
            if (!String.IsNullOrEmpty(BccAdr))
            {
                _bccadd = BccAdr.Split(sepr);
            }

            for (int i = 0; i < _bccadd.Length; i++)
            {
                if (!String.IsNullOrEmpty(_bccadd[i]))
                    mes.Bcc.Add(new MailAddress(_bccadd[i]));

            }
            #endregion

            #region Subject & Body
            mes.Subject = Subject;
            mes.IsBodyHtml = true;
            mes.Body = BodyText;
            mes.BodyEncoding = System.Text.Encoding.UTF8;

            #endregion

            #region Atachment
            if (!String.IsNullOrEmpty(AttachmentFileName))
            {
                _attachFile = AttachmentFileName.Split(sepr);
            }
            for (int i = 0; i < _attachFile.Length; i++)
            {
                if (!String.IsNullOrEmpty(_attachFile[i]))
                    mes.Attachments.Add(new Attachment(_attachFile[i]));

            }
            #endregion

            string LS_MailServerIP = Smtp_IP_Address.ToString().Trim();

            //SmtpClient smtpClient = new SmtpClient(LS_MailServerIP);
            //smtpClient.SendCompleted += new SendCompletedEventHandler(MailDeliveryComplete);

            //if (SmtpPort != null)
            //    smtpClient.Port = Convert.ToInt16(SmtpPort);

            ////smtpClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
            try
            {
                //smtpClient.SendAsync(mes, "Sending Mail");



                using (MailMessage message = mes)
                {
                    //message.IsBodyHtml = true;
                    SmtpClient client = null;
                    //client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    if (LS_MailServerIP.Contains("365"))
                    {
                        client = new SmtpClient(LS_MailServerIP, 587);
                        client.EnableSsl = true;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.Credentials = new NetworkCredential(FromAdr, Frompassword);
                    }
                    else
                    {
                        client = new SmtpClient(LS_MailServerIP);
                    }
                    int temp = ServicePointManager.MaxServicePointIdleTime; //<- Store the original value.
                    ServicePointManager.MaxServicePointIdleTime = 1; //<- Change the idle time to 1.

                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception ex)
                    {
                        //_ERRMSG = ex.ToString();
                        _ERRMSG = (ex.InnerException != null) ? ex.GetBaseException().Message : ex.Message;
                    }
                    finally
                    {
                        ServicePointManager.MaxServicePointIdleTime = temp; //<- Set the idle time back to what it was.
                    }
                }
                return _ERRMSG;
            }
            catch (Exception ex)
            {
                var errmes = "Sending Mail Error: " + "\nErrormessage:" + ex.Message + "\nSource:" + ex.Source + "\nStack:" + ex.StackTrace + "\nInnerMessage:" + ex.InnerException != null ? ex.InnerException.Message : "No inner exception exist";
                return errmes;
            }
        }
    }
}
