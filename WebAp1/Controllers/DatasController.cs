using Application.LogicInterfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebAp1.Controllers;

[ApiController]
[Route("[controller]")]
public class DatasController : ControllerBase
{
    private readonly IDataLogic dataLogic;

    public DatasController(IDataLogic dataLogic)
    {
        this.dataLogic = dataLogic;
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetByMachineIdAsync([FromRoute] int id)
    {
        try
        {
            MachineErrorDto result = await dataLogic.GetByMachineIdAsync(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    
    
}