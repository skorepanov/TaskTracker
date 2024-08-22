namespace TaskTracker.Tests;

public class FolderTests
{
    #region Add task to folder
    [Test]
    public void AddNullTaskToFolder()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var action = () => sut.AddTask(task: null);

        // Assert
        action.Should().Throw<ArgumentNullException>()
                       .WithParameterName(paramName: "task");
        sut.Tasks.Should().BeEmpty();
    }

    [Test]
    public void AddOneTaskToFolder()
    {
        // Arrange
        var task = CreateTask();
        var expectedTasks = new List<UserTask> { task };

        var sut = CreateSut();

        // Act
        sut.AddTask(task);

        // Assert
        sut.Tasks.Should().Equal(expectedTasks);
    }

    [Test]
    public void AddSeveralTasksWithSameIdToFolder()
    {
        // Arrange
        const int TASK_ID = 42;
        var task = CreateTask(TASK_ID);

        var sut = CreateSut();

        // Act & Assert
        sut.AddTask(task);
        sut.Tasks.Should().ContainSingle(t => t.Equals(task));

        var otherTask = CreateTask(TASK_ID);
        sut.AddTask(otherTask);
        sut.Tasks.Should().ContainSingle(t => t.Equals(task));
    }
    #endregion

    #region Get incomplete task count
    [Test]
    public void GetIncompleteTaskCountWhenFolderIsEmpty()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var incompleteTaskCount = sut.IncompleteTaskCount;

        // Assert
        incompleteTaskCount.Should().Be(0);
    }

    [Test]
    public void GetIncompleteTaskCountWhenFolderHasDifferentTasks()
    {
        // Arrange
        var sut = CreateSut();

        var incompleteTask = CreateTask(id: 42_1);

        var completedTask = CreateTask(id: 42_2);
        completedTask.Complete(new DateTime(year: 2022, month: 1, day: 10));

        var deletedTask = CreateTask(id: 42_3);
        deletedTask.Delete(new DateTime(year: 2022, month: 2, day: 20));

        sut.AddTask(incompleteTask);
        sut.AddTask(completedTask);
        sut.AddTask(deletedTask);

        // Act
        var incompleteTaskCount = sut.IncompleteTaskCount;

        // Assert
        incompleteTaskCount.Should().Be(1);
    }
    #endregion

    #region helpers
    private Folder CreateSut(int id = 42, string title = "Folder title 42")
    {
        var folderChangeData = new FolderChangeData(id, title);
        return Folder.CreateFolder(folderChangeData);
    }

    private UserTask CreateTask(int id = 42, string title = "Task title 42",
                                string description = "Description 42")
    {
        var userTaskChangeData = new UserTaskChangeData
        {
            Id = id,
            Title = title,
            Description = description,
        };

        return UserTask.CreateTask(userTaskChangeData);
    }
    #endregion
}
