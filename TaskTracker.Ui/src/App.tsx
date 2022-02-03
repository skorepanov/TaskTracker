import React from 'react';

import { AppUrl, Api } from './api';
import Folder from './components/Folder';
import Task from './components/Task';
import IFolder from './interfaces/IFolder';
import ITask from './interfaces/ITask';

class App extends React.Component<IAppProps, IAppState> {
    constructor(props: IAppProps) {
        super(props);
    
        this.state = {
            folders: [],
            todayTasks: [],
        };
    }

    async loadFolders() {
        const url = `${AppUrl}/folders`;
    
        const folders = await Api.get<IFolder[]>(url);
        return this.setState({ folders: folders });
    }

    async loadTodayTasks() {
        const url = `${AppUrl}/tasks/today`;

        const tasks = await Api.get<ITask[]>(url);
        return this.setState({ todayTasks: tasks });
    }

    componentDidMount() {
        this.loadFolders();
        this.loadTodayTasks();
    }

    render() {
        return (
        <div>
            <strong>Folders</strong>
            {
                this.state.folders.map(f =>
                    <Folder folder={f} key={f.id} />
                )
            }
            <strong>Today tasks</strong>
            {
                this.state.todayTasks.map(t =>
                    <Task task={t} key={t.id} />
                )
            }
        </div>
        );
    }
}

interface IAppProps {
}

interface IAppState {
    folders: IFolder[];
    todayTasks: ITask[];
}

export default App;
