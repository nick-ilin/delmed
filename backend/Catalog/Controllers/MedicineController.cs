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
    /// <summary>
    /// Получить список лекарств
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(MedicineDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<MedicineDto>>> GetList()
    {
        var result = await mediator.Send(new GetMedicineListQuery());
        return Ok(result);
    }

    /// <summary>
    /// Получить лекарство по ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetMedicineResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetMedicineResponse>> GetById(int id)
    {
        var query = new GetMedicineQuery(id);
        var result = await mediator.Send(query);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Создать новое лекарство
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateMedicineResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CreateMedicineResponse>> Create(CreateMedicineCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновить лекарство
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UpdateMedicineResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateMedicineResponse>> Update(int id, UpdateMedicineCommand command)
    {
        var result = await mediator.Send(command);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Удалить лекарство
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await mediator.Send(new DeleteMedicineCommand(id));

        if (!result)
            return NotFound();

        return NoContent();
    }
}