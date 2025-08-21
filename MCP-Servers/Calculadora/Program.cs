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
   .WithSummary("Soma dois números")
   .WithDescription("Adiciona dois números e retorna o resultado");

app.MapPost("/api/subtrair", (SubtrairRequest request) => new { result = request.A - request.B })
   .WithName("SubtrairNumeros")
   .WithSummary("Subtrai dois números")
   .WithDescription("Subtrai o segundo número do primeiro e retorna o resultado");

app.MapPost("/api/multiplicar", (MultiplicarRequest request) => new { result = request.A * request.B })
   .WithName("MultiplicarNumeros")
   .WithSummary("Multiplica dois números")
   .WithDescription("Multiplica dois números e retorna o resultado");

app.MapPost("/api/dividir", (DividirRequest request) => 
    request.B != 0 ? Results.Ok(new { result = (double)request.A / request.B }) 
                   : Results.BadRequest(new { error = "Divisão por zero não é permitida" }))
   .WithName("DividirNumeros")
   .WithSummary("Divide dois números")
   .WithDescription("Divide o primeiro número pelo segundo e retorna o resultado");

app.MapPost("/api/raiz-quadrada", (RaizQuadradaRequest request) => 
    request.Numero >= 0 ? Results.Ok(new { result = Math.Sqrt(request.Numero) }) 
                        : Results.BadRequest(new { error = "Não é possível calcular raiz quadrada de número negativo" }))
   .WithName("RaizQuadrada")
   .WithSummary("Calcula a raiz quadrada")
   .WithDescription("Calcula a raiz quadrada de um número");

app.MapPost("/api/potencia", (PotenciaRequest request) => new { result = Math.Pow(request.Base, request.Expoente) })
   .WithName("Potencia")
   .WithSummary("Eleva um número a uma potência")
   .WithDescription("Eleva um número base a uma potência específica");

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
    [McpServerTool, Description("Adicione dois números entre eles")]
    public async Task<string> SomarNumeros(
        [Description("o primeiro número")] int a,
        [Description("o segundo número")] int b)
    {
        return await Task.FromResult((a + b).ToString());
    }

    [McpServerTool, Description("Subtraia dois números entre eles")]
    public async Task<string> SubtraiaNumeros(
        [Description("o primeiro número")] int a,
        [Description("o segundo número")] int b)
    {
        return await Task.FromResult((a - b).ToString()); // Fixed: was (a + b), now (a - b)
    }

    [McpServerTool, Description("Multiplique dois números entre eles")]
    public async Task<string> MultipliqueNumeros(
        [Description("o primeiro número")] int a,
        [Description("o segundo número")] int b)
    {
        return await Task.FromResult((a * b).ToString());
    }

    [McpServerTool, Description("Divida dois números entre eles")]
    public async Task<string> DividaNumeros(
        [Description("o primeiro número")] int a,
        [Description("o segundo número")] int b)
    {
        if (b == 0)
            return await Task.FromResult("Erro: Divisão por zero");
        return await Task.FromResult(((double)a / b).ToString());
    }

    [McpServerTool, Description("Extrai a raiz quadrada de um número")]
    public async Task<string> RaizQuadrada(
        [Description("o número")] int a)
    {
        if (a < 0)
            return await Task.FromResult("Erro: Não é possível calcular raiz quadrada de número negativo");
        return await Task.FromResult(Math.Sqrt(a).ToString());
    }

    [McpServerTool, Description("Eleva um número a uma potência")]
    public async Task<string> Potencia(
        [Description("o número base")] int baseNum,
        [Description("o expoente")] int expoente)
    {
        return await Task.FromResult(Math.Pow(baseNum, expoente).ToString());
    }
}