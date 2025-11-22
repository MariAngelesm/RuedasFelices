using System;
using MailKit.Net.Smtp;
using MimeKit;
using RuedasFelices.Models;
using RuedasFelices.Repositories.Interfaces;

namespace RuedasFelices.Services
{
    public class EmailService
    {
        private readonly IEmailLogRepository _emailLogRepository;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _senderEmail;
        private readonly string _senderPassword;
        private const string SenderName = "Ruedas Felices";

        public EmailService(IEmailLogRepository emailLogRepository)
        {
            _emailLogRepository = emailLogRepository;

            // Load credentials from environment variables
            _smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "smtp.gmail.com";
            _smtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
            _senderEmail = Environment.GetEnvironmentVariable("SMTP_EMAIL") ?? "";
            _senderPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "";
        }

        public void SendAppointmentConfirmation(
            Appointment appointment, 
            Customer customer, 
            Vehicle vehicle, 
            Inspector inspector)
        {
            var emailLog = new EmailLog
            {
                AppointmentId = appointment.Id,
                RecipientEmail = customer.Email,
                Subject = "✅ Confirmación de Cita - Ruedas Felices",
                SentAt = DateTime.Now
            };

            try
            {
                // Check if credentials are configured
                if (string.IsNullOrEmpty(_senderEmail) || string.IsNullOrEmpty(_senderPassword))
                {
                    Console.WriteLine("\n⚠️  Warning: Email credentials not configured.");
                    Console.WriteLine("Email simulation: Message would be sent to " + customer.Email);
                    emailLog.WasSent = false;
                    emailLog.ErrorMessage = "Credentials not configured";
                    _emailLogRepository.Add(emailLog);
                    return;
                }

                string htmlBody = BuildHtmlEmailBody(appointment, customer, vehicle, inspector);
                string plainBody = BuildPlainEmailBody(appointment, customer, vehicle, inspector);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(SenderName, _senderEmail));
                message.To.Add(new MailboxAddress(customer.Name, customer.Email));
                message.Subject = emailLog.Subject;

                // Create multipart/alternative message (HTML + Plain text fallback)
                var builder = new BodyBuilder
                {
                    HtmlBody = htmlBody,
                    TextBody = plainBody
                };

                message.Body = builder.ToMessageBody();

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Connect(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    smtpClient.Authenticate(_senderEmail, _senderPassword);
                    smtpClient.Send(message);
                    smtpClient.Disconnect(true);

                    emailLog.WasSent = true;
                    Console.WriteLine($"\n✅ Confirmation email sent to {customer.Email}");
                }
            }
            catch (Exception ex)
            {
                emailLog.WasSent = false;
                emailLog.ErrorMessage = ex.Message;
                Console.WriteLine($"\n❌ Error sending email: {ex.Message}");
            }
            finally
            {
                _emailLogRepository.Add(emailLog);
            }
        }

