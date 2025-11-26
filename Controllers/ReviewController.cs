using api_ecommerce.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly ReviewService _service;

    public ReviewController(ReviewService service)
    {
        _service = service;
    }

    [HttpPost("{produtoId}")]
    public async Task<IActionResult> AddReview(int produtoId, [FromBody] ReviewDTO dto)
    {
        await _service.AddReviewAsync(produtoId, dto);
        return Ok("Avaliação adicionada com sucesso!");
    }

    [HttpGet("produto/{produtoId}")]
    public async Task<IActionResult> GetReviews(int produtoId)
    {
        var reviews = await _service.GetReviewsByProductAsync(produtoId);
        return Ok(reviews);
    }

    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> DeleteReview(int reviewId, [FromQuery] int clienteId)
    {
        try
        {
            await _service.DeleteReviewAsync(reviewId, clienteId);
            return Ok(new { message = "Avaliação removida com sucesso." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
