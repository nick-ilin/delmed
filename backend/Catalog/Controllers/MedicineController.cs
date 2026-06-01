using Catalog.Features.Medicines.Create;
using Catalog.Features.Medicines.Delete;
using Catalog.Features.Medicines.Get;
using Catalog.Features.Medicines.GetList;
using Catalog.Features.Medicines.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicineController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<MedicineDto>>> GetList()
    {
        var result = await mediator.Send(new GetMedicineListQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetMedicineResponse>> GetById(int id)
    {
        var query = new GetMedicineQuery(id);
        var result = await mediator.Send(query);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CreateMedicineResponse>> Create(CreateMedicineCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateMedicineResponse>> Update(int id, UpdateMedicineCommand command)
    {
        var result = await mediator.Send(command);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await mediator.Send(new DeleteMedicineCommand(id));

        if (!result)
            return NotFound();

        return NoContent();
    }
}