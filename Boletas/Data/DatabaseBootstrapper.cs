using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Boletas.Data
{
    public static class DatabaseBootstrapper
    {
        public static async Task BootstrapAsync(BoletasDbContext context, CancellationToken cancellationToken = default)
        {
            await context.Database.OpenConnectionAsync(cancellationToken);

            await using var connection = (SqlConnection)context.Database.GetDbConnection();

            var script = """
                         IF OBJECT_ID('dbo.Marca', 'U') IS NULL
                         BEGIN
                             CREATE TABLE dbo.Marca
                             (
                                 Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                                 Nombre NVARCHAR(100) NOT NULL,
                                 Imagen NVARCHAR(500) NULL
                             );
                         END;

                         IF OBJECT_ID('dbo.Usuario', 'U') IS NULL
                         BEGIN
                             CREATE TABLE dbo.Usuario
                             (
                                 Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                                 Nombres NVARCHAR(100) NOT NULL,
                                 Apellidos NVARCHAR(100) NOT NULL,
                                 Usuario NVARCHAR(50) NOT NULL,
                                 Pass NVARCHAR(255) NOT NULL,
                                 Rol INT NOT NULL
                             );
                         END;

                         IF NOT EXISTS (SELECT 1 FROM dbo.Marca)
                         BEGIN
                             INSERT INTO dbo.Marca (Nombre, Imagen)
                             VALUES
                                 (N'Toyota', N'assets/imgs/toyota.png'),
                                 (N'Nissan', N'assets/imgs/nissan.png'),
                                 (N'Chevrolet', N'assets/imgs/Chevrolet.png'),
                                 (N'Ford', N'assets/imgs/ford.png');
                         END;

                         IF NOT EXISTS (SELECT 1 FROM dbo.Usuario)
                         BEGIN
                             INSERT INTO dbo.Usuario (Nombres, Apellidos, Usuario, Pass, Rol)
                             VALUES
                                 (N'Gerardo', N'Chavez', N'CHAVEZG', N'123456', 1),
                                 (N'Mariana', N'Soto', N'SOTOM', N'123456', 2),
                                 (N'Luis', N'Trevino', N'TREVINOL', N'123456', 2),
                                 (N'Ruben', N'Perez', N'PEREZR', N'123456', 3);
                         END;

                         IF NOT EXISTS (SELECT 1 FROM dbo.Operador WHERE Nombre = N'Gerardo Chavez')
                             INSERT INTO dbo.Operador (Nombre) VALUES (N'Gerardo Chavez');

                         IF NOT EXISTS (SELECT 1 FROM dbo.Operador WHERE Nombre = N'Mariana Soto')
                             INSERT INTO dbo.Operador (Nombre) VALUES (N'Mariana Soto');

                         IF NOT EXISTS (SELECT 1 FROM dbo.Operador WHERE Nombre = N'Luis Trevino')
                             INSERT INTO dbo.Operador (Nombre) VALUES (N'Luis Trevino');

                         IF NOT EXISTS (SELECT 1 FROM dbo.Operador WHERE Nombre = N'Ruben Perez')
                             INSERT INTO dbo.Operador (Nombre) VALUES (N'Ruben Perez');

                         IF NOT EXISTS (SELECT 1 FROM dbo.Planta WHERE Nombre = N'Planta Derramadero')
                             INSERT INTO dbo.Planta (Nombre, NombreContacto, NumeroContacto)
                             VALUES (N'Planta Derramadero', N'Oziel Molina', N'8443800905');

                         IF NOT EXISTS (SELECT 1 FROM dbo.Planta WHERE Nombre = N'Planta Ramos Arizpe')
                             INSERT INTO dbo.Planta (Nombre, NombreContacto, NumeroContacto)
                             VALUES (N'Planta Ramos Arizpe', N'Andrea Salas', N'8441002200');

                         IF NOT EXISTS (SELECT 1 FROM dbo.Planta WHERE Nombre = N'Planta Saltillo Norte')
                             INSERT INTO dbo.Planta (Nombre, NombreContacto, NumeroContacto)
                             VALUES (N'Planta Saltillo Norte', N'Diego Flores', N'8442201100');

                         IF NOT EXISTS (SELECT 1 FROM dbo.Traslado)
                         BEGIN
                             INSERT INTO dbo.Traslado (Tipo)
                             VALUES
                                 (N'Entrada'),
                                 (N'Salida');
                         END;

                         IF NOT EXISTS (SELECT 1 FROM dbo.Turno)
                         BEGIN
                             INSERT INTO dbo.Turno (Descripcion)
                             VALUES
                                 (N'Primero'),
                                 (N'Segundo'),
                                 (N'Tercero'),
                                 (N'Tiempo extra');
                         END;

                         IF NOT EXISTS (SELECT 1 FROM dbo.Unidad WHERE NumeroUnidad = N'TTN-021')
                             INSERT INTO dbo.Unidad (NumeroUnidad, Marca, Modelo)
                             VALUES (N'TTN-021', N'Nissan', N'Urvan 2024');

                         IF NOT EXISTS (SELECT 1 FROM dbo.Unidad WHERE NumeroUnidad = N'TTN-034')
                             INSERT INTO dbo.Unidad (NumeroUnidad, Marca, Modelo)
                             VALUES (N'TTN-034', N'Toyota', N'Hiace 2023');

                         IF NOT EXISTS (SELECT 1 FROM dbo.Unidad WHERE NumeroUnidad = N'TTN-051')
                             INSERT INTO dbo.Unidad (NumeroUnidad, Marca, Modelo)
                             VALUES (N'TTN-051', N'Chevrolet', N'Onix 2022');

                         IF NOT EXISTS (SELECT 1 FROM dbo.Unidad WHERE NumeroUnidad = N'TTN-064')
                             INSERT INTO dbo.Unidad (NumeroUnidad, Marca, Modelo)
                             VALUES (N'TTN-064', N'Ford', N'Transit 2024');

                         IF NOT EXISTS (SELECT 1 FROM dbo.Boleta)
                         BEGIN
                             INSERT INTO dbo.Boleta
                             (
                                 IdTraslado,
                                 IdTurno,
                                 IdUnidad,
                                 IdOperador,
                                 IdPlanta,
                                 TsCarga,
                                 HoraEntrada,
                                 HoraSalida,
                                 NombreSupervisor,
                                 NombreSupervisorPlanta,
                                 Folio
                             )
                             VALUES
                             (
                                 1,
                                 1,
                                 1,
                                 1,
                                 1,
                                 GETUTCDATE(),
                                 N'07:00',
                                 N'07:45',
                                 N'Gerardo Chavez',
                                 N'Oziel Molina',
                                 N'G11111'
                             );
                         END;

                         IF NOT EXISTS (SELECT 1 FROM dbo.BoletaUsuario)
                         BEGIN
                             INSERT INTO dbo.BoletaUsuario (IdBoleta, Nomina, NombreUsuario, Direccion)
                             VALUES
                                 (1, N'10001', N'Andrea Rios', N'Saltillo Centro'),
                                 (1, N'10002', N'Carlos Mendez', N'Ramos Arizpe'),
                                 (1, N'10003', N'Diana Flores', N'Derramadero');
                         END;
                         """;

            await using (var command = new SqlCommand(script, connection))
            {
                command.CommandTimeout = 120;
                await command.ExecuteNonQueryAsync(cancellationToken);
            }

            var verificationQuery = """
                                    SELECT 'Marca' AS Tabla, COUNT(*) AS Total FROM dbo.Marca
                                    UNION ALL
                                    SELECT 'Usuario', COUNT(*) FROM dbo.Usuario
                                    UNION ALL
                                    SELECT 'Operador', COUNT(*) FROM dbo.Operador
                                    UNION ALL
                                    SELECT 'Planta', COUNT(*) FROM dbo.Planta
                                    UNION ALL
                                    SELECT 'Traslado', COUNT(*) FROM dbo.Traslado
                                    UNION ALL
                                    SELECT 'Turno', COUNT(*) FROM dbo.Turno
                                    UNION ALL
                                    SELECT 'Unidad', COUNT(*) FROM dbo.Unidad
                                    UNION ALL
                                    SELECT 'Boleta', COUNT(*) FROM dbo.Boleta
                                    UNION ALL
                                    SELECT 'BoletaUsuario', COUNT(*) FROM dbo.BoletaUsuario;
                                    """;

            await using var verification = new SqlCommand(verificationQuery, connection);
            await using var reader = await verification.ExecuteReaderAsync(cancellationToken);

            Console.WriteLine("Bootstrap SQL completado. Conteos actuales:");
            while (await reader.ReadAsync(cancellationToken))
            {
                Console.WriteLine($"- {reader.GetString(0)}: {reader.GetInt32(1)}");
            }
        }
    }
}
