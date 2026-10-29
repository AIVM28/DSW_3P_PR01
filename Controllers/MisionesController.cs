using Microsoft.AspNetCore.Mvc;
using DSW_3P_PR01.Models;
using DSW_3P_PR01.Services;

namespace DSW_3P_PR01.Controllers;

[ApiController]
[Route("api/misiones")]
public class MisionesController : ControllerBase
{
    private readonly IMisionService _service;

    public MisionesController(IMisionService service)
    {
        _service = service;
    }

    // GET: api/misiones
    // Obtener todas las misiones.
    [HttpGet]
    [ProducesResponseType(typeof(List<Mision>), StatusCodes.Status200OK)]
    public IActionResult ObtenerTodas()
    {
        return Ok(_service.ObtenerTodas());
    }

    // GET: api/misiones/1
    // Obtener una misión por su identificador.
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Mision), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ObtenerPorId(int id)
    {
        var mision = _service.ObtenerPorId(id);

        if (mision == null)
        {
            return NotFound(new
            {
                mensaje = "La misión no existe."
            });
        }

        return Ok(mision);
    }

    // POST: api/misiones
    // Registrar una nueva misión.
    [HttpPost]
    [ProducesResponseType(typeof(Mision), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Agregar([FromBody] Mision nuevaMision)
    {
        try
        {
            var creada = _service.Agregar(nuevaMision);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = creada.Id },
                creada
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // PUT: api/misiones/1
    // Actualizar una misión existente.
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Actualizar(
        int id,
        [FromBody] Mision misionActualizada)
    {
        try
        {
            if (!_service.Actualizar(id, misionActualizada))
            {
                return NotFound(new
                {
                    mensaje = "La misión no existe."
                });
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    // DELETE: api/misiones/1
    // Eliminar una misión existente.
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id)
    {
        if (!_service.Eliminar(id))
        {
            return NotFound(new
            {
                mensaje = "La misión no existe."
            });
        }

        return NoContent();
    }
}