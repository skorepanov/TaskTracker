using TaskTracker.Web.Models;

namespace TaskTracker.Web.Controllers;

[ApiController]
[Route("api/folders")]
public class FolderController : ControllerBase
{
    private readonly TaskService _taskService;

    public FolderController(TaskService taskService)
    {
        _taskService = taskService;
    }

    // TODO method should returns folder id only
    [HttpPost]
    public async Task<IActionResult> CreateFolder([FromBody] FolderChangeData changeData)
    {
        var folder = await _taskService.CreateFolder(changeData);
        var folderVm = new FolderVm(folder);
        return Ok(folderVm);
    }

    [HttpGet]
    public async Task<IActionResult> GetFolders()
    {
        var folders = await _taskService.GetFolders();
        var folderVms = FolderVm.CreateCollectionFrom(folders);
        return Ok(folderVms);
    }

    [HttpGet]
    [Route("{folderId:int}/incompleteTasks")]
    public async Task<IActionResult> GetIncompleteTasks(int folderId)
    {
        var tasks = await _taskService.GetIncompleteTasks(folderId);
        var taskVms = UserTaskVm.CreateCollectionFrom(tasks, DateTime.Now);
        return Ok(taskVms);
    }

    [HttpGet]
    [Route("{folderId:int}/completedTasks")]
    public async Task<IActionResult> GetCompletedTasks(int folderId)
    {
        var tasks = await _taskService.GetCompletedTasks(folderId);
        var taskVms = UserTaskVm.CreateCollectionFrom(tasks, DateTime.Now);
        return Ok(taskVms);
    }
}
