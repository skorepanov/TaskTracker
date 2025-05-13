import React, { useState, useEffect } from 'react';
import { Space } from 'antd';

import { AppUrl, Api } from './api';
import Task from './components/Task';
import FolderCreationForm from './components/FolderCreationForm';
import TaskCreationForm from './components/TaskCreationForm';
import TaskMovedToTrash from './components/TaskMovedToTrash';
import FolderList from './components/FolderList';
import IFolder from './interfaces/IFolder';
import ITask from './interfaces/ITask';

const App: React.FC = () => {
    const [folders, setFolders] = useState<IFolder[]>([]);
    const [todayTasks, setTodayTasks] = useState<ITask[]>([]);
    const [tasksInInbox, setTasksInInbox] = useState<ITask[]>([]);
    const [tasksMovedToTrash, setTasksMovedToTrash] = useState<ITask[]>([]);

    const loadFolders = async () => {
        const url = `${AppUrl}/folders`;

        const folders = await Api.get<IFolder[]>(url);

        setFolders(folders);
    }

    const loadFolderTasks = async (folderId: number, isForcedLoad: boolean = false) => {
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

        const newTasks = [...incompleteTasks, ...completedTasks];
        const newIncompleteTaskCount = incompleteTasks.length;

        setFolders(prev =>
            prev.map(f =>
                f.id === folder.id
                    ? {
                        ...f,
                        tasks: newTasks,
                        incompleteTaskCount: newIncompleteTaskCount }
                    : f));
    }

    const createFolder = async (title: string) => {
        const url = `${AppUrl}/folders`;
        const params = {
            title: title,
            createdDateTime: new Date().toISOString()
        };

        await Api.post<IFolder>(url, params);

        await loadFolders();
    }

    const createTask = async (
        title: string,
        description: string,
        dueDateTime: Date | null,
        folderId: number | null
    ) => {
        const url = `${AppUrl}/tasks`;

        const params = {
            title: title,
            description: description,
            dueDateTime: dueDateTime,
            folderId: folderId,
            createdDateTime: new Date().toISOString()
        };

        await Api.post<ITask>(url, params);

        const loadCurrentTasks = folderId !== null
            ? () => loadFolderTasks(folderId, true)
            : () => loadTasksInInbox();

        await Promise.all([
            loadFolders,
            loadTodayTasks,
            loadCurrentTasks
        ]);
    }

    const loadTodayTasks = async () => {
        const url = `${AppUrl}/tasks/today`;

        const tasks = await Api.get<ITask[]>(url);

        setTodayTasks(tasks);
    }

    const loadTasksInInbox = async () => {
        const url = `${AppUrl}/tasks/inbox`;

        const tasks = await Api.get<ITask[]>(url);

        setTasksInInbox(tasks);
    }

    const loadTasksInTrash = async () => {
        const url = `${AppUrl}/tasks/trash`;

        const tasks = await Api.get<ITask[]>(url);

        setTasksMovedToTrash(tasks);
    }

    const completeTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/completed`;

        const params = {
            completedDateTime: new Date().toISOString()
        };

        await Api.put<ITask>(url, params);

        if (task.folderId !== null) {
            await loadFolderTasks(task.folderId, true);
        } else {
            await loadTasksInInbox();
        }
    }

    const incompleteTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/incompleted`;

        const params = {
            modifiedDateTime: new Date().toISOString()
        };

        await Api.put<ITask>(url, params);

        if (task.folderId !== null) {
            await loadFolderTasks(task.folderId, true);
        } else {
            await loadTasksInInbox();
        }
    }

    const moveTaskToTrash = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/movedToTrash`;

        const params = {
            movedToTrashDateTime: new Date().toISOString()
        };

        await Api.put<ITask>(url, params);

        await loadTasksInTrash();

        if (task.folderId !== null) {
            await loadFolderTasks(task.folderId, true);
        } else {
            await loadTasksInInbox();
        }
    }

    const moveTaskFromTrash = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/movedFromTrash`;

        const params = {
            modifiedDateTime: new Date().toISOString()
        };

        await Api.put<ITask>(url, params);

        await loadTasksInTrash();

        if (task.folderId !== null) {
            await loadFolderTasks(task.folderId, true);
        } else {
            await loadTasksInInbox();
        }
    }

    const deleteTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}`;

        await Api.delete(url);

        await loadTasksInTrash();
    }

    useEffect(() => {
        const loadData = async () => {
            await Promise.all([
                loadFolders(),
                loadTodayTasks(),
                loadTasksInInbox(),
                loadTasksInTrash()
            ]);
        };

        loadData();
    }, []);

    return (
        <>
            <Space direction='vertical'>
                <FolderCreationForm
                    createFolder={createFolder}
                />
                <TaskCreationForm
                    folders={folders}
                    createTask={createTask}
                />
                <FolderList
                    folders={folders}
                    loadTasks={loadFolderTasks}
                    completeTask={completeTask}
                    incompleteTask={incompleteTask}
                    moveTaskToTrash={moveTaskToTrash}
                />
                <strong>Задачи на сегодня</strong>
                {
                    todayTasks.map(t =>
                        <Task
                            key={`today-${t.id}`}
                            task={t}
                            completeTask={completeTask}
                            incompleteTask={incompleteTask}
                            moveTaskToTrash={moveTaskToTrash}
                        />
                    )
                }
                <strong>Inbox</strong>
                {
                    tasksInInbox.map(t =>
                        <Task
                            key={`inbox-${t.id}`}
                            task={t}
                            completeTask={completeTask}
                            incompleteTask={incompleteTask}
                            moveTaskToTrash={moveTaskToTrash}
                        />
                    )
                }
                <strong>Корзина</strong>
                {
                    tasksMovedToTrash.map(t =>
                        <TaskMovedToTrash
                            key={`trash-${t.id}`}
                            task={t}
                            moveTaskFromTrash={moveTaskFromTrash}
                            deleteTask={deleteTask}
                        />
                    )
                }
            </Space>
        </>
    );
}

export default App;
