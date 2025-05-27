import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { Collapse, Space } from "antd";
import { useStore } from "./stores/RootStore";
import TaskListView from "./components/TaskListView";
import TaskMovedToTrashListView from "./components/TaskMovedToTrashListView";
import TaskCreationForm from "./components/TaskCreationForm";
import FolderList from "./components/FolderList";
import FolderCreationForm from "./components/FolderCreationForm";

const App: React.FC = observer(() => {
    const { taskStore, folderStore } = useStore();

    useEffect(() => {
        const loadData = async () => {
            await Promise.all([
                taskStore.fetchIncompleteTasks(),
                taskStore.fetchCompletedTasks(),
                folderStore.fetchFolders(),
                taskStore.fetchTasksInTrash(),
            ]);
        };

        loadData();
    }, [taskStore, folderStore]);

    const todayIncompletedTaskComponents = taskStore
        .getTodayIncompletedTasks()
        .map(t => (
            <TaskListView
                key={`today-${t.id}`}
                task={t}
            />
        ));

    const todayCompletedTaskComponents = taskStore
        .getTodayCompletedTasks()
        .map(t => (
            <TaskListView
                key={`today-${t.id}`}
                task={t}
            />
        ));

    const inboxIncompletedTaskComponents = taskStore
        .getInboxIncompletedTasks()
        .map(t => (
            <TaskListView
                key={`inbox-${t.id}`}
                task={t}
            />
        ));

    const inboxCompletedTaskComponents = taskStore
        .getInboxCompletedTasks()
        .map(t => (
            <TaskListView
                key={`inbox-${t.id}`}
                task={t}
            />
        ));

    const taskMovedToTrashComponents = taskStore.tasksMovedToTrash.map(t => (
        <TaskMovedToTrashListView
            key={`trash-${t.id}`}
            task={t}
        />
    ));

    const taskGroupComponents = [
        {
            key: "today",
            label: "Задачи на сегодня",
            children: (
                <>
                    {todayIncompletedTaskComponents}
                    {todayCompletedTaskComponents}
                </>
            ),
        },
        {
            key: "inbox",
            label: "Inbox",
            children: (
                <>
                    {inboxIncompletedTaskComponents}
                    {inboxCompletedTaskComponents}
                </>
            ),
        },
        {
            key: "trash",
            label: "Корзина",
            children: <>{taskMovedToTrashComponents}</>,
        },
    ];

    return (
        <>
            <Space
                direction="horizontal"
                align="start">
                <Space direction="vertical">
                    <FolderCreationForm />
                    <TaskCreationForm />
                </Space>
                <FolderList />
                <Collapse
                    accordion
                    items={taskGroupComponents}
                />
            </Space>
        </>
    );
});

export default App;
