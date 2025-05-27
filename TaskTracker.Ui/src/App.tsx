import React, { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Space } from 'antd';
import { useStore } from './stores/RootStore';
import TaskListView from './components/TaskListView';
import TaskMovedToTrashListView from './components/TaskMovedToTrashListView';
import TaskCreationForm from './components/TaskCreationForm';
import FolderList from './components/FolderList';
import FolderCreationForm from './components/FolderCreationForm';

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
                        <TaskListView
                            key={`today-${t.id}`}
                            task={t}
                        />
                    )
                }
                {
                    taskStore.getTodayCompletedTasks().map(t =>
                        <TaskListView
                            key={`today-${t.id}`}
                            task={t}
                        />
                    )
                }
                <strong>Inbox</strong>
                {
                    taskStore.getInboxIncompletedTasks().map(t =>
                        <TaskListView
                            key={`inbox-${t.id}`}
                            task={t}
                        />
                    )
                }
                {
                    taskStore.getInboxCompletedTasks().map(t =>
                        <TaskListView
                            key={`inbox-${t.id}`}
                            task={t}
                        />
                    )
                }
                <strong>Корзина</strong>
                {
                    taskStore.tasksMovedToTrash.map(t =>
                        <TaskMovedToTrashListView
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
