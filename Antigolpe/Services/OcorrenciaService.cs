using Antigolpe.Data;
using Antigolpe.Models;
using Microsoft.EntityFrameworkCore;

namespace Antigolpe.Services
{
    public class OcorrenciaService
    {
        public void Cadastrar(Ocorrencia ocorrencia)
        {
            using (var db = new AppDbContext())
            {
                db.Ocorrencias.Add(ocorrencia);
                db.SaveChanges();
            }
        }

        public List<Ocorrencia> ListarTodas()
        {
            using (var db = new AppDbContext())
            {
                return db.Ocorrencias
                    .OrderByDescending(o => o.DataRegistro)
                    .ToList();
            }
        }

        public Ocorrencia? BuscarPorId(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Ocorrencias
                    .FirstOrDefault(o => o.Id == id);
            }
        }


        public bool AtualizarStatus(int id, string novoStatus)
        {
            using (var db = new AppDbContext())
            {
                var ocorrencia = db.Ocorrencias
                    .FirstOrDefault(o => o.Id == id);

                if (ocorrencia == null)
                {
                    return false;
                }

                ocorrencia.Status = novoStatus;

                db.SaveChanges();

                return true;
            }
        }


        public bool Excluir(int id)
        {
            using (var db = new AppDbContext())
            {
                var ocorrencia = db.Ocorrencias
                    .FirstOrDefault(o => o.Id == id);

                if (ocorrencia == null)
                {
                    return false;
                }

                db.Ocorrencias.Remove(ocorrencia);

                db.SaveChanges();

                return true;
            }
        }


    }
}
