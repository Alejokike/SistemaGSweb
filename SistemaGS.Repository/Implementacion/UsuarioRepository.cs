using Microsoft.EntityFrameworkCore;
using SistemaGS.Model;
using SistemaGS.Repository.Contrato;
using SistemaGS.Repository.DBContext;

namespace SistemaGS.Repository.Implementacion
{
    public class UsuarioRepository : GenericoRepository<Usuario>, IUsuarioRepository
    {
        private readonly DbsistemaGsContext _dbContext;

        public UsuarioRepository(DbsistemaGsContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Editar(Usuario usuario, Persona persona)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (await _dbContext.Usuarios.AnyAsync(u => u.Cedula == usuario.Cedula) || await _dbContext.Personas.AnyAsync(p => p.Cedula == persona.Cedula))
                    {
                        _dbContext.Personas.Update(persona);
                        await _dbContext.SaveChangesAsync();
                        _dbContext.Usuarios.Update(usuario);
                        await _dbContext.SaveChangesAsync();

                        transaction.Commit();
                    }
                    else throw new TaskCanceledException("Este usuario no existe en el sistema");

                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                    throw;
                }
            }
        }

        public async Task<bool> Eliminar(int Cedula)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (await _dbContext.Usuarios.AnyAsync(u => u.Cedula == Cedula) || await _dbContext.Personas.AnyAsync(p => p.Cedula == Cedula))
                    {
                        if (await _dbContext.Ayuda.AnyAsync(a => a.Funcionario == Cedula || a.Solicitante == Cedula)) 
                            throw new InvalidOperationException("El usuario seleccionado ya esta referenciado en una ayuda");

                        Persona auxP = _dbContext.Personas.Where(p => p.Cedula == Cedula).First();
                        Usuario auxU = _dbContext.Usuarios.Where(u => u.Cedula == Cedula).First();

                        _dbContext.Personas.Remove(auxP);                        
                        await _dbContext.SaveChangesAsync();
                        _dbContext.Usuarios.Remove(auxU);
                        await _dbContext.SaveChangesAsync();

                        transaction.Commit();
                    }
                    else throw new TaskCanceledException("No existe este usuario en sistema");
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                    throw;
                }
            }
        }

        public async Task<bool> Registrar(Usuario usuario, Persona persona)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (await _dbContext.Usuarios.AnyAsync(u => u.Cedula == usuario.Cedula) || await _dbContext.Personas.AnyAsync(p => p.Cedula == persona.Cedula))
                    {
                        await _dbContext.AddAsync(usuario);
                        await _dbContext.SaveChangesAsync();
                        await _dbContext.AddAsync(persona); 
                        await _dbContext.SaveChangesAsync();

                        transaction.Commit();
                    }
                    else throw new TaskCanceledException("Ya existe este usuario en sistema");

                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                    throw;
                }
            }
        }
    }
}
