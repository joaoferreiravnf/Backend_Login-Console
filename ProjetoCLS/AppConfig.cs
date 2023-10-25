using System.Data.SqlClient;
using System.Reflection;

namespace ProjetoCLS
{
    // [AppInfo] Classe que retorna do código do projeto e da base de dados, informação
    // sobre o projeto que está a funcionar no momento em que o código está em execução
    // (run-time)
    public class AppConfig
    {
        #region Construtor

        // Construtor vazio para poder instanciar esta classe sem passar o Id do projeto
        public AppConfig()
        {

        }

        #endregion

        #region Propriedades

        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public bool Online { get; set; }
        public string DbConn { get; set; }

        #endregion

        #region Métodos

        // Devolve o nome e a versão da aplicação - Método 1 (através de um método)
        //public string Detalhes()
        //{
        //    return Nome + " v" + Versao;
        //}

        // Devolve o nome e a versão da aplicação - Método 2 (através de um método anónimo)
        public string Title() => Id + " v" + Version;

        // Devolve o nome e a versão da aplicação - Método 3 (através de um método anónimo
        // e fazendo um override a um método pré-existente ToString() que ficará com o valor)
        //public override string ToString() => Nome + " " + Versao;

        // Devolve o nome e a versão da aplicação - Método 4 (através de um override a um
        // método pré-existente ToString() que ficará com o valor)
        //public override string ToString()
        //{
        //    return Nome + " v" + Versao;
        //}

        public static AppConfig AppInfo() => DataAccess.GetAppInfo(Assembly.GetCallingAssembly().ToString().Split(',')[0]);
        #endregion
    }
}
