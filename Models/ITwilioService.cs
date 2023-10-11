using Twilio;

namespace AppClientesMexaba.Models
{
    public interface ITwilioService
    {
        void EnviarMensaje(string numeroCelular, string mensaje);
    }

    public class TwilioService : ITwilioService
    {
        private readonly TwilioClient _twilioClient;

        public TwilioService(string accountSid, string authToken)
        {
            TwilioClient.Init(accountSid, authToken);
        }

        public void EnviarMensaje(string numeroCelular, string mensaje)
        {
            // Lógica para enviar un mensaje usando Twilio
            //var mensaje = MessageResource.Create(
              //  body: mensaje,
               // from: new Twilio.Types.PhoneNumber("your_twilio_phone_number"),
               // to: new Twilio.Types.PhoneNumber(numeroCelular)
           // );

            // Puedes manejar la respuesta o lanzar una excepción si hay un error
        }
    }
}

