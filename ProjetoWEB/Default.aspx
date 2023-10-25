<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProjetoWEB.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        function userLoginMessage(icon, title, html) {           
            Swal.fire({
                icon: icon,
                title: title,
                html: html
            })          
        }
        window.onload = function () {
            
        };        
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <h2>Página principal!</h2>

            <h3>
                <label id="welcome" runat="server"></label>
            </h3>
            <br />
            <button id="logout" class="btn btn-primary" runat="server" onserverclick="Logout_Click">Logout</button>
        </div>
    </form>
</body>
</html>