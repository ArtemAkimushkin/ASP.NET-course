using GiornaleOnline.DataContext;
using GiornaleOnline.DataContext.Models;
using GiornaleOnline.Extensions;
using GiornaleOnline.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace GiornaleOnline.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ArticoliController : ControllerBase
    {
        private readonly ILogger<ArticoliController> _logger;
        private readonly GOContext _dc;
        public ArticoliController(ILogger<ArticoliController> logger, GOContext dc)
        {
            _logger = logger;
            _dc = dc;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticoloModel>>> GetAll()
        {
            return Ok(await _dc.Articoli
                .Include(x => x.Categoria).Include(x => x.Autore).Where(x => x.Pubblicato == true)
                .Select(x => x.ToArticoloModel())
                .ToListAsync());
        }

        public async Task<ActionResult<IEnumerable<ArticoloModel>>> GetMine()
        {
            var userIdClaim = this.User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return BadRequest();
            }
            var userId = Convert.ToInt32(userIdClaim.Value);

            return Ok(await _dc.Articoli
                .Include(x => x.Categoria).Include(x => x.Autore).Where(x => x.Autore!.Id == userId)
                .Select(x => x.ToArticoloModel())
                .ToListAsync());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticoloModel>> GetById(int id)
        {
            var articolo = await _dc.Articoli.Include(x => x.Categoria).Include(x => x.Autore)
                .SingleOrDefaultAsync(x => x.Id == id && x.Pubblicato == true);
            if (articolo == null)
            {
                return NotFound();
            }
            return Ok(articolo.ToArticoloModel());

        }

        [HttpGet("edit/{id}")]
        public async Task<ActionResult<ArticoloDTO>> GetDTOById(int id)
        {
            var userIdClaim = this.User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return BadRequest();
            }
            var userId = Convert.ToInt32(userIdClaim.Value);

            var articolo = await _dc.Articoli.Include(x => x.Categoria).Include(x => x.Autore)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (articolo == null)
            {
                return NotFound();
            }
            if (articolo.Autore!.Id != userId)
            {
                return Unauthorized();
            }
            var dto = new ArticoloDTO
            {
                CategoriaId = articolo.Categoria!.Id,
                Titolo = articolo.Title,
                Testo = articolo.Testo,
                Pubblicato = articolo.Pubblicato,
                DataCreazione = articolo.DataCreazione,
                DataUltimaModifica = articolo.DataUltimaModifica 
            };
            return Ok(dto);

        }

        [HttpPost]
        public async Task<ActionResult<ArticoloModel>> Add(ArticoloDTO item)
        {
            var userIdClaim = this.User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return BadRequest();
            }
            var userId = Convert.ToInt32(userIdClaim.Value);

            var categoria = await _dc.Categorie.FindAsync(item.CategoriaId);
            if (categoria == null)
            {
                return Problem("Categoria non trovata", statusCode: StatusCodes.Status400BadRequest);
            }
            var autore = await _dc.Utenti.FindAsync(userId);
            if (autore == null)
            {
                return Problem("Autore non trovata", statusCode: StatusCodes.Status400BadRequest);
            }
            var articolo = new Articolo
            {
                Title = item.Titolo,
                Testo= item.Testo, 
                Autore = autore,
                Categoria= categoria,
                Pubblicato= item.Pubblicato
            };

            try
            {
                _dc.Articoli.Add(articolo);
                await _dc.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }

            return CreatedAtAction(nameof(Add), new { id = articolo.Id }, articolo.ToArticoloModel());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ArticoloModel>> Update(int id, ArticoloDTO item)
        {
            var userIdClaim = this.User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return BadRequest();
            }
            var userId = Convert.ToInt32(userIdClaim.Value);

            var articolo = await _dc.Articoli.FindAsync(id);
            if (articolo == null)
            {
                return NotFound();
            }
            if (articolo.Autore!.Id != userId)
            {
                return Unauthorized();
            }
            var categoria = await _dc.Categorie.FindAsync(item.CategoriaId);
            if (categoria == null)
            {
                return BadRequest("Categoria non trovata");
            }
            var autore = await _dc.Utenti.FindAsync(userId);
            if (autore == null)
            {
                return BadRequest("Autore non trovata");
            }
            articolo.Title = item.Titolo;
            articolo.Testo = item.Testo;
            articolo.Autore = autore;
            articolo.Categoria = categoria;
            articolo.Pubblicato = item.Pubblicato;
            articolo.DataUltimaModifica = DateTime.Now;
            try
            {
                await _dc.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }

            return Ok(articolo.ToArticoloModel());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var articolo = await _dc.Articoli.FindAsync(id);
            if (articolo == null)
            {
                return NotFound();
            }
            _dc.Articoli.Remove(articolo);
            try
            {
                await _dc.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }

            return NoContent();
        }
    }
}
