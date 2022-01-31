using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public IReadOnlyCollection<FolderVm> GetFolders()
    {
        var folders = _taskService.GetFolders();
        var folderVms = FolderVm.CreateCollectionFrom(folders);
        return folderVms;
    }

    [HttpGet]
    [Route("{folderId:int}/incompleteTasks")]
    public IReadOnlyCollection<UserTaskVm> GetIncompleteTasks(int folderId)
    {
        var tasks = _taskService.GetIncompleteTasks(folderId);
        var taskVms = UserTaskVm.CreateCollectionFrom(tasks, DateTime.Now);
        return taskVms;
    }

    [HttpGet]
    [Route("{folderId:int}/completedTasks")]
    public IReadOnlyCollection<UserTaskVm> GetCompletedTasks(int folderId)
    {
        var tasks = _taskService.GetCompletedTasks(folderId);
        var taskVms = UserTaskVm.CreateCollectionFrom(tasks, DateTime.Now);
        return taskVms;
    }
}
