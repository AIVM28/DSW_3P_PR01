using DSW_3P_PR01.Models;
using DSW_3P_PR01.Repositories;

namespace DSW_3P_PR01.Services;

public class MisionService : IMisionService
{
    private readonly IMisionRepository _repository;

    public MisionService(IMisionRepository repository)
    {
        _repository = repository;
    }

    // Validar las reglas de negocio de una misión.
    private static void Validar(Mision mision)
    {
        if (string.IsNullOrWhiteSpace(mision.Nombre))
        {
            throw new ArgumentException(
                "El nombre de la misión es obligatorio."
            );
        }

        if (string.IsNullOrWhiteSpace(mision.Dificultad))
        {
            throw new ArgumentException(
                "La dificultad de la misión es obligatoria."
            );
        }

        string dificultad = mision.Dificultad.Trim();

        if (
            !dificultad.Equals("Fácil", StringComparison.OrdinalIgnoreCase)
            && !dificultad.Equals("Media", StringComparison.OrdinalIgnoreCase)
            && !dificultad.Equals("Difícil", StringComparison.OrdinalIgnoreCase)
        )
        {
            throw new ArgumentException(
                "La dificultad debe ser Fácil, Media o Difícil."
            );
        }

        if (mision.RecompensaXP < 0)
        {
            throw new ArgumentException(
                "La recompensa XP no puede ser negativa."
            );
        }
    }

    // Obtener todas las misiones.
    public List<Mision> ObtenerTodas()
    {
        return _repository.ObtenerTodas();
    }

    // Obtener una misión por su identificador.
    public Mision? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    // Registrar una nueva misión.
    public Mision Agregar(Mision nuevaMision)
    {
        Validar(nuevaMision);

        // Todas las misiones nuevas comienzan sin completar.
        nuevaMision.Completada = false;

        return _repository.Agregar(nuevaMision);
    }

    // Actualizar una misión existente.
    public bool Actualizar(int id, Mision misionActualizada)
    {
        if (_repository.ObtenerPorId(id) == null)
        {
            return false;
        }

        Validar(misionActualizada);

        // El identificador de la URL prevalece sobre el del cuerpo.
        misionActualizada.Id = id;

        return _repository.Actualizar(id, misionActualizada);
    }

    // Eliminar una misión existente.
    public bool Eliminar(int id)
    {
        return _repository.Eliminar(id);
    }
}