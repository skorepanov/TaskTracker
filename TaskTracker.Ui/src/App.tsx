import React from 'react';

import { AppUrl, Api } from './api';
import Folder from './components/Folder';
import Task from './components/Task';
import FolderCreationForm from './components/FolderCreationForm';
import DeletedTask from './components/DeletedTask';
import IFolder from './interfaces/IFolder';
import ITask from './interfaces/ITask';
import IDeletedTask from './interfaces/IDeletedTask'

class App extends React.Component<IAppProps, IAppState> {
    constructor(props: IAppProps) {
        super(props);

        this.state = {
            folders: [],
            todayTasks: [],
            deletedTasks: [],
        };

        this.createFolder = this.createFolder.bind(this);
    }

    async loadFolders() {
        const url = `${AppUrl}/folders`;

        const folders = await Api.get<IFolder[]>(url);
        return this.setState({ folders: folders });
    }

    async createFolder(title: string) {
        const url = `${AppUrl}/folders`;

        await Api.post<IFolder>(url, { title: title })
        await this.loadFolders();
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
        <div>
            <strong>Папки</strong>
            {
                this.state.folders.map(f =>
                    <Folder folder={f} key={f.id} />
                )
            }
            <br />
            <strong>Задачи на сегодня</strong>
            {
                this.state.todayTasks.map(t =>
                    <Task task={t} key={t.id} />
                )
            }
            <br />
            <strong>Удалённые задачи</strong>
            {
                this.state.deletedTasks.map(t =>
                    <DeletedTask task={t} key={t.id} />
                )
            }
            <br />
            <FolderCreationForm createFolder={this.createFolder} />
        </div>
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
