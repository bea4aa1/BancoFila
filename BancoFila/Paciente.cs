using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoFila
{
   public class Paciente
    {
  
        public string nome;
        private string cpf;
        public string preferencial;
        public int posicao;


        public void Cadastrar()
        {
            Console.WriteLine("Digite seu nome:");
            nome = Console.ReadLine();

            Console.WriteLine("Digite seu CPF:");
            cpf = Console.ReadLine();

            Console.WriteLine("Atendimento preferencial?(Responda com sim ou não):");
            preferencial = Console.ReadLine();
        }



        public void AlterarDados()
        {
            Console.WriteLine("Digite seu novo nome:");
            nome = Console.ReadLine();

            Console.WriteLine("Digite seu novo CPF:");
            cpf = Console.ReadLine();

            Console.WriteLine("Atendimento preferencial?(Responda com sim ou não):");
            preferencial = Console.ReadLine();
        }




        public void Exibir(int posicao)
        {
            string tipo = (preferencial.ToLower() == "sim") ? "PREFERENCIAL" : "COMUM";
            Console.WriteLine($"{posicao} - Nome: {nome} | CPF: {cpf} | Tipo: {tipo}");
        }

    }
}

