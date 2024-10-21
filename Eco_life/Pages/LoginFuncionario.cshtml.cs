using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Eco_life.Models;
using Microsoft.AspNetCore.Http;

namespace Eco_life.Pages
{
    public class LoginFuncionarioModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginFuncionarioModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string? Email_Funcionario { get; set; }

        [BindProperty]
        public string? Senha_Funcionario { get; set; }

        public string? ErrorMessage { get; set; } // Para mensagens de erro

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Email_Funcionario) || string.IsNullOrWhiteSpace(Senha_Funcionario))
            {
                ErrorMessage = "Email e senha s�o obrigat�rios.";
                return Page(); // Retorna a p�gina atual com erro
            }

            // Tenta encontrar o funcion�rio com as credenciais fornecidas
            var funcionario = _context.Funcionarios1
                .FirstOrDefault(f => f.Email_Funcionario == Email_Funcionario && f.Senha_Funcionario == Senha_Funcionario);

            if (funcionario != null)
            {
                // Autentica��o bem-sucedida
                HttpContext.Session.SetString("FuncionarioId", funcionario.Id_Funcionario.ToString());
                HttpContext.Session.SetString("FuncionarioNome", funcionario.Nome_Funcionario ?? "");
                HttpContext.Session.SetString("FuncionarioEmail", funcionario.Email_Funcionario ?? "");
                return RedirectToPage("/Index"); // Redireciona para home
            }

            // Falha na autentica��o
            ErrorMessage = "Email ou senha inv�lidos."; // Define a mensagem de erro
            return Page(); // Retorna a p�gina atual com erro

        }
    }
}

