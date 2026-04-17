public interface IStorable
{
    // Ahora devuelve un bool: true si se guardó, false si el inventario está lleno
    bool Store(IPickable item); 
}