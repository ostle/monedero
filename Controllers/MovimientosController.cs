using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monedero.Context;
using Monedero.Models;
using Microsoft.AspNetCore.Http;

namespace Monedero.Controllers
{
    public class MovimientosController : Controller
    {
        private readonly MonederoContext _context;

        public MovimientosController(MonederoContext context)
        {
            _context = context;
        }

        // GET: Movimientos
        public async Task<IActionResult> Index()
        {
            var micuenta = HttpContext.Session.GetInt32("CuentaID");
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.Saldo = HttpContext.Session.GetString("Saldo");
            ViewBag.Usuario = HttpContext.Session.GetString("UserID");
            var movs = from m in _context.Movimientos where m.CuentaId == micuenta orderby m.Fecha select m;
            
            return View(movs);
        }

        // GET: Movimientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.Saldo = HttpContext.Session.GetString("Saldo");
            ViewBag.Usuario = HttpContext.Session.GetString("UserID");

            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.Movimientos
                .FirstOrDefaultAsync(m => m.MovimientoId == id);
            if (movimiento == null)
            {
                return NotFound();
            }

            return View(movimiento);
        }

        // GET: Movimientos/Create
        public IActionResult Create()
        {
            ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
            ViewBag.Saldo = HttpContext.Session.GetString("Saldo");
            ViewBag.Usuario = HttpContext.Session.GetString("UserID");
            return View();
        }

        // POST: Movimientos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovimientoId,Importe,Descripcion,TipoMovimiento")] Movimiento movimiento)
        {
            if (ModelState.IsValid)
            {                
                movimiento.Fecha =DateTime.Now;
                
                var consulta = _context.Cuentas.Where(s => s.CuentaId == HttpContext.Session.GetInt32("CuentaID"));
                var cuenta = consulta.FirstOrDefault<Cuenta>();
                var saldo=cuenta.Saldo;
                var micuenta = cuenta.CuentaId;

                movimiento.CuentaId = micuenta;

                var esValido = true;
                //Si existe la cuenta
                if (movimiento.TipoMovimiento.Equals(TipoMovimiento.Ingreso))
                {
                    var aux = saldo + movimiento.Importe;
                    cuenta.Saldo = aux;
                    _context.SaveChanges();
                    HttpContext.Session.SetString("Saldo", aux.ToString());
                }
                else {
                    var aux = saldo - movimiento.Importe;
                    if (aux >= 0)
                    {
                        HttpContext.Session.SetString("Saldo", aux.ToString());
                        cuenta.Saldo = aux;
                        _context.SaveChanges();
                    }
                    else
                    {
                        esValido = false;
                    }
                }

                if (esValido)
                {
                    _context.Add(movimiento);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Redirigir a error
                    ModelState.AddModelError("Importe", "El monto ingresado supera el saldo de la cuenta");
                    return View(movimiento);
                }
            }
            return View(movimiento);
        }       

        // GET: Movimientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.Movimientos.FindAsync(id);
            if (movimiento == null)
            {
                return NotFound();
            }
            return View(movimiento);
        }

        // POST: Movimientos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovimientoId,Importe,Descripcion,Fecha,TipoMovimiento")] Movimiento movimiento)
        {
            if (id != movimiento.MovimientoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movimiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovimientoExists(movimiento.MovimientoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movimiento);
        }

        // GET: Movimientos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.Movimientos
                .FirstOrDefaultAsync(m => m.MovimientoId == id);
            if (movimiento == null)
            {
                return NotFound();
            }

            return View(movimiento);
        }

        // POST: Movimientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movimiento = await _context.Movimientos.FindAsync(id);
            _context.Movimientos.Remove(movimiento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovimientoExists(int id)
        {
            return _context.Movimientos.Any(e => e.MovimientoId == id);
        }
    }
}
