namespace WebApplication2.Models.DTO
{
    public class ResponseDTO
    {
        public string Message { get; set; } = string.Empty;
        public Boolean Status { get; set; }

        public Object? Result {  get; set; }
    }
}
