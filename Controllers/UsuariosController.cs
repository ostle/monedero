using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monedero.Context;
using Monedero.Models;
using Microsoft.AspNetCore.Http;


namespace Monedero.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly MonederoContext _context;

        public UsuariosController(MonederoContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.Saldo = HttpContext.Session.GetString("Saldo");
            ViewBag.Usuario = HttpContext.Session.GetString("UserID");
            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,Nombre,Apellido,Email,Password")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                Cuenta cuenta = new Cuenta();
                cuenta.Saldo = 0;
                usuario.Cuenta = cuenta;
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return View("~/Views/Login.cshtml");
            }
            return View("~/Views/Login.cshtml");
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.Saldo = HttpContext.Session.GetString("Saldo");
            ViewBag.Usuario = HttpContext.Session.GetString("UserID");
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Nombre,Apellido,Email,Password")] Usuario usuario)
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.Saldo = HttpContext.Session.GetString("Saldo");
            ViewBag.Usuario = HttpContext.Session.GetString("UserID");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }                
                return View("~/Views/Home/Index.cshtml");
            }

            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
