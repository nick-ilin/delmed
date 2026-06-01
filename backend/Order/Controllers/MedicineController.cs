using Order.Features.Medicines.GetList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Order.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicineController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<MedicineDto>>> GetList()
    {
        var query = new GetMedicineListQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }
}