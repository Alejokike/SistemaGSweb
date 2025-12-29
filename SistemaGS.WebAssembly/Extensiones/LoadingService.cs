namespace SistemaGS.WebAssembly.Extensiones
{
    public class LoadingService
    {
        public event Action? OnChange; public bool IsLoading { get; private set; }
        public void Show() { IsLoading = true; OnChange?.Invoke(); }
        public void Hide() { IsLoading = false; OnChange?.Invoke(); }
    }
}
