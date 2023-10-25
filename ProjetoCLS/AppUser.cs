using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace ProjetoCLS
{
    public class AppUser
    {
        private string _connString = string.Empty;

        public AppUser()
        {

        }

        public AppUser(string callingApp)
        {
            _connString = DataAccess.GetAppConfig(callingApp).DbConn;
        }

        #region Properties
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }

        #endregion Properties

        public class Roles
        {
            public const string SUPER = "Super Administrador";
            public const string ADMIN = "Administrador";
            public const string MANAGER = "Gestor";
            public const string EDITOR = "Editor";
            public const string VIEWER = "Consulta";
            public const string ANALIST = "Análise";
            public const string BLOCKED = "Sem permissão";
        }

        // Método para autenticar o utilizador
        public AppUser Login(string userName, string password)
        {
            SqlConnection connection = new SqlConnection(_connString);

            using (connection)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    // Vamos selecionar todos os utilizadores
                    SqlCommand command = new SqlCommand(@"select [Id], [Login], [Nome], 
                        [Email], [Permissao], [Ativo] from tbUtilizadores where [Login]='"
                        + userName + "' and [Password] = '" + password + "' and [Ativo]=1", connection);

                    // Vamos ler os resultados que são devolvidos pela query sql da linha anterior
                    SqlDataReader dataReader = command.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        AppUser userInfo = new AppUser();

                        // Enquanto o 'dataReader' conseguir ler os dados obtidos
                        // da base de dados coloca os dados no objeto 'userInfo'
                        while (dataReader.Read())
                        {
                            userInfo.Id = dataReader["Id"].ToString();
                            userInfo.UserName = dataReader["Login"].ToString();
                            userInfo.Name = dataReader["Nome"].ToString();
                            userInfo.Email = dataReader["Email"].ToString();
                            userInfo.Role = dataReader["Permissao"].ToString();
                            userInfo.Active = (bool)dataReader["Ativo"];
                        }

                        // Devolve o objeto com os dados a quem chamou o método 'User.Autenticar'
                        return userInfo;
                    }
                    else
                        return null;
                }
                catch (Exception)
                {
                    connection.Close();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public List<AppUser> ListUsers()
        {
            SqlConnection connection = new SqlConnection(_connString);

            using (connection)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    SqlCommand cm = new SqlCommand(@"select [Id], [Login], [Nome], 
                        [Email], [Permissao], [Ativo] from tbUtilizadores", connection);

                    SqlDataReader dr = cm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        // Cria uma lista de objetos do tipo 'User.Info'
                        List<AppUser> listaUsers = new List<AppUser>();

                        // Enquanto o 'dataReader' conseguir ler os dados obtidos
                        // da base de dados coloca os dados no objeto 'userInfo'
                        while (dr.Read())
                        {
                            // Cria um objeto do tipo 'User.Info'
                            AppUser userInfo = new AppUser();

                            userInfo.Id = dr["Id"].ToString();
                            userInfo.UserName = dr["Login"].ToString();
                            userInfo.Name = dr["Nome"].ToString();
                            userInfo.Email = dr["Email"].ToString();
                            userInfo.Role = dr["Permissao"].ToString();
                            userInfo.Active = (bool)dr["Ativo"];

                            // Como a query pode devolver mais do que um registo da base de dados,
                            // depois de colocarmos os dados no objeto 'userInfo', atribuímos
                            // este objeto à lista de objetos 'listaUsers' do tipo 'User.Info'
                            // ficando o objeto 'listaUsers' com uma coleção de dados do mesmo tipo
                            listaUsers.Add(userInfo);

                            //Outro método para adicionar
                            //listaUsers.Add(new AppUser() { 
                            //    Id = dr["Id"].ToString(),
                            //    UserName = dr["Login"].ToString(),
                            //    Name = dr["Nome"].ToString(),
                            //    Email = dr["Email"].ToString(),
                            //    Role = dr["Permissao"].ToString(),
                            //    Active = (bool)dr["Ativo"]
                            //});
                        }

                        // Devolve a lista de utilizadores a quem chamou o método User.Autenticar
                        return listaUsers;
                    }
                    else
                        return null;
                }
                catch (Exception)
                {
                    connection.Close();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool Create(AppUser user)
        {
            SqlConnection connection = new SqlConnection(_connString);

            using (connection)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    // Query SQL que insere um utilizador
                    // O @ permite escrever numa cadeia de texto (string) em multiplas linhas
                    // sem termos de estar constantemente a fechar " e a adicionar o sinal +
                    // para mudar de linha
                    SqlCommand commmand = new SqlCommand(@"insert into tbUtilizadores 
                        (Id, Login, Password, Nome, Email, Permissao, Ativo) values
                        (@Id, @Login, @Password, @Nome, @Email, @Permissao, @Ativo)", connection);

                    // Adicionamos os valores do parâmetro recebido pelo método às variáveis da query SQL
                    commmand.Parameters.AddWithValue("@Id", user.Id);
                    commmand.Parameters.AddWithValue("@Login", user.UserName);
                    commmand.Parameters.AddWithValue("@Password", user.Password);
                    commmand.Parameters.AddWithValue("@Nome", user.Name);
                    commmand.Parameters.AddWithValue("@Email", user.Email);
                    commmand.Parameters.AddWithValue("@Permissao", user.Role);
                    commmand.Parameters.AddWithValue("@Ativo", user.Active);

                    // Executamos o comando e ficamos com o resultado das linhas afetadas na tabela da BD
                    var resultado = commmand.ExecuteNonQuery();

                    // Se o resultado for 1 significa que adicionamos um utilizador o que configura que 
                    // o código foi executado como pretendido, caso contrário retorna 'false'
                    if (resultado == 1)
                        return true;
                    else
                        return false;

                }
                catch (Exception)
                {
                    connection.Close();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool Update(AppUser user)
        {
            SqlConnection connection = new SqlConnection(_connString);

            using (connection)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    // Query SQL que insere um utilizador
                    // O @ permite escrever numa cadeia de texto (string) em multiplas linhas
                    // sem termos de estar constantemente a fechar " e a adicionar o sinal +
                    // para mudar de linha
                    SqlCommand commmand = new SqlCommand(@"update tbUtilizadores 
                        set Nome=@Nome, Email=@Email, 
                        Permissao=@Permissao, Ativo=@Ativo WHERE Login=@Login", connection);

                    // Adicionamos os valores do parâmetro recebido pelo método às variáveis da query SQL                    
                    commmand.Parameters.AddWithValue("@Nome", user.Name);
                    commmand.Parameters.AddWithValue("@Email", user.Email);
                    commmand.Parameters.AddWithValue("@Permissao", user.Role);
                    commmand.Parameters.AddWithValue("@Ativo", user.Active);
                    commmand.Parameters.AddWithValue("@Login", user.UserName);

                    // Executamos o comando e ficamos com o resultado das linhas afetadas na tabela da BD
                    var resultado = commmand.ExecuteNonQuery();

                    // Se o resultado for 1 significa que alteramos um utilizador o que configura que 
                    // o código foi executado como pretendido, caso contrário retorna 'false'
                    if (resultado == 1)
                        return true;
                    else
                        return false;

                }
                catch (Exception)
                {
                    connection.Close();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool Delete(string userName)
        {
            SqlConnection connection = new SqlConnection(_connString);

            using (connection)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    // Query SQL que insere um utilizador
                    // O @ permite escrever numa cadeia de texto (string) em multiplas linhas
                    // sem termos de estar constantemente a fechar " e a adicionar o sinal +
                    // para mudar de linha
                    SqlCommand commmand = new SqlCommand(@"delete from tbUtilizadores 
                        WHERE Login=@Login", connection);

                    // Adicionamos os valores do parâmetro recebido pelo método às variáveis da query SQL
                    commmand.Parameters.AddWithValue("@Login", userName);

                    // Executamos o comando e ficamos com o resultado das linhas afetadas na tabela da BD
                    var resultado = commmand.ExecuteNonQuery();

                    // Se o resultado for 1 significa que apagamos um utilizador o que configura que 
                    // o código foi executado como pretendido, caso contrário retorna 'false'
                    // Em caso de erro o SQL devolve -1, e se não afetar nenhuma linha devolve zero,
                    // o que significa que o utilizador que se queria apagar não existe na base de dados
                    if (resultado == 1)
                        return true;
                    else
                        return false;

                }
                catch (Exception)
                {
                    connection.Close();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public string GetPassword(string user)
        {
            SqlConnection connection = new SqlConnection(_connString);

            using (connection)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    // Vamos selecionar todos os utilizadores
                    SqlCommand command = new SqlCommand(@"select [Password] from tbUtilizadores where [Login]='"
                    + user + "' and [Ativo]=1", connection);


                    // Vamos ler os resultados que são devolvidos pela query sql da linha anterior
                    SqlDataReader dataReader = command.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        string pass = "";

                        // Enquanto o 'dataReader' conseguir ler os dados obtidos
                        // da base de dados coloca os dados no objeto 'userInfo'
                        while (dataReader.Read())
                        {
                            pass = dataReader["Password"].ToString();
                        }

                        // Devolve o objeto com os dados a quem chamou o método 'User.Autenticar'
                        return pass;
                    }
                    else
                        return null;
                }
                catch (Exception)
                {
                    connection.Close();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public string GetPermissao(string user)
        {
            SqlConnection connection = new SqlConnection(_connString);

            using (connection)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    // Vamos selecionar todos os utilizadores
                    SqlCommand command = new SqlCommand(@"select [Permissao] from tbUtilizadores where [Login]='"
                    + user + "' and [Ativo]=1", connection);


                    // Vamos ler os resultados que são devolvidos pela query sql da linha anterior
                    SqlDataReader dataReader = command.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        string permissao = "";

                        // Enquanto o 'dataReader' conseguir ler os dados obtidos
                        // da base de dados coloca os dados no objeto 'userInfo'
                        while (dataReader.Read())
                        {
                            permissao = dataReader["Permissao"].ToString();
                        }

                        // Devolve o objeto com os dados a quem chamou o método 'User.Autenticar'
                        return permissao;
                    }
                    else
                        return null;
                }
                catch (Exception)
                {
                    connection.Close();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool ChangePassword(string pass, string login)
        {
            SqlConnection connection = new SqlConnection(_connString);

            using (connection)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    // Query SQL que insere um utilizador
                    // O @ permite escrever numa cadeia de texto (string) em multiplas linhas
                    // sem termos de estar constantemente a fechar " e a adicionar o sinal +
                    // para mudar de linha
                    SqlCommand commmand = new SqlCommand(@"update tbUtilizadores 
                        set Password = @Password WHERE Login = @Login", connection);

                    // Adicionamos os valores do parâmetro recebido pelo método às variáveis da query SQL
                    commmand.Parameters.AddWithValue("@Login", login);
                    commmand.Parameters.AddWithValue("@Password", pass);

                    // Executamos o comando e ficamos com o resultado das linhas afetadas na tabela da BD
                    var resultado = commmand.ExecuteNonQuery();

                    // Se o resultado for 1 significa que alteramos um utilizador o que configura que 
                    // o código foi executado como pretendido, caso contrário retorna 'false'
                    if (resultado == 1)
                        return true;
                    else
                        return false;

                }
                catch (Exception)
                {
                    connection.Close();
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

     }
}
