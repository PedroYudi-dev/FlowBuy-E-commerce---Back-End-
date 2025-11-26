namespace api_ecommerce.Domain.DTOs
{
    public class ReviewDTO
    {
        public int Nota { get; set; } 
        public string Comentario { get; set; }
        public int ClienteId { get; set; }

    }
}
