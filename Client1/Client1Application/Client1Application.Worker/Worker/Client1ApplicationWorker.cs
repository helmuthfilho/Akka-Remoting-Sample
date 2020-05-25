using Akka.Actor;
using Akka.Dispatch.SysMsg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Client1Application.Worker.Worker
{
    public class Client1ApplicationWorker : ReceiveActor
    {
        private readonly ActorSelection _serverApllication = Context.ActorSelection("akka.tcp://serverApplication@localhost:6000/user/server-application-guardian/server-application-worker");
        public Client1ApplicationWorker()
        {
            Receive<string>(SendToServer);
        }

        private void SendToServer(string line)
        {
            var correctedString = line.Replace(" ", string.Empty);
            if (correctedString.Length == 9)
            {
                _serverApllication.Tell(CPFWithCheckNumber(correctedString));
            }
            else
            {
                _serverApllication.Tell(CNPJCheckNumber(correctedString));
            }
        }

        private string CPFWithCheckNumber(string cpf)
        {
            var builder = new StringBuilder();
            int resultado = 0;
            int multiplicador = 1;
            for (int i = 0; i < 9; i++)
            {
                builder.Append(cpf[i]);
                resultado += ((int)cpf[i] - '0') * multiplicador;
                multiplicador++;
            }
            resultado = resultado % 11;
            if (resultado == 10)
            {
                resultado = 0;
            }
            builder.Append(Convert.ToString(resultado));
            resultado = 0;
            multiplicador = 0;
            for (int i = 0; i < 10; i++)
            {
                resultado += (((int)builder[i]) - '0') * multiplicador;
                multiplicador++;
            }
            resultado = resultado % 11;
            if (resultado == 10)
            {
                resultado = 0;
            }
            builder.Append(Convert.ToString(resultado));
            return builder.ToString();
        }

        private string CNPJCheckNumber(string cnpj)
        {
            var builder = new StringBuilder();
            int resultado = 0;
            int multiplicador = 6;
            for (int i = 0; i < 12; i++)
            {
                builder.Append(cnpj[i]);
                resultado += ((int)cnpj[i] - '0') * (multiplicador % 10);
                multiplicador++;
                if (multiplicador % 10 == 0)
                {
                    multiplicador += 2;
                }
            }
            resultado = resultado % 11;
            if (resultado == 10)
            {
                resultado = 0;
            }
            builder.Append(Convert.ToString(resultado));
            resultado = 0;
            multiplicador = 5;
            for (int i = 0; i < 13; i++)
            {
                resultado += (((int)builder[i]) - '0') * (multiplicador % 10);
                multiplicador++;
                if (multiplicador % 10 == 0)
                {
                    multiplicador += 2;
                }
            }
            resultado = resultado % 11;
            if (resultado == 10)
            {
                resultado = 0;
            }
            builder.Append(Convert.ToString(resultado));
            return builder.ToString();
        }
    }
}