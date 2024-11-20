namespace WebApiDarper.Models
{
    public class ResponseModel<T>
    {
        public T? Dados { get; set; }
        public string Memsagem { get; set; } = string.Empty;

        public bool Status { get; set; } = true;
    }
}
