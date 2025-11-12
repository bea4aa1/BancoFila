using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoFila
{
    public class Conexao
    {
        string conectar = "server=localhost;database=BancoFila;uid=root;password=root;";
  
        public MySqlConnection conexao()
        {
            MySqlConnection conexao = new MySqlConnection(conectar);
            conexao.Open();
            return conexao;
        }
    }
}