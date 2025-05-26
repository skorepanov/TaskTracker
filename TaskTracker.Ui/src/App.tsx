import React, { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { useStore } from './stores/RootStore';
import { Space } from 'antd';

import Task from './components/Task';
import FolderCreationForm from './components/FolderCreationForm';
import TaskCreationForm from './components/TaskCreationForm';
import TaskMovedToTrash from './components/TaskMovedToTrash';
import FolderList from './components/FolderList';

const App: React.FC = observer(() => {
    const { taskStore, folderStore } = useStore();

    useEffect(() => {
        const loadData = async () => {
            await Promise.all([
                taskStore.fetchIncompleteTasks(),
                taskStore.fetchCompletedTasks(),
                folderStore.fetchFolders(),
                taskStore.fetchTasksInTrash()
            ]);
        };

        loadData();
    }, []);

    return (
        <>
            <Space direction='vertical'>
                <FolderCreationForm
                    createFolder={folderStore.createFolder}
                />
                <TaskCreationForm
                    folders={folderStore.folders}
                    createTask={taskStore.createTask}
                />
                <FolderList
                    folders={folderStore.folders}
                    incompletedTasks={taskStore.incompletedTasks}
                    completedTasks={taskStore.completedTasks}
                    deleteFolder={folderStore.deleteFolder}
                    completeTask={taskStore.completeTask}
                    incompleteTask={taskStore.incompleteTask}
                    moveTaskToTrash={taskStore.moveTaskToTrash}
                />
                <strong>Задачи на сегодня</strong>
                {
                    taskStore.getTodayIncompletedTasks().map(t =>
                        <Task
                            key={`today-${t.id}`}
                            task={t}
                            completeTask={taskStore.completeTask}
                            incompleteTask={taskStore.incompleteTask}
                            moveTaskToTrash={taskStore.moveTaskToTrash}
                        />
                    )
                }
                {
                    taskStore.getTodayCompletedTasks().map(t =>
                        <Task
                            key={`today-${t.id}`}
                            task={t}
                            completeTask={taskStore.completeTask}
                            incompleteTask={taskStore.incompleteTask}
                            moveTaskToTrash={taskStore.moveTaskToTrash}
                        />
                    )
                }
                <strong>Inbox</strong>
                {
                    taskStore.getInboxIncompletedTasks().map(t =>
                        <Task
                            key={`inbox-${t.id}`}
                            task={t}
                            completeTask={taskStore.completeTask}
                            incompleteTask={taskStore.incompleteTask}
                            moveTaskToTrash={taskStore.moveTaskToTrash}
                        />
                    )
                }
                {
                    taskStore.getInboxCompletedTasks().map(t =>
                        <Task
                            key={`inbox-${t.id}`}
                            task={t}
                            completeTask={taskStore.completeTask}
                            incompleteTask={taskStore.incompleteTask}
                            moveTaskToTrash={taskStore.moveTaskToTrash}
                        />
                    )
                }
                <strong>Корзина</strong>
                {
                    taskStore.tasksMovedToTrash.map(t =>
                        <TaskMovedToTrash
                            key={`trash-${t.id}`}
                            task={t}
                            moveTaskFromTrash={taskStore.moveTaskFromTrash}
                            deleteTask={taskStore.deleteTask}
                        />
                    )
                }
            </Space>
        </>
    );
});

export default App;
