using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Eco_life.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Eco_life.Pages
{
    public class CadastrarFornecedorModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CadastrarFornecedorModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Funcionario1 Fornecedor1 { get; set; } = new Funcionario1();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Verifica se o e-mail do fornecedor não está vazio
            if (string.IsNullOrEmpty(Fornecedor1.Email_Funcionario))
            {
                ModelState.AddModelError("", "O e-mail do fornecedor não pode estar vazio.");
                return Page();
            }

            // Verifica se o e-mail já está cadastrado
            var emailExists = await _context.Funcionarios1.AnyAsync(f => f.Email_Funcionario == Fornecedor1.Email_Funcionario);
            if (emailExists)
            {
                ModelState.AddModelError("", "Este e-mail já está cadastrado.");
                return Page();
            }

            // Adicione o Fornecedor1 ao contexto
            _context.Funcionarios1.Add(Fornecedor1);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cadastro realizado com sucesso!";
            return RedirectToPage("./Index");
        }
    }
}
