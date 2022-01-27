using Microsoft.AspNetCore.Mvc;

namespace TaskTracker.Web;

[ApiController]
[Route("api/folders")]
public class FolderController : ControllerBase
{
    [HttpGet]
    public List<FolderVm> GetFolders()
    {
        // TODO: delete test data. Get folders from TaskService
        var testTask = new UserTask(title: "Test task", description: "Test description");
        var testDomainFolder = new Folder(title: "Test folder");
        testDomainFolder.AddTask(testTask);
        var folder = new FolderVm(testDomainFolder);

        return new List<FolderVm> { folder };
    }
}
