import React from 'react';
import { Space } from 'antd';

import { AppUrl, Api } from './api';
import Task from './components/Task';
import FolderCreationForm from './components/FolderCreationForm';
import TaskCreationForm from './components/TaskCreationForm';
import DeletedTask from './components/DeletedTask';
import FolderList from './components/FolderList';
import IFolder from './interfaces/IFolder';
import ITask from './interfaces/ITask';
import IDeletedTask from './interfaces/IDeletedTask';

import 'antd/dist/antd.css';

class App extends React.Component<IAppProps, IAppState> {
    constructor(props: IAppProps) {
        super(props);

        this.state = {
            folders: [],
            todayTasks: [],
            deletedTasks: [],
        };
    }

    async loadFolders() {
        const url = `${AppUrl}/folders`;

        const folders = await Api.get<IFolder[]>(url);
        return this.setState({ folders: folders });
    }

    loadFolderTasks = async (folderId: number, isForcedLoad: boolean = false) => {
        const folders = this.state.folders;
        const folder = folders.find(f => f.id === folderId);

        if (folder === undefined) {
            return;
        }

        if (!isForcedLoad && Array.isArray(folder.tasks) && folder.tasks.length > 0) {
            return;
        }

        // TODO add paging for completed tasks
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

        await Api.post<IFolder>(url, { title: title });
        await this.loadFolders();
    }

    createTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks`;
        const params = { title: task.title, description: task.description,
                         dueDate: task.dueDate, folderId: task.folderId };

        await Api.post<ITask>(url, params);
        this.loadFolders();
        this.loadTodayTasks();
        this.loadFolderTasks(task.folderId, true);
    }

    async loadTodayTasks() {
        const url = `${AppUrl}/tasks/today`;

        const tasks = await Api.get<ITask[]>(url);
        return this.setState({ todayTasks: tasks });
    }

    async loadDeletedTasks() {
        const url = `${AppUrl}/tasks/deleted`;

        const tasks = await Api.get<IDeletedTask[]>(url);
        return this.setState({ deletedTasks: tasks });
    }

    componentDidMount() {
        this.loadFolders();
        this.loadTodayTasks();
        this.loadDeletedTasks();
    }

    render() {
        return (
            <>
                <Space direction='vertical'>
                    <FolderCreationForm createFolder={this.createFolder} />
                    <TaskCreationForm
                        folders={this.state.folders}
                        createTask={this.createTask}
                    />
                    <FolderList
                        folders={this.state.folders}
                        loadTasks={this.loadFolderTasks}
                    />
                    <strong>Задачи на сегодня</strong>
                    {
                        this.state.todayTasks.map(t =>
                            <Task task={t} key={t.id} />
                        )
                    }
                    <strong>Удалённые задачи</strong>
                    {
                        this.state.deletedTasks.map(t =>
                            <DeletedTask task={t} key={t.id} />
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
    deletedTasks: IDeletedTask[];
}

export default App;
