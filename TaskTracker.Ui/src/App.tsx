import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { Collapse, Space } from "antd";
import { useStore } from "./stores/RootStore";
import TaskCreationForm from "./components/TaskCreationForm";
import InboxTaskList from "./components/InboxTaskList";
import TodayTaskList from "./components/TodayTaskList";
import TaskInTrashList from "./components/TaskInTrashList";
import FolderList from "./components/FolderList";
import FolderCreationForm from "./components/FolderCreationForm";
import TagCreationForm from "./components/TagCreationForm";

const App: React.FC = observer(() => {
    const { taskStore, folderStore } = useStore();

    useEffect(() => {
        const loadData = async () => {
            await Promise.all([
                taskStore.fetchIncompleteTasks(),
                taskStore.fetchCompletedTasks(),
                taskStore.fetchTasksInTrash(),
                folderStore.fetchFolders(),
            ]);
        };

        loadData();
    }, [taskStore, folderStore]);

    const taskGroupComponents = [
        {
            key: "inbox",
            label: `Inbox`,
            children: <InboxTaskList />,
        },
        {
            key: "today",
            label: `Задачи на сегодня`,
            children: <TodayTaskList />,
        },
        {
            key: "trash",
            label: "Корзина",
            children: <TaskInTrashList />,
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
                    <TagCreationForm />
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
