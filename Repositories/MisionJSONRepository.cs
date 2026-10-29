using System.Text.Json;
using DSW_3P_PR01.Models;

namespace DSW_3P_PR01.Repositories;

public class MisionJsonRepository : IMisionRepository
{
    private readonly string _ruta;

    private readonly JsonSerializerOptions _opciones = new()
    {
        WriteIndented = true
    };

    public MisionJsonRepository(IWebHostEnvironment environment)
    {
        _ruta = Path.Combine(
            environment.ContentRootPath,
            "Data",
            "misiones.json"
        );

        Directory.CreateDirectory(
            Path.GetDirectoryName(_ruta)!
        );

        if (!File.Exists(_ruta))
        {
            File.WriteAllText(_ruta, "[]");
        }
    }

    // Leer las misiones almacenadas en el archivo JSON.
    private List<Mision> Leer()
    {
        string json = File.ReadAllText(_ruta);

        return JsonSerializer.Deserialize<List<Mision>>(json)
               ?? new List<Mision>();
    }

    // Guardar las misiones en el archivo JSON.
    private void Guardar(List<Mision> datos)
    {
        string json = JsonSerializer.Serialize(datos, _opciones);

        File.WriteAllText(_ruta, json);
    }

    // Obtener todas las misiones.
    public List<Mision> ObtenerTodas()
    {
        return Leer();
    }

    // Buscar una misión por su identificador.
    public Mision? ObtenerPorId(int id)
    {
        return Leer().FirstOrDefault(m => m.Id == id);
    }

    // Registrar una nueva misión.
    public Mision Agregar(Mision nuevaMision)
    {
        var datos = Leer();

        nuevaMision.Id = datos.Count == 0
            ? 1
            : datos.Max(m => m.Id) + 1;

        datos.Add(nuevaMision);

        Guardar(datos);

        return nuevaMision;
    }

    // Actualizar una misión existente.
    public bool Actualizar(int id, Mision misionActualizada)
    {
        var datos = Leer();

        var existente = datos.FirstOrDefault(m => m.Id == id);

        if (existente == null)
        {
            return false;
        }

        existente.Nombre = misionActualizada.Nombre;
        existente.Dificultad = misionActualizada.Dificultad;
        existente.RecompensaXP = misionActualizada.RecompensaXP;
        existente.Completada = misionActualizada.Completada;

        Guardar(datos);

        return true;
    }

    // Eliminar una misión existente.
    public bool Eliminar(int id)
    {
        var datos = Leer();

        var existente = datos.FirstOrDefault(m => m.Id == id);

        if (existente == null)
        {
            return false;
        }

        datos.Remove(existente);

        Guardar(datos);

        return true;
    }
}