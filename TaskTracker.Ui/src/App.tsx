import React from 'react';
import { Space } from 'antd';

import { AppUrl, Api } from './api';
import Task from './components/Task';
import FolderCreationForm from './components/FolderCreationForm';
import TaskCreationForm from './components/TaskCreationForm';
import TaskMovedToTrash from './components/TaskMovedToTrash';
import FolderList from './components/FolderList';
import IFolder from './interfaces/IFolder';
import ITask from './interfaces/ITask';
import ITaskMovedToTrash from './interfaces/ITaskMovedToTrash';

class App extends React.Component<IAppProps, IAppState> {
    constructor(props: IAppProps) {
        super(props);

        this.state = {
            folders: [],
            todayTasks: [],
            tasksInInbox: [],
            tasksMovedToTrash: [],
        };
    }

    loadFolders = async () => {
        const url = `${AppUrl}/folders`;

        const folders = await Api.get<IFolder[]>(url);

        return this.setState({ folders: folders });
    }

    loadFolderTasks = async (folderId: number, isForcedLoad: boolean = false) => {
        const folders = this.state.folders;
        const folder = folders.find(f => f.id === folderId);

        if (folder == null) {
            return;
        }

        if (!isForcedLoad && Array.isArray(folder.tasks) && folder.tasks.length > 0) {
            return;
        }

        const incompleteTasksUrl = `${AppUrl}/folders/${folderId}/incompleteTasks`;
        const incompleteTasks = await Api.get<ITask[]>(incompleteTasksUrl);

        const completedTasksUrl = `${AppUrl}/folders/${folderId}/completedTasks`;
        const completedTasks = await Api.get<ITask[]>(completedTasksUrl);

        folder.tasks = [...incompleteTasks, ...completedTasks];
        folder.incompleteTaskCount = incompleteTasks.length;

        return this.setState({ folders: folders });
    }

    createFolder = async (title: string) => {
        const url = `${AppUrl}/folders`;
        const params = {
            title: title,
            createdDateTime: new Date().toISOString()
        };

        await Api.post<IFolder>(url, params);

        await this.loadFolders();
    }

    createTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks`;
        const params = {
            title: task.title,
            description: task.description,
            dueDateTime: task.dueDateTime,
            folderId: task.folderId,
            createdDateTime: new Date().toISOString()
        };

        await Api.post<ITask>(url, params);

        this.loadFolders();
        this.loadTodayTasks();

        if (task.folderId != null) {
            this.loadFolderTasks(task.folderId, true);
        } else {
            this.loadTasksInInbox();
        }
    }

    loadTodayTasks = async () => {
        const url = `${AppUrl}/tasks/today`;

        const tasks = await Api.get<ITask[]>(url);

        return this.setState({ todayTasks: tasks });
    }

    loadTasksInInbox = async () => {
        const url = `${AppUrl}/tasks/inbox`;

        const tasks = await Api.get<ITask[]>(url);

        return this.setState({ tasksInInbox: tasks });
    }

    loadTasksInTrash = async () => {
        const url = `${AppUrl}/tasks/trash`;

        const tasks = await Api.get<ITaskMovedToTrash[]>(url);

        return this.setState({ tasksMovedToTrash: tasks });
    }

    completeTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/completed`;
        const params = {
            completedDateTime: new Date().toISOString()
        };

        await Api.put<ITask>(url, params);

        if (task.folderId != null) {
            await this.loadFolderTasks(task.folderId, true);
        } else {
            await this.loadTasksInInbox();
        }
    }

    incompleteTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/incompleted`;
        const params = {
            modifiedDateTime: new Date().toISOString()
        };

        await Api.put<ITask>(url, params);

        if (task.folderId != null) {
            await this.loadFolderTasks(task.folderId, true);
        } else {
            await this.loadTasksInInbox();
        }
    }

    componentDidMount() {
        this.loadFolders();
        this.loadTodayTasks();
        this.loadTasksInInbox();
        this.loadTasksInTrash();
    }

    render() {
        return (
            <>
                <Space direction='vertical'>
                    <FolderCreationForm
                        createFolder={this.createFolder}
                    />
                    <TaskCreationForm
                        folders={this.state.folders}
                        createTask={this.createTask}
                    />
                    <FolderList
                        folders={this.state.folders}
                        loadTasks={this.loadFolderTasks}
                        completeTask={this.completeTask}
                        incompleteTask={this.incompleteTask}
                    />
                    <strong>Задачи на сегодня</strong>
                    {
                        this.state.todayTasks.map(t =>
                            <Task
                                key={t.id}
                                task={t}
                                completeTask={this.completeTask}
                                incompleteTask={this.incompleteTask}
                            />
                        )
                    }
                    <strong>Inbox</strong>
                    {
                        this.state.tasksInInbox.map(t =>
                            <Task
                                key={t.id}
                                task={t}
                                completeTask={this.completeTask}
                                incompleteTask={this.incompleteTask}
                            />
                        )
                    }
                    <strong>Корзина</strong>
                    {
                        this.state.tasksMovedToTrash.map(t =>
                            <TaskMovedToTrash
                                key={t.id}
                                task={t}
                            />
                        )
                    }
                </Space>
            </>
        );
    }
}

interface IAppProps {
}

interface IAppState {
    folders: IFolder[];
    todayTasks: ITask[];
    tasksInInbox: ITask[];
    tasksMovedToTrash: ITaskMovedToTrash[];
}

export default App;
