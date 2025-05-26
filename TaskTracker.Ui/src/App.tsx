import React, { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Space } from 'antd';
import { useStore } from './stores/RootStore';
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
    }, [taskStore, folderStore]);

    return (
        <>
            <Space direction='vertical'>
                <FolderCreationForm />
                <TaskCreationForm />
                <FolderList />
                <strong>Задачи на сегодня</strong>
                {
                    taskStore.getTodayIncompletedTasks().map(t =>
                        <Task
                            key={`today-${t.id}`}
                            task={t}
                        />
                    )
                }
                {
                    taskStore.getTodayCompletedTasks().map(t =>
                        <Task
                            key={`today-${t.id}`}
                            task={t}
                        />
                    )
                }
                <strong>Inbox</strong>
                {
                    taskStore.getInboxIncompletedTasks().map(t =>
                        <Task
                            key={`inbox-${t.id}`}
                            task={t}
                        />
                    )
                }
                {
                    taskStore.getInboxCompletedTasks().map(t =>
                        <Task
                            key={`inbox-${t.id}`}
                            task={t}
                        />
                    )
                }
                <strong>Корзина</strong>
                {
                    taskStore.tasksMovedToTrash.map(t =>
                        <TaskMovedToTrash
                            key={`trash-${t.id}`}
                            task={t}
                        />
                    )
                }
            </Space>
        </>
    );
});

export default App;
