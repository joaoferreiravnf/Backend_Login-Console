using System;
using System.Data.SqlClient;

namespace ProjetoCLS
{
    // [DataAccess] Classe que fornece ao projeto (console/asp.net/windows forms)
    // que referencia esta livraria, uma ligação a uma base de dados, que pode ser a
    // base de dados "base" ou outra ligação definida nessa base de dados numa tabela específica
    public class DataAccess
    {
        #region Variáveis

        // Variáveis onde especificamos a ligação DEFAULT à base de dados
        private const string serverName = "poweron.pt\\MSSQLSERVER2017";
        private const string dbName = "EIsnt";
        private const string userName = "EIsnt";
        private const string password = "46zQq69l$";

        #endregion

        #region Métodos

        // Método privado que aceita como parâmetro uma variável do tipo string com o valor do
        // ID da aplicação que está em execução e procura na base de dados na tabela onde se
        // encontram armazenadas todas as ligações 'tbConfig' pela ligação correspondente para
        // devolver um resultado do tipo SqlConnection para que seja usado sempre que solicitado
        private static AppConfig GetAppInfoFromDB(string appId = null, bool getConnStr = true)
        {
            //string connStr = "Server=poweron.pt\\MSSQLSERVER2017;Database=EIsnt;User Id=EIsnt;Password=46zQq69l$;";
            string connStr = $"Server={serverName};Database={dbName};User Id={userName};Password={password};";

            // Implementar a ligação à base de dados com os dados da connection string criada na linha anterior
            SqlConnection ligacaoBD = new SqlConnection(connStr);

            if (appId != null)
            {
                try
                {
                    ligacaoBD.Open();

                    SqlCommand cm = new SqlCommand("select [Id], [Nome], [Versao], [Online], [LigacaoBD] from tbConfig where [Id]=@AppId", ligacaoBD);
                    cm.Parameters.AddWithValue("@AppId", appId);

                    SqlDataReader dataReader = cm.ExecuteReader();

                    if (!dataReader.HasRows)
                    {
                        throw new Exception("Ligação não encontrada! Abortar...");
                    }
                    else
                    {
                        while (dataReader.Read())
                        {
                            ////Leitura de valores da base de dados - Método 1
                            //string newConnStr = dr["LigacaoBD"].ToString();
                            //SqlConnection novaLigacaoBD = new SqlConnection(newConnStr);
                            //return novaLigacaoBD;

                            ////Leitura de valores da base de dados - Método 2
                            //return new SqlConnection(dr["LigacaoBD"].ToString());

                            //// Atribui os valores da BD a um objeto e devolve-os - Método 1
                            //AppInfo temp = new AppInfo();
                            //temp.Id = dataReader["Id"].ToString();
                            //temp.Nome = dataReader["Nome"].ToString();
                            //temp.Versao = dataReader["Versao"].ToString();
                            ////conversão para boleano
                            //temp.Online = Convert.ToBoolean(dataReader["Online"]);
                            ////conversão para boleano através de um typecast
                            ////temp.Online = (bool)dataReader["Online"];
                            //temp.LigacaoBD = new SqlConnection(dataReader["LigacaoBD"].ToString());
                            //return temp;

                            //// Atribui os valores da BD a um objeto e devolve-os - Método 2
                            return new AppConfig()
                            {
                                Id = dataReader["Id"].ToString(),
                                Name = dataReader["Nome"].ToString(),
                                Version = dataReader["Versao"].ToString(),
                                //conversão para boleano
                                Online = Convert.ToBoolean(dataReader["Online"]),
                                //conversão para boleano através de um typecast
                                //temp.Online = (bool)dataReader["Online"];
                                DbConn = getConnStr ? dataReader["LigacaoBD"].ToString() : string.Empty,
                            };
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                    //throw new Exception("Erro ao estabelecer a ligação! Abortar...");
                }
                finally
                {
                    ligacaoBD.Close();
                }
            }

            // Caso exista só uma base de dados, inicializamos os detalhes da aplicação
            // com valores por defeito mas que podem ser obtidos através de um ficheiro
            // de configuração externo ou da base de dados configurada por defeito
            return new AppConfig()
            {
                Id = appId,
                Name = appId,
                Online = false,
                Version = "1.0",
                DbConn = connStr
            };
        }

        // Método que aceita um objeto do tipo 'AppInfo' e que retorna outro objeto
        // do mesmo tipo mas com valores devolvidos pelo método privado [ObterLigacaoBD]
        protected internal static AppConfig GetAppConfig(string appId) => GetAppInfoFromDB(appId, true);

        protected internal static AppConfig GetAppInfo(string appId) => GetAppInfoFromDB(appId, false);
        #endregion
    }
}