using TaskTracker.Bll;

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

    [HttpPost]
    public IActionResult CreateFolder([FromBody] FolderChangeData changeData)
    {
        var folder = _taskService.CreateFolder(changeData);
        return Ok(folder);
    }

    [HttpGet]
    public IActionResult GetFolders()
    {
        var folders = _taskService.GetFolders();
        var folderVms = FolderVm.CreateCollectionFrom(folders);
        return Ok(folderVms);
    }

    [HttpGet]
    [Route("{folderId:int}/incompleteTasks")]
    public IActionResult GetIncompleteTasks(int folderId)
    {
        var tasks = _taskService.GetIncompleteTasks(folderId);
        var taskVms = UserTaskVm.CreateCollectionFrom(tasks, DateTime.Now);
        return Ok(taskVms);
    }

    [HttpGet]
    [Route("{folderId:int}/completedTasks")]
    public IActionResult GetCompletedTasks(int folderId)
    {
        var tasks = _taskService.GetCompletedTasks(folderId);
        var taskVms = UserTaskVm.CreateCollectionFrom(tasks, DateTime.Now);
        return Ok(taskVms);
    }
}
