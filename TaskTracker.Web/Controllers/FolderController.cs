using Microsoft.AspNetCore.Mvc;

namespace TaskTracker.Web;

[ApiController]
[Route("api/folders")]
public class FolderController : ControllerBase
{
    private readonly TaskService _taskService;

    public FolderController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public List<FolderVm> GetFolders()
    {
        var folders = _taskService.GetFolders();
        var folderVms = folders.Select(f => new FolderVm(f)).ToList();
        return folderVms;
    }
}
