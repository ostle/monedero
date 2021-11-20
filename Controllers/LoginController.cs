using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Monedero.Context;
using Monedero.Models;
using Microsoft.AspNetCore.Http;

namespace Monedero.Controllers
{
    public class LoginController : Controller
    {
        private readonly MonederoContext _context;

        public LoginController(MonederoContext context)
        {
            _context = context;
        }

        // GET: Login
        public async Task<IActionResult> Index()
        {
            return View("~/Views/Login.cshtml");
        }

        [HttpPost, ActionName("Login")]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Usuario user)
        {           
            using (_context)
            {
                var obj = _context.Usuarios.Where(a => a.Email.Equals(user.Email) && a.Password.Equals(user.Password)).FirstOrDefault();
                if (obj != null)
                {
                    var cons = from us in _context.Usuarios where us.UsuarioId == obj.UsuarioId select us.Cuenta;
                    var micuenta= cons.FirstOrDefault<Cuenta>();

                    HttpContext.Session.SetString("UserID", obj.UsuarioId.ToString());
                    HttpContext.Session.SetInt32("CuentaID", micuenta.CuentaId);
                    HttpContext.Session.SetString("Nombre", obj.Nombre.ToString());
                    HttpContext.Session.SetString("Saldo", micuenta.Saldo.ToString());

                    return RedirectToAction("UserDashBoard");
                }
            }
            
            ModelState.AddModelError("Password", "Los datos ingresados son invalidos");
            return View("~/Views/Login.cshtml");
        }

        public IActionResult UserDashBoard()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
                ViewBag.Saldo = HttpContext.Session.GetString("Saldo");
                ViewBag.Usuario = HttpContext.Session.GetString("UserID");
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                return View("~/Views/Login.cshtml");
            }
        }

        public IActionResult CerrarSesion()
        {            
            HttpContext.Session.SetString("UserID", "");
            HttpContext.Session.SetInt32("CuentaID", 0);
            HttpContext.Session.SetString("Nombre", "");
            HttpContext.Session.SetString("Saldo", "");

            return View("~/Views/Login.cshtml");
        }
    }
}
