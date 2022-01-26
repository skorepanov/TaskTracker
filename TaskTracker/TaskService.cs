﻿namespace TaskTracker;

public class TaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        this._taskRepository = taskRepository;
    }

    public UserTask CreateTask(string title, string description, int folderId)
    {
        var newTask = new UserTask(title, description);

        if (_taskRepository.GetFolder(folderId) is null)
        {
            throw new DomainEntityNotFoundException(typeof(Folder),
                                                    message: "Папка не обнаружена");
        }

        _taskRepository.SaveNewTask(newTask, folderId);
        return newTask;
    }

    public Folder CreateFolder(string title)
    {
        var newFolder = new Folder(title);
        _taskRepository.SaveNewFolder(newFolder);
        return newFolder;
    }

    public void MoveTaskToOtherFolder(int taskId, int destinationFolderId)
    {
        if (_taskRepository.GetTask(taskId) is null)
        {
            throw new DomainEntityNotFoundException(domainEntityType: typeof(UserTask),
                                                    message: "Задача не обнаружена");
        }

        if (_taskRepository.GetFolder(destinationFolderId) is null)
        {
            throw new DomainEntityNotFoundException(typeof(Folder),
                                                    message: "Папка не обнаружена");
        }

        _taskRepository.UpdateTaskFolder(taskId, destinationFolderId);
    }
}
