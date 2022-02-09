import React from 'react';
import { Space } from 'antd';

import { AppUrl, Api } from './api';
import Folder from './components/Folder';
import Task from './components/Task';
import FolderCreationForm from './components/FolderCreationForm';
import TaskCreationForm from './components/TaskCreationForm';
import DeletedTask from './components/DeletedTask';
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

    createFolder = async (title: string) => {
        const url = `${AppUrl}/folders`;

        await Api.post<IFolder>(url, { title: title });
        await this.loadFolders();
    }

    createTask = async (title: string, description: string, folderId: number) => {
        const url = `${AppUrl}/tasks`;
        const params = { title: title, description: description, folderId: folderId };

        await Api.post<ITask>(url, params);
        this.loadFolders();
        this.loadTodayTasks();
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
                    <strong>Папки</strong>
                    {
                        this.state.folders.map(f =>
                            <Folder folder={f} key={f.id} />
                        )
                    }
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
