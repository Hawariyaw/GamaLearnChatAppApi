using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.Interface.Group;
using Server.Entity;

namespace Server.Infrastructure.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "JwtScheme")]
public class GroupController : ControllerBase
{
    private readonly ILogger<GroupController> _logger;

    private readonly IGroupRepository _groupRepository;

    public GroupController(ILogger<GroupController> logger, IGroupRepository groupRepository)
    {
        _logger = logger;
        _groupRepository = groupRepository;
    }

    [HttpPost("Group")]
    public async Task<GroupConnection> Create(GroupConnection group)
    {
        return await _groupRepository.CreateGroup(group);
    }

    [HttpGet("Group/Groups")]
    public async Task<IEnumerable<GroupConnection?>> GetAll()
    {
        return await _groupRepository.GetGroups();
    }

    [HttpGet("Group/{Id}")]
    public async Task<GroupConnection?> Get(Guid Id)
    {
        return await _groupRepository.GetGroup(Id);
    }
}
