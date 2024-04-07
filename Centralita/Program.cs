using System.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Alejandro Gomez (23-0573)\n\n---------------------Centralita Telefonica---------------------\n");
        Console.WriteLine(" Reporte de llamadas:\n");

        string connectionString = "Server=tcp:centralitaserver.database.windows.net,1433;Initial Catalog=Centralita;Persist Security Info=False;User ID=Administrador;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        Centralita centralita = new Centralita(connectionString);

        centralita.AgregarLlamada(new LlamadaLocal("767772", "185436", 104, 0.15));
        centralita.AgregarLlamada(new LlamadaLocal("033398", "761655", 3310, 0.15));
        centralita.AgregarLlamada(new LlamadaLocal("148633", "676363", 301, 0.15));

        centralita.AgregarLlamada(new LlamadaProvincial("822487", "198713", 152, 0.15, 0.20, 0.25, 5));
        centralita.AgregarLlamada(new LlamadaProvincial("127634", "776536", 1708, 0.15, 0.20, 0.25, 5));
        centralita.AgregarLlamada(new LlamadaProvincial("315777", "4543134", 340, 0.15, 0.20, 0.25, 5));

        double totalLocal = centralita.CalcularTotalLocal();
        double totalProvincial = centralita.CalcularTotalProvincial();

        Console.WriteLine($"    Hubo un total de {centralita.Acumulador} llamadas. \n");
        Console.WriteLine($"    Total de llamadas locales: {totalLocal:C}");
        Console.WriteLine($"    Total de llamadas provinciales: {totalProvincial:C}");
        Console.WriteLine();
        Console.WriteLine($"    Total de todas las llamadas: {totalLocal + totalProvincial:C}\n\n\n\n");
    }
}

public abstract class Llamada
{
    protected string numOrigen;
    protected string numDestino;

    public Llamada(string numOrigen, string numDestino)
    {
        this.numOrigen = numOrigen;
        this.numDestino = numDestino;
    }

    public abstract double CalcularPrecio();
}

public class LlamadaProvincial : Llamada
{
    private double duracion;
    private double precio1;
    private double precio2;
    private double precio3;
    private int franja;

    public LlamadaProvincial(string numOrigen, string numDestino, double duracion, double precio1, double precio2, double precio3, int franja) : base(numOrigen, numDestino)
    {
        this.duracion = duracion;
        this.precio1 = precio1;
        this.precio2 = precio2;
        this.precio3 = precio3;
        this.franja = franja;
    }

    public override double CalcularPrecio()
    {
        if (duracion <= franja)
        {
            return duracion * precio1;
        }
        else if (duracion <= franja * 2)
        {
            return duracion * precio2;
        }
        else
        {
            return duracion * precio3;
        }
    }
}

public class LlamadaLocal : Llamada
{
    private double duracion;
    private double precio;

    public LlamadaLocal(string numOrigen, string numDestino, double duracion, double precio) : base(numOrigen, numDestino)
    {
        this.duracion = duracion;
        this.precio = precio;
    }

    public override double CalcularPrecio()
    {
        return duracion * precio;
    }
}

public class Centralita
{
    private List<Llamada> llamadas;
    private string connectionString;

    public int Acumulador { get; private set; }

    public Centralita(string connectionString)
    {
        this.connectionString = connectionString;
        llamadas = new List<Llamada>();
        Acumulador = 0;
    }

    public void AgregarLlamada(Llamada llamada)
    {
        llamadas.Add(llamada);
        Acumulador++;
    }

    public double CalcularTotalLocal()
    {
        double total = 0;

        foreach (var llamada in llamadas)
        {
            if (llamada is LlamadaLocal)
            {
                total += llamada.CalcularPrecio();
            }
        }

        return total;
    }

    public double CalcularTotalProvincial()
    {
        double total = 0;

        foreach (var llamada in llamadas)
        {
            if (llamada is LlamadaProvincial)
            {
                total += llamada.CalcularPrecio();
            }
        }

        return total;
    }
}
