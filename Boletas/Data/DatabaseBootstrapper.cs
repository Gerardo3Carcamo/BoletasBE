using Boletas.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Boletas.Data
{
    public static class DatabaseBootstrapper
    {
        public static async Task ResetAsync(BoletasDbContext context, CancellationToken cancellationToken = default)
        {
            context.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));
            Console.WriteLine("Eliminando tablas actuales...");
            await DropAllTablesAsync(context, cancellationToken);
            Console.WriteLine("Recreando esquema desde entidades EF...");
            await BootstrapAsync(context, cancellationToken);
        }

        public static async Task BootstrapAsync(BoletasDbContext context, CancellationToken cancellationToken = default)
        {
            await context.Database.EnsureCreatedAsync(cancellationToken);

            if (!await context.Marcas.AnyAsync(cancellationToken))
            {
                await context.Marcas.AddRangeAsync(
                    [
                    new Marca { Nombre = "Toyota", Imagen = "assets/imgs/toyota.png" },
                    new Marca { Nombre = "Nissan", Imagen = "assets/imgs/nissan.png" },
                    new Marca { Nombre = "Chevrolet", Imagen = "assets/imgs/Chevrolet.png" },
                    new Marca { Nombre = "Ford", Imagen = "assets/imgs/ford.png" },
                    ],
                    cancellationToken);
            }

            if (!await context.UsuariosSistema.AnyAsync(cancellationToken))
            {
                await context.UsuariosSistema.AddRangeAsync(
                    [
                    new UsuarioSistema { Nombres = "Gerardo", Apellidos = "Chavez", Usuario = "CHAVEZG", Pass = "123456", Rol = 1 },
                    new UsuarioSistema { Nombres = "Mariana", Apellidos = "Soto", Usuario = "SOTOM", Pass = "123456", Rol = 2 },
                    new UsuarioSistema { Nombres = "Luis", Apellidos = "Trevino", Usuario = "TREVINOL", Pass = "123456", Rol = 2 },
                    new UsuarioSistema { Nombres = "Ruben", Apellidos = "Perez", Usuario = "PEREZR", Pass = "123456", Rol = 3 },
                    ],
                    cancellationToken);
            }

            if (!await context.Operadores.AnyAsync(cancellationToken))
            {
                await context.Operadores.AddRangeAsync(
                    [
                    new Operador { Nombre = "Gerardo Chavez" },
                    new Operador { Nombre = "Mariana Soto" },
                    new Operador { Nombre = "Luis Trevino" },
                    new Operador { Nombre = "Ruben Perez" },
                    ],
                    cancellationToken);
            }

            if (!await context.Plantas.AnyAsync(cancellationToken))
            {
                await context.Plantas.AddRangeAsync(
                    [
                    new Planta { Nombre = "Planta Derramadero", NombreContacto = "Oziel Molina", NumeroContacto = "8443800905" },
                    new Planta { Nombre = "Planta Ramos Arizpe", NombreContacto = "Andrea Salas", NumeroContacto = "8441002200" },
                    new Planta { Nombre = "Planta Saltillo Norte", NombreContacto = "Diego Flores", NumeroContacto = "8442201100" },
                    ],
                    cancellationToken);
            }

            if (!await context.Traslados.AnyAsync(cancellationToken))
            {
                await context.Traslados.AddRangeAsync(
                    [
                    new Traslado { Tipo = "Entrada" },
                    new Traslado { Tipo = "Salida" },
                    ],
                    cancellationToken);
            }

            if (!await context.Turnos.AnyAsync(cancellationToken))
            {
                await context.Turnos.AddRangeAsync(
                    [
                    new Turno { Descripcion = "Primero" },
                    new Turno { Descripcion = "Segundo" },
                    new Turno { Descripcion = "Tercero" },
                    new Turno { Descripcion = "Tiempo extra" },
                    ],
                    cancellationToken);
            }

            if (!await context.Unidades.AnyAsync(cancellationToken))
            {
                await context.Unidades.AddRangeAsync(
                    [
                    new Unidad { NumeroUnidad = "TTN-021", Marca = "Nissan", Modelo = "Urvan 2024" },
                    new Unidad { NumeroUnidad = "TTN-034", Marca = "Toyota", Modelo = "Hiace 2023" },
                    new Unidad { NumeroUnidad = "TTN-051", Marca = "Chevrolet", Modelo = "Onix 2022" },
                    new Unidad { NumeroUnidad = "TTN-064", Marca = "Ford", Modelo = "Transit 2024" },
                    ],
                    cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);

            if (!await context.Boletas.AnyAsync(cancellationToken))
            {
                var boleta = new Boleta
                {
                    IdTraslado = await context.Traslados.Select(x => x.Id).FirstAsync(cancellationToken),
                    IdTurno = await context.Turnos.Select(x => x.Id).FirstAsync(cancellationToken),
                    IdUnidad = await context.Unidades.Select(x => x.Id).FirstAsync(cancellationToken),
                    IdOperador = await context.Operadores.Select(x => x.Id).FirstAsync(cancellationToken),
                    IdPlanta = await context.Plantas.Select(x => x.Id).FirstAsync(cancellationToken),
                    TsCarga = DateTime.UtcNow,
                    HoraEntrada = "07:00",
                    HoraSalida = "07:45",
                    NombreSupervisor = "Gerardo Chavez",
                    NombreSupervisorPlanta = "Oziel Molina",
                    Folio = "G11111"
                };

                await context.Boletas.AddAsync(boleta, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            if (!await context.BoletaUsuarios.AnyAsync(cancellationToken))
            {
                var boletaId = await context.Boletas.Select(x => x.Id).FirstAsync(cancellationToken);

                await context.BoletaUsuarios.AddRangeAsync(
                    [
                    new BoletaUsuario { IdBoleta = boletaId, Nomina = "10001", NombreUsuario = "Andrea Rios", Direccion = "Saltillo Centro" },
                    new BoletaUsuario { IdBoleta = boletaId, Nomina = "10002", NombreUsuario = "Carlos Mendez", Direccion = "Ramos Arizpe" },
                    new BoletaUsuario { IdBoleta = boletaId, Nomina = "10003", NombreUsuario = "Diana Flores", Direccion = "Derramadero" },
                    ],
                    cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);

            Console.WriteLine("Bootstrap MySQL completado. Conteos actuales:");
            Console.WriteLine($"- Marca: {await context.Marcas.CountAsync(cancellationToken)}");
            Console.WriteLine($"- Usuario: {await context.UsuariosSistema.CountAsync(cancellationToken)}");
            Console.WriteLine($"- Operador: {await context.Operadores.CountAsync(cancellationToken)}");
            Console.WriteLine($"- Planta: {await context.Plantas.CountAsync(cancellationToken)}");
            Console.WriteLine($"- Traslado: {await context.Traslados.CountAsync(cancellationToken)}");
            Console.WriteLine($"- Turno: {await context.Turnos.CountAsync(cancellationToken)}");
            Console.WriteLine($"- Unidad: {await context.Unidades.CountAsync(cancellationToken)}");
            Console.WriteLine($"- Boleta: {await context.Boletas.CountAsync(cancellationToken)}");
            Console.WriteLine($"- BoletaUsuario: {await context.BoletaUsuarios.CountAsync(cancellationToken)}");
        }

        private static async Task DropAllTablesAsync(BoletasDbContext context, CancellationToken cancellationToken)
        {
            var connection = context.Database.GetDbConnection();
            var shouldClose = connection.State != System.Data.ConnectionState.Open;

            if (shouldClose)
            {
                await connection.OpenAsync(cancellationToken);
            }

            try
            {
                var tableNames = new List<string>();

                await using (var listCommand = connection.CreateCommand())
                {
                    listCommand.CommandText = """
                                              SELECT table_name
                                              FROM information_schema.tables
                                              WHERE table_schema = DATABASE()
                                                AND table_type = 'BASE TABLE';
                                              """;

                    await using var reader = await listCommand.ExecuteReaderAsync(cancellationToken);
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        tableNames.Add(reader.GetString(0));
                    }
                }

                if (tableNames.Count == 0)
                {
                    return;
                }

                await ExecuteNonQueryAsync(connection, "SET FOREIGN_KEY_CHECKS = 0;", cancellationToken);

                foreach (var tableName in tableNames)
                {
                    var escapedName = tableName.Replace("`", "``");
                    await ExecuteNonQueryAsync(connection, $"DROP TABLE IF EXISTS `{escapedName}`;", cancellationToken);
                }

                await ExecuteNonQueryAsync(connection, "SET FOREIGN_KEY_CHECKS = 1;", cancellationToken);
            }
            finally
            {
                if (shouldClose)
                {
                    await connection.CloseAsync();
                }
            }
        }

        private static async Task ExecuteNonQueryAsync(DbConnection connection, string commandText, CancellationToken cancellationToken)
        {
            await using var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandTimeout = (int)TimeSpan.FromMinutes(5).TotalSeconds;
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
