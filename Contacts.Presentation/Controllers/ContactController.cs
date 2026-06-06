using Contacts.Application.DTOs;
using Contacts.Application.Interfaces.UseCases;
using Contacts.Application.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Presentation.Controllers;

[ApiController]
[Route("api/contacts")]
public class ContactController(IContactUseCase contactUseCase)
    : ControllerBase
{
    [HttpGet("getAllContacts")]
    public async Task<IActionResult> GetList(
        [FromQuery] ContactQueryParameters parameters,
        CancellationToken cancellationToken)
    {
        var contacts = await contactUseCase.GetAllAsync(parameters, cancellationToken);
        return Ok(contacts);
    }

    [HttpGet("getContactById/{contactId:int}")]
    public async Task<IActionResult> GetById(int contactId, CancellationToken cancellationToken)
    {
        var contact = await contactUseCase.GetByIdAsync(contactId, cancellationToken);
        return Ok(contact);
    }

    [HttpPost("addContact")]
    public async Task<IActionResult> Create(
        [FromBody] ContactDto contactDto,
        CancellationToken cancellationToken)
    {
        await contactUseCase.CreateAsync(contactDto, cancellationToken);
        return NoContent();
    }

    [HttpPut("updateContact/{contactId:int}")]
    public async Task<IActionResult> Update(
        int contactId,
        [FromBody] ContactDto contactDto,
        CancellationToken cancellationToken)
    {
        await contactUseCase.UpdateAsync(contactId, contactDto, cancellationToken);
        return Ok();
    }

    [HttpDelete("deleteContact/{contactId:int}")]
    public async Task<IActionResult> Delete(int contactId, CancellationToken cancellationToken)
    {
        await contactUseCase.DeleteAsync(contactId, cancellationToken);
        return Ok();
    }
}