        private string BuildHtmlEmailBody(
            Appointment appointment, 
            Customer customer, 
            Vehicle vehicle, 
            Inspector inspector)
        {
            string vehicleTypeSpanish = vehicle.Type switch
            {
                VehicleType.Motorcycle => "Motocicleta",
                VehicleType.LightVehicle => "Vehículo Liviano",
                VehicleType.HeavyVehicle => "Vehículo Pesado",
                _ => vehicle.Type.ToString()
            };

            string inspectionTypeSpanish = inspector.InspectionType switch
            {
                InspectionType.Motorcycle => "Motocicletas",
                InspectionType.LightVehicle => "Vehículos Livianos",
                InspectionType.HeavyVehicle => "Vehículos Pesados",
                _ => inspector.InspectionType.ToString()
            };

            string statusSpanish = appointment.Status switch
            {
                AppointmentStatus.Scheduled => "Programada",
                AppointmentStatus.Completed => "Completada",
                AppointmentStatus.Cancelled => "Cancelada",
                _ => appointment.Status.ToString()
            };

            string statusColor = appointment.Status switch
            {
                AppointmentStatus.Scheduled => "#28a745",
                AppointmentStatus.Completed => "#007bff",
                AppointmentStatus.Cancelled => "#dc3545",
                _ => "#6c757d"
            };

            return $@"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Confirmación de Cita - Ruedas Felices</title>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #333;
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
        }}
        .container {{
            max-width: 600px;
            margin: 20px auto;
            background-color: #ffffff;
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        }}
        .header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 30px;
            text-align: center;
        }}
        .header h1 {{
            margin: 0;
            font-size: 28px;
            font-weight: bold;
        }}
        .header p {{
            margin: 5px 0 0 0;
            font-size: 14px;
            opacity: 0.9;
        }}
        .content {{
            padding: 30px;
        }}
        .greeting {{
            font-size: 18px;
            margin-bottom: 20px;
            color: #333;
        }}
        .section {{
            background-color: #f8f9fa;
            border-left: 4px solid #667eea;
            padding: 20px;
            margin: 20px 0;
            border-radius: 5px;
        }}
        .section-title {{
            font-size: 16px;
            font-weight: bold;
            color: #667eea;
            margin: 0 0 15px 0;
            text-transform: uppercase;
            letter-spacing: 1px;
        }}
        .info-row {{
            display: flex;
            padding: 8px 0;
            border-bottom: 1px solid #e0e0e0;
        }}
        .info-row:last-child {{
            border-bottom: none;
        }}
        .info-label {{
            font-weight: 600;
            min-width: 140px;
            color: #555;
        }}
        .info-value {{
            color: #333;
            flex: 1;
        }}
        .appointment-number {{
            display: inline-block;
            background-color: #667eea;
            color: white;
            padding: 8px 16px;
            border-radius: 20px;
            font-weight: bold;
            font-size: 14px;
            margin: 10px 0;
        }}
        .status-badge {{
            display: inline-block;
            padding: 6px 12px;
            border-radius: 15px;
            font-weight: bold;
            font-size: 12px;
            color: white;
            background-color: {statusColor};
        }}
        .highlight-box {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 20px;
            border-radius: 8px;
            text-align: center;
            margin: 20px 0;
        }}
        .highlight-box .date {{
            font-size: 24px;
            font-weight: bold;
            margin: 10px 0;
        }}
        .highlight-box .time {{
            font-size: 32px;
            font-weight: bold;
            margin: 5px 0;
        }}
        .recommendations {{
            background-color: #fff3cd;
            border-left: 4px solid #ffc107;
            padding: 20px;
            margin: 20px 0;
            border-radius: 5px;
        }}
        .recommendations h3 {{
            color: #856404;
            margin: 0 0 15px 0;
            font-size: 16px;
        }}
        .recommendations ul {{
            margin: 0;
            padding-left: 20px;
            color: #856404;
        }}
        .recommendations li {{
            margin: 8px 0;
        }}
        .contact-info {{
            background-color: #e7f3ff;
            border-left: 4px solid #007bff;
            padding: 20px;
            margin: 20px 0;
            border-radius: 5px;
        }}
        .contact-info h3 {{
            color: #004085;
            margin: 0 0 15px 0;
            font-size: 16px;
        }}
        .contact-info p {{
            margin: 8px 0;
            color: #004085;
        }}
        .footer {{
            background-color: #333;
            color: #fff;
            text-align: center;
            padding: 20px;
            font-size: 12px;
        }}
        .footer p {{
            margin: 5px 0;
        }}
        .emoji {{
            font-size: 20px;
        }}
        @media only screen and (max-width: 600px) {{
            .container {{
                margin: 0;
                border-radius: 0;
            }}
            .content {{
                padding: 20px;
            }}
            .info-row {{
                flex-direction: column;
            }}
            .info-label {{
                min-width: auto;
                margin-bottom: 5px;
            }}
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>🚗 RUEDAS FELICES 🚗</h1>
            <p>Centro de Revisión Técnico-Mecánica</p>
        </div>
        
        <div class=""content"">
            <p class=""greeting"">Estimado/a <strong>{customer.Name}</strong>,</p>
            <p>¡Su cita de revisión técnico-mecánica ha sido confirmada exitosamente!</p>
            
            <div class=""highlight-box"">
                <div class=""appointment-number"">Cita #{appointment.Id:D4}</div>
                <div class=""date"">📅 {appointment.AppointmentDate:dddd, dd 'de' MMMM 'de' yyyy}</div>
                <div class=""time"">🕐 {appointment.AppointmentDate:HH:mm}</div>
                <div style=""margin-top: 15px;"">
                    <span class=""status-badge"">{statusSpanish}</span>
                </div>
            </div>

            <div class=""section"">
                <div class=""section-title"">🚘 Información del Vehículo</div>
                <div class=""info-row"">
                    <div class=""info-label"">🔖 Placa:</div>
                    <div class=""info-value""><strong>{vehicle.LicensePlate}</strong></div>
                </div>
                <div class=""info-row"">
                    <div class=""info-label"">🏭 Marca:</div>
                    <div class=""info-value"">{vehicle.Brand}</div>
                </div>
                <div class=""info-row"">
                    <div class=""info-label"">📦 Modelo:</div>
                    <div class=""info-value"">{vehicle.Model}</div>
                </div>
                <div class=""info-row"">
                    <div class=""info-label"">📆 Año:</div>
                    <div class=""info-value"">{vehicle.Year}</div>
                </div>
                <div class=""info-row"">
                    <div class=""info-label"">🚙 Tipo:</div>
                    <div class=""info-value"">{vehicleTypeSpanish}</div>
                </div>
            </div>

            <div class=""section"">
                <div class=""section-title"">👨‍🔧 Inspector Asignado</div>
                <div class=""info-row"">
                    <div class=""info-label"">👤 Nombre:</div>
                    <div class=""info-value""><strong>{inspector.Name}</strong></div>
                </div>
                <div class=""info-row"">
                    <div class=""info-label"">🔧 Especialización:</div>
                    <div class=""info-value"">{inspectionTypeSpanish}</div>
                </div>
                <div class=""info-row"">
                    <div class=""info-label"">📞 Teléfono:</div>
                    <div class=""info-value"">{inspector.Phone}</div>
                </div>
                <div class=""info-row"">
                    <div class=""info-label"">✉️ Email:</div>
                    <div class=""info-value"">{inspector.Email}</div>
                </div>
            </div>

            <div class=""recommendations"">
                <h3>⚠️ Recomendaciones Importantes</h3>
                <ul>
                    <li>Por favor llegue <strong>10 minutos antes</strong> de su hora programada</li>
                    <li>Traiga los <strong>documentos del vehículo</strong> (SOAT, tarjeta de propiedad)</li>
                    <li>Asegúrese de que el vehículo esté <strong>limpio</strong> para la inspección</li>
                    <li>En caso de retraso, <strong>comuníquese con nosotros</strong> inmediatamente</li>
                </ul>
            </div>

            <div class=""contact-info"">
                <h3>📞 Información de Contacto</h3>
                <p>Si necesita reprogramar o cancelar su cita, contáctenos:</p>
                <p><strong>📧 Email:</strong> info@ruedasfelices.com</p>
                <p><strong>📱 Teléfono:</strong> +57 (5) 123-4567</p>
                <p><strong>📍 Dirección:</strong> Carrera 53 #82-90, Barranquilla</p>
            </div>

            <p style=""text-align: center; margin-top: 30px; font-size: 16px;"">
                ✨ <strong>Gracias por confiar en Ruedas Felices</strong> ✨
            </p>
            <p style=""text-align: center; color: #666;"">
                Estamos comprometidos con su seguridad y la de su vehículo.
            </p>
        </div>

        <div class=""footer"">
            <p><strong>Ruedas Felices</strong></p>
            <p>Centro de Revisión Técnico-Mecánica</p>
            <p style=""margin-top: 15px; opacity: 0.8;"">
                Este es un mensaje automático, por favor no responda a este correo.<br>
                Para consultas, utilice nuestros canales oficiales de contacto.
            </p>
        </div>
    </div>
</body>
</html>";
        }

        private string BuildPlainEmailBody(
            Appointment appointment, 
            Customer customer, 
            Vehicle vehicle, 
            Inspector inspector)
        {
            string vehicleTypeSpanish = vehicle.Type switch
            {
                VehicleType.Motorcycle => "Motocicleta",
                VehicleType.LightVehicle => "Vehículo Liviano",
                VehicleType.HeavyVehicle => "Vehículo Pesado",
                _ => vehicle.Type.ToString()
            };

            string inspectionTypeSpanish = inspector.InspectionType switch
            {
                InspectionType.Motorcycle => "Motocicletas",
                InspectionType.LightVehicle => "Vehículos Livianos",
                InspectionType.HeavyVehicle => "Vehículos Pesados",
                _ => inspector.InspectionType.ToString()
            };

            string statusSpanish = appointment.Status switch
            {
                AppointmentStatus.Scheduled => "Programada",
                AppointmentStatus.Completed => "Completada",
                AppointmentStatus.Cancelled => "Cancelada",
                _ => appointment.Status.ToString()
            };

            return $@"
RUEDAS FELICES
Centro de Revisión Técnico-Mecánica
═══════════════════════════════════════════════

Estimado/a {customer.Name},

¡Su cita de revisión técnico-mecánica ha sido confirmada exitosamente!

DETALLES DE LA CITA
───────────────────────────────────────────────
Número de Cita: #{appointment.Id:D4}
Fecha: {appointment.AppointmentDate:dddd, dd 'de' MMMM 'de' yyyy}
Hora: {appointment.AppointmentDate:HH:mm}
Estado: {statusSpanish}

INFORMACIÓN DEL VEHÍCULO
───────────────────────────────────────────────
Placa: {vehicle.LicensePlate}
Marca: {vehicle.Brand}
Modelo: {vehicle.Model}
Año: {vehicle.Year}
Tipo: {vehicleTypeSpanish}

INSPECTOR ASIGNADO
───────────────────────────────────────────────
Nombre: {inspector.Name}
Especialización: {inspectionTypeSpanish}
Teléfono: {inspector.Phone}
Email: {inspector.Email}

RECOMENDACIONES IMPORTANTES
───────────────────────────────────────────────
- Por favor llegue 10 minutos antes de su hora programada
- Traiga los documentos del vehículo (SOAT, tarjeta de propiedad)
- Asegúrese de que el vehículo esté limpio para la inspección
- En caso de retraso, comuníquese con nosotros inmediatamente

INFORMACIÓN DE CONTACTO
───────────────────────────────────────────────
Si necesita reprogramar o cancelar su cita:

Email: info@ruedasfelices.com
Teléfono: +57 (5) 123-4567
Dirección: Carrera 53 #82-90, Barranquilla

═══════════════════════════════════════════════
Gracias por confiar en Ruedas Felices
Estamos comprometidos con su seguridad y la de su vehículo.

Atentamente,
El equipo de Ruedas Felices

Este es un mensaje automático, por favor no responda a este correo.
Para consultas, utilice nuestros canales oficiales de contacto.
";
        }
    }
}