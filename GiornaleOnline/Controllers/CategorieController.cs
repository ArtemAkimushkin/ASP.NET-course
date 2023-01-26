using GiornaleOnline.DataContext;
using GiornaleOnline.DataContext.Models;
using GiornaleOnline.Extensions;
using GiornaleOnline.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiornaleOnline.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("[controller]")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        private readonly ILogger<CategorieController> _logger;
        private readonly GOContext _dc;
        public CategorieController(ILogger<CategorieController> logger, GOContext dc)
        {
            _logger = logger;
            _dc = dc;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaModel>>> GetAll()
        {
            return Ok(await _dc.Categorie.Select(x => x.ToCategoriaModel()).ToListAsync());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaModel>> GetById(int id)
        {
            var categoria = await _dc.Categorie.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria.ToCategoriaModel());

        }

        [HttpGet("edit/{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetDTOById(int id)
        {
            var categoria = await _dc.Categorie.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            var dto = new CategoriaDTO
            {
                Nome = categoria.Nome
            };
            return Ok(dto);

        }

        [HttpPost]
        public async Task<ActionResult<CategoriaModel>> Add(CategoriaDTO item)
        {
            var categoria =new Categoria 
            {
                Nome = item.Nome
            };

            try
            {
                _dc.Categorie.Add(categoria);
                await _dc.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }

            return CreatedAtAction(nameof(Add), new { id = categoria.Id }, categoria.ToCategoriaModel());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoriaModel>> Update(int id, CategoriaDTO item)
        {
            var categoria = await _dc.Categorie.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            categoria.Nome = item.Nome;
            try
            {
                await _dc.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }

            return Ok(categoria.ToCategoriaModel());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var categoria = await _dc.Categorie.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            _dc.Categorie.Remove(categoria);
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
