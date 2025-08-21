using System.ComponentModel;
using ModelContextProtocol.Server;
using ModelContextProtocol.Protocol;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Calculadora API", Version = "v1" });
});

builder.Services
       .AddMcpServer()
       .WithHttpTransport()
       .WithTools<Tools>();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Calculadora API v1"));


app.MapMcp();

// Add API endpoints for OpenAPI documentation
app.MapPost("/api/somar", (SomarRequest request) => new { result = request.A + request.B })
   .WithName("SomarNumeros")
   .WithSummary("Soma dois n�meros")
   .WithDescription("Adiciona dois n�meros e retorna o resultado");

app.MapPost("/api/subtrair", (SubtrairRequest request) => new { result = request.A - request.B })
   .WithName("SubtrairNumeros")
   .WithSummary("Subtrai dois n�meros")
   .WithDescription("Subtrai o segundo n�mero do primeiro e retorna o resultado");

app.MapPost("/api/multiplicar", (MultiplicarRequest request) => new { result = request.A * request.B })
   .WithName("MultiplicarNumeros")
   .WithSummary("Multiplica dois n�meros")
   .WithDescription("Multiplica dois n�meros e retorna o resultado");

app.MapPost("/api/dividir", (DividirRequest request) => 
    request.B != 0 ? Results.Ok(new { result = (double)request.A / request.B }) 
                   : Results.BadRequest(new { error = "Divis�o por zero n�o � permitida" }))
   .WithName("DividirNumeros")
   .WithSummary("Divide dois n�meros")
   .WithDescription("Divide o primeiro n�mero pelo segundo e retorna o resultado");

app.MapPost("/api/raiz-quadrada", (RaizQuadradaRequest request) => 
    request.Numero >= 0 ? Results.Ok(new { result = Math.Sqrt(request.Numero) }) 
                        : Results.BadRequest(new { error = "N�o � poss�vel calcular raiz quadrada de n�mero negativo" }))
   .WithName("RaizQuadrada")
   .WithSummary("Calcula a raiz quadrada")
   .WithDescription("Calcula a raiz quadrada de um n�mero");

app.MapPost("/api/potencia", (PotenciaRequest request) => new { result = Math.Pow(request.Base, request.Expoente) })
   .WithName("Potencia")
   .WithSummary("Eleva um n�mero a uma pot�ncia")
   .WithDescription("Eleva um n�mero base a uma pot�ncia espec�fica");

app.Run();

// Request models for OpenAPI
public record SomarRequest(int A, int B);
public record SubtrairRequest(int A, int B);
public record MultiplicarRequest(int A, int B);
public record DividirRequest(int A, int B);
public record RaizQuadradaRequest(double Numero);
public record PotenciaRequest(double Base, double Expoente);

[McpServerToolType]
public sealed class Tools
{
    [McpServerTool, Description("Adicione dois n�meros entre eles")]
    public async Task<string> SomarNumeros(
        [Description("o primeiro n�mero")] int a,
        [Description("o segundo n�mero")] int b)
    {
        return await Task.FromResult((a + b).ToString());
    }

    [McpServerTool, Description("Subtraia dois n�meros entre eles")]
    public async Task<string> SubtraiaNumeros(
        [Description("o primeiro n�mero")] int a,
        [Description("o segundo n�mero")] int b)
    {
        return await Task.FromResult((a - b).ToString()); // Fixed: was (a + b), now (a - b)
    }

    [McpServerTool, Description("Multiplique dois n�meros entre eles")]
    public async Task<string> MultipliqueNumeros(
        [Description("o primeiro n�mero")] int a,
        [Description("o segundo n�mero")] int b)
    {
        return await Task.FromResult((a * b).ToString());
    }

    [McpServerTool, Description("Divida dois n�meros entre eles")]
    public async Task<string> DividaNumeros(
        [Description("o primeiro n�mero")] int a,
        [Description("o segundo n�mero")] int b)
    {
        if (b == 0)
            return await Task.FromResult("Erro: Divis�o por zero");
        return await Task.FromResult(((double)a / b).ToString());
    }

    [McpServerTool, Description("Extrai a raiz quadrada de um n�mero")]
    public async Task<string> RaizQuadrada(
        [Description("o n�mero")] int a)
    {
        if (a < 0)
            return await Task.FromResult("Erro: N�o � poss�vel calcular raiz quadrada de n�mero negativo");
        return await Task.FromResult(Math.Sqrt(a).ToString());
    }

    [McpServerTool, Description("Eleva um n�mero a uma pot�ncia")]
    public async Task<string> Potencia(
        [Description("o n�mero base")] int baseNum,
        [Description("o expoente")] int expoente)
    {
        return await Task.FromResult(Math.Pow(baseNum, expoente).ToString());
    }
}