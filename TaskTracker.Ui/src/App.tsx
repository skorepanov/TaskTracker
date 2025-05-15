import React, { useState, useEffect } from 'react';
import dayjs, { Dayjs } from 'dayjs';
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
    const [incompletedTasks, setIncompletedTasks] = useState<ITask[]>([]);
    const [completedTasks, setCompletedTasks] = useState<ITask[]>([]);
    const [folders, setFolders] = useState<IFolder[]>([]);
    const [tasksMovedToTrash, setTasksMovedToTrash] = useState<ITask[]>([]);

    const todayIncompletedTasks = () => {
        const todayDate = dayjs(new Date()).startOf('day');

        return incompletedTasks
            .filter(t => isTodayIncompletedTask(t, todayDate));
    }

    const isTodayIncompletedTask = (task: ITask, todayDate: Dayjs) => {
        if (task.dueDateTime === null) {
            return false;
        }

        const dueDateTime = dayjs(task.dueDateTime).startOf('day');

        if (dueDateTime.isSame(todayDate) || dueDateTime.isBefore(todayDate)) {
            return true;
        }
    }

    const todayCompletedTasks = () => {
        const todayDate = dayjs(new Date()).startOf('day');

        return completedTasks
            .filter(t => isTodayCompletedTask(t, todayDate));
    }

    const isTodayCompletedTask = (task: ITask, todayDate: Dayjs) => {
        const completedDateTime = dayjs(task.completedDateTime).startOf('day');

        if (completedDateTime.isSame(todayDate)) {
            return true;
        }
    }

    const inboxIncompletedTasks = () => {
        return incompletedTasks
            .filter(task => task.folderId === null);
    }

    const inboxCompletedTasks = () => {
        return completedTasks
            .filter(task => task.folderId === null);
    }

    const loadIncompleteTasks = async () => {
        const url = `${AppUrl}/tasks/incomplete`;

        const incompletedTasks = await Api.get<ITask[]>(url);

        setIncompletedTasks(incompletedTasks);
    }

    const loadCompletedTasks = async () => {
        const url = `${AppUrl}/tasks/complete`;

        const completedTasks = await Api.get<ITask[]>(url);

        setCompletedTasks(completedTasks);
    }

    const loadFolders = async () => {
        const url = `${AppUrl}/folders`;

        const folders = await Api.get<IFolder[]>(url);

        setFolders(folders);
    }

    const createFolder = async (title: string) => {
        const url = `${AppUrl}/folders`;
        const params = {
            title: title,
            createdDateTime: new Date().toISOString()
        };

        const createdFolder = await Api.post<IFolder>(url, params);

        setFolders(prevFolders => [...prevFolders, createdFolder]);
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

        const createdTask = await Api.post<ITask>(url, params);

        setIncompletedTasks(prevTasks => [...prevTasks, createdTask]);
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

        const updatedTask = await Api.put<ITask>(url, params);

        setIncompletedTasks(prevTasks =>
            prevTasks.filter(t => t.id !== updatedTask.id));

        setCompletedTasks(prevTasks => [...prevTasks, updatedTask]);
    }

    const incompleteTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/incompleted`;

        const params = {
            modifiedDateTime: new Date().toISOString()
        };

        const updatedTask = await Api.put<ITask>(url, params);

        setCompletedTasks(prevTasks =>
            prevTasks.filter(t => t.id !== updatedTask.id));

        setIncompletedTasks(prevTasks => [...prevTasks, updatedTask]);
    }

    const moveTaskToTrash = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/movedToTrash`;

        const params = {
            movedToTrashDateTime: new Date().toISOString()
        };

        const taskMovedToTrash = await Api.put<ITask>(url, params);

        if (taskMovedToTrash.completedDateTime !== null) {
            setCompletedTasks(prevTasks =>
                prevTasks.filter(t => t.id !== taskMovedToTrash.id));
        } else {
            setIncompletedTasks(prevTasks =>
                prevTasks.filter(t => t.id !== taskMovedToTrash.id));
        }

        setTasksMovedToTrash(prevTasks => [...prevTasks, taskMovedToTrash]);
    }

    const moveTaskFromTrash = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}/movedFromTrash`;

        const params = {
            modifiedDateTime: new Date().toISOString()
        };

        const taskMovedFromTrash = await Api.put<ITask>(url, params);

        if (taskMovedFromTrash.completedDateTime !== null){
            setCompletedTasks(prevTasks => [...prevTasks, taskMovedFromTrash]);
        } else {
            setIncompletedTasks(prevTasks => [...prevTasks, taskMovedFromTrash]);
        }

        setTasksMovedToTrash(prevTasks =>
            prevTasks.filter(t => t.id !== taskMovedFromTrash.id));
    }

    const deleteTask = async (task: ITask) => {
        const url = `${AppUrl}/tasks/${task.id}`;

        await Api.delete(url);

        setTasksMovedToTrash(prevTasks =>
            prevTasks.filter(t => t.id !== task.id));
    }

    useEffect(() => {
        const loadData = async () => {
            await Promise.all([
                loadIncompleteTasks(),
                loadCompletedTasks(),
                loadFolders(),
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
                    incompletedTasks={incompletedTasks}
                    completedTasks={completedTasks}
                    completeTask={completeTask}
                    incompleteTask={incompleteTask}
                    moveTaskToTrash={moveTaskToTrash}
                />
                <strong>Задачи на сегодня</strong>
                {
                    todayIncompletedTasks().map(t =>
                        <Task
                            key={`today-${t.id}`}
                            task={t}
                            completeTask={completeTask}
                            incompleteTask={incompleteTask}
                            moveTaskToTrash={moveTaskToTrash}
                        />
                    )
                }
                {
                    todayCompletedTasks().map(t =>
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
                    inboxIncompletedTasks().map(t =>
                        <Task
                            key={`inbox-${t.id}`}
                            task={t}
                            completeTask={completeTask}
                            incompleteTask={incompleteTask}
                            moveTaskToTrash={moveTaskToTrash}
                        />
                    )
                }
                {
                    inboxCompletedTasks().map(t =>
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
