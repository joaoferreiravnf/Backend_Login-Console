using ProjetoCLS;
using System;
using System.Collections.Generic;
using System.Globalization;
using static ProjetoCLS.AppUser;

namespace ProjetoDOS
{
    internal class Program
    {
      
        static void Main(string[] args)
        {
            string appId = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

            #region Inicializa a aplicação e obtém as configurações da mesma

            // O Objeto 'appInfo' faz uma nova instância da classe 'AppInfo' e passa o argumento do ID da app
            // através do nome do processo em execução: [ProcessName]          
            //AppInfo appInfo = new AppInfo(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            AppConfig appInfo = AppConfig.AppInfo();

            // Escreve na consola os detalhes da aplicação através do método 'Detalhes' do objeto 'appInfo'
            Console.WriteLine(appInfo.Title());

            #endregion

            #region Interacção com o utilizador para obter os dados de login

            // Lê o input do utilizador
            Console.WriteLine("\nBem-vindo! Por favor efetue o login para ter acesso às funcionalidades avançadas:\n");
            Console.Write("Utilizador: ");
            var readUser = Console.ReadLine();
            Console.Write("\nPassword: ");
            var readPass = Console.ReadLine();

            // Autentica o utilizador enviando para o método autenticar os dados 
            // inseridos no ecrã e passando a ligação da base de dados que queremos
            // usar para obter os dados do utilizador
            AppUser userApp = new AppUser(appId);
            AppUser user = userApp.Login(readUser, readPass);

            #endregion

            #region Utilizadores

            // Execução da connection e obtenção dos valores da base de dados
            try
            {

                #region Login

                if (user == null)
                {
                    Console.WriteLine("Utilizador não encontrado!");
                }
                else
                {
                    bool sair = false;
                    while (!sair)
                    { 
                        Console.Clear();
                        Console.WriteLine(appInfo.Title());

                        Console.WriteLine("\nBem vindo " + user.Name + "!\n");

                        Console.WriteLine("Escolha uma das seguintes opções:");                        
                        Console.WriteLine("\n1 - Modificar Password\n2 - Ver Utilizadores Ativos\n3 - Adicionar User\n4 - Eliminar User\n5 - Modificar Utilizador");
                        Console.WriteLine("\nEsc - Sair do Programa");

                        ConsoleKeyInfo keyInfo = Console.ReadKey();
                        Console.WriteLine();

                        if (keyInfo.Key == ConsoleKey.Escape)
                        {
                            break;
                        }

                        string escolhaOpçãoStr = keyInfo.KeyChar.ToString();

                        if (int.TryParse(escolhaOpçãoStr, out int escolhaOpção))
                        {
                            if (escolhaOpção == 1)
                            {
                                while (true)
                                {
                                    Console.WriteLine("Insira a sua password atual: ");
                                    string atualPass = Console.ReadLine();

                                    AppUser userApp3 = new AppUser(appId);
                                    var atualPassSistema = userApp3.GetPassword(readUser);

                                    if (atualPass != atualPassSistema)
                                    {
                                        Console.WriteLine("A password inserida não corresponde à password atual. Por favor, tente novamente.");
                                    }
                                    else
                                    {

                                        Console.WriteLine("Insira a sua nova password: ");
                                        string novaPass = Console.ReadLine();

                                        if (atualPass == novaPass)
                                        {
                                            Console.WriteLine("A nova password não pode ser igual à password atual. Por favor, tente novamente.");
                                        }
                                        else
                                        {
                                            AppUser userApp1 = new AppUser(appId);
                                            bool senhaAlterada = userApp1.ChangePassword(novaPass, readUser);

                                            if (senhaAlterada)
                                            {
                                                Console.WriteLine("Password alterada com sucesso!");
                                                Console.ReadKey(true);
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Não foi possível alterar a Password.");
                                                Console.ReadKey(true);
                                                break;
                                            }
                                        }
                                    
                                    }
                                }
                            }
                            else if (escolhaOpção == 2)
                            {
                                Console.WriteLine("Aqui está uma lista dos utilizadores atualmente ativos (prima qualquer tecla para mostrar a lista):\n");
                                Console.ReadKey(true);

                                AppUser userApp1 = new AppUser(appId);
                                List<AppUser> activeUsers = userApp1.ListUsers();
                                var userPermissao = userApp1.GetPermissao(readUser);

                                if (activeUsers != null)
                                {
                                    foreach (AppUser user1 in activeUsers)
                                    {
                                        Console.WriteLine("Utilizador: " + user1.UserName);
                                        Console.WriteLine("Permissão: " + user1.Role);
                                        Console.WriteLine("Ativo: " + user1.Active);
                                        Console.WriteLine("------------------------");
                                    }
                                    Console.WriteLine("Prima qualquer tecla para retornar ao menu.");
                                    Console.ReadKey(true);
                                }
                                else
                                {
                                    Console.WriteLine("Não existem utilizadores ativos.");
                                    Console.ReadKey(true);
                                }
                            }
                            else if (escolhaOpção == 3)
                            {
                                string novoID = Guid.NewGuid().ToString();
                                Console.WriteLine("Indique o novo Login: ");
                                string novoLogin = Console.ReadLine();
                                Console.WriteLine("Indique a nova Password: ");
                                string novaPass = Console.ReadLine();
                                Console.WriteLine("Indique o novo Nome: ");
                                string novoNome = Console.ReadLine();
                                Console.WriteLine("Indique o novo Email: ");
                                string novoEmail = Console.ReadLine();
                                bool permissaoValida = false;
                                string novaPermissao = string.Empty;
                                while (!permissaoValida)
                                {
                                    Console.WriteLine("Indique a nova Permissão (Super Administrador / Administrador / Gestor / Editor / Consulta / Análise / Sem permissão: ");
                                    novaPermissao = Console.ReadLine();
                                    string novaPermissaoCap1 = char.ToUpper(novaPermissao[0]) + novaPermissao.Substring(1);

                                    switch (novaPermissaoCap1)
                                    {
                                        case Roles.SUPER:
                                        case Roles.ADMIN:
                                        case Roles.MANAGER:
                                        case Roles.EDITOR:
                                        case Roles.VIEWER:
                                        case Roles.ANALIST:
                                        case Roles.BLOCKED:

                                            permissaoValida = true;
                                            break;

                                        default:
                                            Console.WriteLine("Permissão inválida. Por favor, escolha novamente.");
                                            break;
                                    }
                                }
                                string novaPermissaoCap = char.ToUpper(novaPermissao[0]) + novaPermissao.Substring(1);
                                bool estadoValido = false;
                                string novoEstado = string.Empty;

                                while (!estadoValido)
                                {
                                    Console.WriteLine("Indique o novo Estado (Ativo / Inativo): ");
                                    novoEstado = Console.ReadLine();

                                    switch (novoEstado.ToLower())
                                    {
                                        case "ativo":
                                        case "inativo":
                                            estadoValido = true;
                                            break;

                                        default:
                                            Console.WriteLine("Estado inválido. Por favor, escolha novamente.");
                                            break;
                                    }
                                }

                                AppUser novoUser = new AppUser
                                {
                                    Id = novoID,
                                    UserName = novoLogin,
                                    Password = novaPass,
                                    Name = novoNome,
                                    Email = novoEmail,
                                    Role = novaPermissao,
                                    Active = estadoValido
                                };

                                AppUser userApp1 = new AppUser(appId);
                                bool usuarioCriado = userApp1.Create(novoUser);

                                if (usuarioCriado)
                                {
                                    Console.WriteLine("Utilizador criado com sucesso! Prima qualquer tecla para voltar ao menu.");
                                    Console.ReadKey(true);
                                }
                                else
                                {
                                    Console.WriteLine("Não foi possível criar o utilizador.");
                                    Console.ReadKey(true);
                                }
                            }
                            else if (escolhaOpção == 4)
                            {
                                Console.WriteLine("Aqui está uma lista dos utilizadores atualmente ativos:\n");

                                AppUser userApp1 = new AppUser(appId);
                                List<AppUser> activeUsers = userApp1.ListUsers();

                                if (activeUsers != null)
                                {
                                    foreach (AppUser user2 in activeUsers)
                                    {
                                        Console.WriteLine("Utilizador: " + user2.UserName);
                                    }

                                    while (true)
                                    {

                                        Console.WriteLine("Insira o nome do utilizador a ser eliminado: ");
                                        string utilizadorEliminar = Console.ReadLine();

                                        if (utilizadorEliminar == readUser)
                                        {
                                            Console.WriteLine("Você está prestes a eliminar o seu próprio acesso. Deseja continuar? (S/N): ");
                                            string confirmacao = Console.ReadLine();

                                            if (confirmacao.ToLower() == "s")
                                            {
                                                AppUser userApp2 = new AppUser(appId);
                                                bool utilizadorEliminar1 = userApp1.Delete(utilizadorEliminar);
                                                Console.WriteLine("Utilizador eliminado com sucesso! Prima qualquer tecla para voltar ao menu.");
                                                Console.ReadKey(true);
                                                break;
                                            }
                                            else
                                            {
                                                continue; // Volta ao início do ciclo
                                            }
                                        }
                                        else if (utilizadorEliminar != readUser)
                                        {
                                            AppUser userApp2 = new AppUser(appId);
                                            bool utilizadorEliminar1 = userApp1.Delete(utilizadorEliminar);
                                            Console.WriteLine("Utilizador eliminado com sucesso! Prima qualquer tecla para voltar ao menu.");
                                            Console.ReadKey(true);
                                            break;
                                        }
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("Não existem utilizadores ativos. Prima qualquer tecla para voltar ao menu.");
                                    Console.ReadKey(true);
                                }

                            }
                            else if (escolhaOpção == 5)
                            {
                                Console.WriteLine("Aqui está uma lista dos utilizadores atualmente ativos:\n");

                                AppUser userApp1 = new AppUser(appId);
                                List<AppUser> activeUsers = userApp1.ListUsers();

                                if (activeUsers != null)
                                {
                                    foreach (AppUser user2 in activeUsers)
                                    {
                                        Console.WriteLine("Utilizador: " + user2.UserName);
                                    }

                                    while (true)
                                    {

                                        Console.WriteLine("Insira o nome do utilizador a ser eliminado: ");
                                        string utilizadorEliminar = Console.ReadLine();

                                        if (utilizadorEliminar == readUser)
                                        {
                                            Console.WriteLine("Você está prestes a eliminar o seu próprio acesso. Deseja continuar? (S/N): ");
                                            string confirmacao = Console.ReadLine();

                                            if (confirmacao.ToLower() == "s")
                                            {
                                                AppUser userApp2 = new AppUser(appId);
                                                bool utilizadorEliminar1 = userApp1.Delete(utilizadorEliminar);
                                                Console.WriteLine("Utilizador eliminado com sucesso! Prima qualquer tecla para voltar ao menu.");
                                                Console.ReadKey(true);
                                                break;
                                            }
                                            else
                                            {
                                                continue; // Volta ao início do ciclo
                                            }
                                        }
                                        else if (utilizadorEliminar != readUser)
                                        {
                                            AppUser userApp2 = new AppUser(appId);
                                            bool utilizadorEliminar1 = userApp1.Delete(utilizadorEliminar);
                                            Console.WriteLine("Utilizador eliminado com sucesso! Prima qualquer tecla para voltar ao menu.");
                                            Console.ReadKey(true);
                                            break;
                                        }
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("Não existem utilizadores ativos. Prima qualquer tecla para voltar ao menu.");
                                    Console.ReadKey(true);
                                }

                            }
                            else
                            {
                                Console.WriteLine("Opção inválida. Por favor tente novamente escolhendo uma das opções indicadas. \nPrima qualquer tecla para voltar ao menu de escolha.");
                                Console.ReadKey(true);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Opção inválida. Por favor tente novamente escolhendo uma das opções indicadas. \nPrima qualquer tecla para voltar ao menu de escolha.");
                            Console.ReadKey(true);
                        }
                    }                               
                    
                }
             
            }
            #endregion

            catch (Exception ex)
            {
                Console.WriteLine($"ERRO: {ex.Message}");
            }

            #endregion

            // Impede que a consola feche automáticamente no fim do programa
            Console.Write("\nObrigado por utilizar o " + appInfo.Title() + "! Pressione qualquer tecla para sair.");
            Console.ReadKey(true);
        }
    }
}
