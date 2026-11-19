using Antigolpe.Models;
using Microsoft.EntityFrameworkCore;

namespace Antigolpe.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Ocorrencia> Ocorrencias => Set<Ocorrencia>();

        public static string CaminhoBanco
        {
            get
            {
                string pasta = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Antigolpe");

                Directory.CreateDirectory(pasta);
                string destino = Path.Combine(pasta, "antigolpe.db");

                // Migra automaticamente o banco criado pela versão Console,
                // quando ele ainda está na pasta do projeto.
                if (!File.Exists(destino))
                {
                    string[] candidatos =
                    {
                        Path.Combine(AppContext.BaseDirectory, "antigolpe.db"),
                        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
                            "..", "..", "..", "..", "Antigolpe", "antigolpe.db"))
                    };

                    string? legado = candidatos.FirstOrDefault(File.Exists);
                    if (legado != null && !Path.GetFullPath(legado).Equals(
                        Path.GetFullPath(destino), StringComparison.OrdinalIgnoreCase))
                    {
                        File.Copy(legado, destino, overwrite: false);
                    }
                }

                return destino;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite($"Data Source={CaminhoBanco}");
        }
    }
}
