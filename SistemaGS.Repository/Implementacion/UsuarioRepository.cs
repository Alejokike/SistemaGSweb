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
                    if(await _dbContext.Usuarios.AnyAsync(u => u.Cedula == usuario.Cedula) || await _dbContext.Personas.AnyAsync(p => p.Cedula == persona.Cedula))
                    {
                        _dbContext.Usuarios.Update(usuario);
                        await _dbContext.SaveChangesAsync();
                        _dbContext.Personas.Update(persona);
                        await _dbContext.SaveChangesAsync();

                        transaction.Commit();
                    }
                    else throw new TaskCanceledException("No existen registros de ese usuario");
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
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
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
        public async Task<List<(Usuario usuario, Persona persona, Rol rol)>> Listar(int rol, string buscar)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string filtro = (buscar ?? "").ToLower();

                    var lista = from u in _dbContext.Usuarios
                                join p in _dbContext.Personas on u.Cedula equals p.Cedula
                                join r in _dbContext.Rols on u.IdRol equals r.IdRol
                                where
                                (rol == 0 || r.IdRol.Equals(rol)) &&
                                (string.IsNullOrEmpty(filtro) ||
                                    EF.Functions.Like((p.Cedula.ToString() ?? "").ToLower(), filtro) ||
                                    EF.Functions.Like((p.Nombre ?? "").ToLower(), filtro) ||
                                    EF.Functions.Like((p.Apellido ?? "").ToLower(), filtro) ||
                                    EF.Functions.Like((p.TelefonoTrabajo ?? "").ToLower(), filtro) ||
                                    EF.Functions.Like((p.TelefonoHabitacion ?? "").ToLower(), filtro))
                                select new
                                {
                                    usuario = u,
                                    persona = p,
                                    rol = r
                                };

                    List<(Usuario usuario, Persona persona, Rol rol)> temp = new List<(Usuario usuario, Persona persona, Rol rol)>(); 
                    await lista.AsNoTracking().ForEachAsync(t => 
                    {
                        (Usuario usuario, Persona persona, Rol rol) x;
                        x.usuario = t.usuario;
                        x.persona = t.persona;
                        x.rol = t.rol;
                        temp.Add(x);
                    });
                    return temp;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
        public async Task<(Usuario usuario, Persona persona, Rol rol)> Obtener(int IdUsuario)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var consulta =
                         await (from u in _dbContext.Usuarios
                                join p in _dbContext.Personas on u.Cedula equals p.Cedula
                                join r in _dbContext.Rols on u.IdRol equals r.IdRol
                                where u.Cedula == IdUsuario
                                select new
                                {
                                    u,
                                    p,
                                    r
                                }).AsNoTracking().FirstOrDefaultAsync();
                    (Usuario usuario, Persona persona, Rol rol) t = (usuario: new Usuario(), persona: new Persona(), rol: new Rol());
                    if (consulta != null)
                    {
                        t = (usuario: consulta.u, persona: consulta.p, rol: consulta.r);
                        return t;
                    }                    
                    return t;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
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
                    if (!await _dbContext.Rols.AnyAsync(r => r.IdRol == usuario.IdRol)) throw new TaskCanceledException("El rol seleccionado no existe");
                    if (await _dbContext.Usuarios.AnyAsync(u => u.NombreUsuario == usuario.NombreUsuario)) throw new TaskCanceledException("Ya existe ese nombre de usuario");
                    if (await _dbContext.Usuarios.AnyAsync(u => u.Correo == usuario.Correo)) throw new TaskCanceledException("Ya ese correo esta en uso");

                    if (!await _dbContext.Usuarios.AnyAsync(u => u.Cedula == usuario.Cedula))
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
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
    }
}
