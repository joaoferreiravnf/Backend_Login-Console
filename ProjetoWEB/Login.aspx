<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProjetoWEB.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ProjetoWEB</title>
    <link rel="stylesheet" href="styles.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-rbsA2VBKQhggwzxH7pPCaAqO46MgnOM80zW1RWuH61DGLwZJEdK2Kadq2F9CUG65" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-kenU1KFdBIe4zVF0s0G1M5b4hcpxyD9F7jL+jjXkk+Q2h455rYXK/7HAuoJl+0I4" crossorigin="anonymous"></script>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        function userLoginMessage(iconVar, titleVar, htmlVar) {           
            Swal.fire({
                icon: iconVar,
                title: titleVar,
                html: htmlVar
            })          
        }
        window.onload = function () {
            Swal.fire({
                icon: iconVar,
                title: titleVar,
                html: htmlVar
            })     
        };        
    </script>
</head>

<body>
    <div class="back">
        <div class="div-center">
            <div class="content">
                <h3>Autenticar</h3>
                <hr />
                <form id="form_login" runat="server">
                    <div class="form-group">
                        <%--<label for="username">Utilizador</label>--%>
                        <%--<input type="text" class="form-control" id="username" placeholder="Utilizador" runat="server" />--%>

                        <asp:Label ID="label1" runat="server" Text="Utilizador"></asp:Label>
                        <asp:TextBox ID="Utilizador" class="form-control" runat="server" placeholder="Utilizador"></asp:TextBox>
                    </div>

                    <div class="form-group" style="margin-top: 10px">
                        <label for="password">Senha</label>
                        <input type="password" class="form-control" id="password" placeholder="Password" runat="server" />
                    </div>
                    <br />
                    <button id="login" class="btn btn-primary" runat="server" onserverclick="AutenticaUtilizador_Click">Entrar</button>
                    <hr />
                    <label id="mensagem" runat="server"></label>
                </form>
            </div>
        </div>
    </div>
</body>
</html>
