using API_MOV_ALM.Models;


namespace Services.Repository.Interface
{
    public interface IUsuarioRepository<T> where T : class
    {
        Task<List<T>> Buscar(T clase);
        Task<List<T>> Lista();
        Task<bool> Guardar(T clase);
        Task<bool> Editar(T clase);
        Task<bool> Eliminar(int id);
        Task<T> IniciarSesion(T clase);

      

    }
}
