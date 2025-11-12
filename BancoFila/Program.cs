﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace BancoFila
{
    internal class Program
    {
        public class Conexao
        {
            string conectar = "server=localhost;database=BancoFila;uid=root;password=root;";

            public MySqlConnection GetConexao()
            {
                return new MySqlConnection(conectar);
            }
        }

            static void Main(string[] args)
            {
                Conexao slaConexao = new Conexao();

                try
                {
                    MySqlConnection conexao = slaConexao.GetConexao();

                    using (conexao)
                    {
                        conexao.Open();

                        Console.WriteLine("Conexão aberta com sucesso!");

                        Paciente[] fila = new Paciente[15];

                        int numeropessoas = 0;
                        string opcao = "1";


                        while (opcao != "q")
                        {
                            Console.Clear();
                            Console.WriteLine("\n----------Menu----------");
                            Console.WriteLine("1 - Cadastrar paciente");
                            Console.WriteLine("2 - Listar pacientes da fila");
                            Console.WriteLine("3 - Atendimento");
                            Console.WriteLine("4 - Alterar dados");
                            Console.WriteLine("5 - Sair (Digite q)");
                            Console.WriteLine("Digite sua opção:");
                            opcao = Console.ReadLine();

                            if (opcao == "1")
                            {
                                if (numeropessoas >= 15)
                                {
                                    Console.WriteLine("Fila cheia, por favor aguarde");
                                }
                                else
                                {
                                    Paciente novo = new Paciente();
                                    novo.Cadastrar();
                                    fila[numeropessoas] = novo;
                                    numeropessoas++;

                                    
                                    Cadastrar(conexao);
                                }
                            }

                            if (opcao == "2")
                            {
                                Console.Clear();
                                Console.WriteLine("----------Lista de pacientes----------\n");

                                if (numeropessoas == 0)
                                {
                                    Console.WriteLine("A fila está vazia.");
                                }
                                else
                                {
                                    Console.WriteLine("Pacientes Preferenciais:");
                                    Console.WriteLine("-------------------------");
                                    for (int i = 0; i < numeropessoas; i++)
                                    {
                                        if (fila[i].preferencial.ToLower() == "sim")
                                        {
                                            fila[i].Exibir(i + 1);
                                        }
                                    }

                                    Console.WriteLine("\nPacientes Comuns:");
                                    Console.WriteLine("-------------------------");
                                    for (int i = 0; i < numeropessoas; i++)
                                    {
                                        if (fila[i].preferencial.ToLower() != "sim")
                                        {
                                            fila[i].Exibir(i + 1);
                                        }
                                    }
                                }

                                
                                Console.WriteLine("\n----- Lista -----");
                                Exibir(conexao);

                                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                                Console.ReadKey();
                            }

                            if (opcao == "3")
                            {
                                Console.Clear();
                                Console.WriteLine("----------Atendimento----------\n");

                                if (numeropessoas == 0)
                                {
                                    Console.WriteLine("Não há pacientes na fila");
                                }
                                else
                                {
                                    Console.WriteLine("Atendendo paciente:");
                                    fila[0].Exibir(1);

                                    for (int i = 0; i < numeropessoas - 1; i++)
                                    {
                                        fila[i] = fila[i + 1];
                                    }

                                    numeropessoas--;
                                    Console.WriteLine("\nPaciente atendido e removido da fila");
                                }

                                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                                Console.ReadKey();
                            }

                            if (opcao == "4")
                            {
                                Console.Clear();
                                Console.WriteLine("----------Alterar Dados----------\n");

                                if (numeropessoas == 0)
                                {
                                    Console.WriteLine("Não há pacientes para alterar.");
                                }
                                else
                                {
                                    for (int i = 0; i < numeropessoas; i++)
                                    {
                                        fila[i].Exibir(i + 1);
                                    }

                                    Console.Write("\nDigite o número do paciente que deseja alterar: ");
                                    int escolha;

                                    if (int.TryParse(Console.ReadLine(), out escolha) && escolha > 0 && escolha <= numeropessoas)
                                    {
                                        fila[escolha - 1].AlterarDados();
                                        Console.WriteLine("\nDados alterados na fila em memória com sucesso.");

                                     
                                        AlterarDados(conexao);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Número inválido.");
                                    }
                                }

                                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                                Console.ReadKey();
                            }

                            if (opcao == "5" || opcao == "q")
                            {
                                opcao = "q";
                                Console.WriteLine("\nSaindo do sistema...");
                                Console.ReadKey();
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Erro ao conectar: {ex.Message}");
                }
            }

            

            static void Cadastrar(MySqlConnection conexao)
            {
                Console.WriteLine("\n--- Cadastro do Paciente ---");

                Console.Write("Digite o nome: ");
                string nomeDigitado = Console.ReadLine();

                Console.Write("Digite o CPF: ");
                string cpfDigitado = Console.ReadLine();

                
                string sqlInsert = "INSERT INTO Paciente (Nome_paciente, Cpf_paciente) VALUES (@nome, @cpf)";

                using (MySqlCommand cmd = new MySqlCommand(sqlInsert, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", nomeDigitado);
                    cmd.Parameters.AddWithValue("@cpf", cpfDigitado);

                    int linhas = cmd.ExecuteNonQuery();

                    
                }
            }


            static void Exibir(MySqlConnection conexao)
            {
                Console.WriteLine("\n--- Lista de pacientes cadastrados ---");

                
                string sqlSelect = "SELECT Cod_paciente, Nome_paciente, Cpf_paciente FROM Paciente";

                using (MySqlCommand cmd = new MySqlCommand(sqlSelect, conexao))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int codigo = Convert.ToInt32(reader["Cod_paciente"]);
                            string nome = reader.GetString("Nome_paciente");
                            string cpf = reader.GetString("Cpf_paciente");

                            Console.WriteLine($"Cód: {codigo} | Nome: {nome} | Cpf: {cpf}");
                        }
                    }
                }
            }


            static void AlterarDados(MySqlConnection conexao)
            {
                Console.WriteLine("\n--- Atualização do Paciente  ---");
                Console.Write("Digite o codigo do paciente: ");
                int idParaAtualizar = Convert.ToInt32(Console.ReadLine());

                Console.Write("Digite o novo nome: ");
                string novoNome = Console.ReadLine();

                Console.Write("Digite o novo cpf:");
                string novoCpf = Console.ReadLine();

               
                string sqlUpdate = "UPDATE Paciente SET Nome_paciente = @nome,  Cpf_paciente = @cpf WHERE Cod_paciente = @id";

                using (MySqlCommand cmd = new MySqlCommand(sqlUpdate, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", novoNome);
                    cmd.Parameters.AddWithValue("@cpf", novoCpf);
                    cmd.Parameters.AddWithValue("@id", idParaAtualizar);

                    int alterados = cmd.ExecuteNonQuery();

           
                }
            }
        }
    }
