import React, { useEffect } from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Space } from "antd";
import { useStore } from "./stores/RootStore";
import MainMenu from "./components/MainMenu";
import TaskCreationForm from "./components/TaskCreationForm";
import AllTaskList from "./components/AllTaskList";
import InboxTaskList from "./components/InboxTaskList";
import TodayTaskList from "./components/TodayTaskList";
import TaskInTrashList from "./components/TaskInTrashList";
import FolderTaskList from "./components/FolderTaskList";
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

    return (
        <BrowserRouter>
            <Space
                direction="horizontal"
                align="start">
                <MainMenu />
                <Routes>
                    <Route
                        path="/"
                        element={
                            <Space
                                direction="horizontal"
                                align="start">
                                <Space direction="vertical">
                                    <FolderCreationForm />
                                    <TaskCreationForm />
                                    <TagCreationForm />
                                </Space>
                                <FolderList />
                            </Space>
                        }
                    />
                    <Route
                        path="/all"
                        element={<AllTaskList />}
                    />
                    <Route
                        path="/inbox"
                        element={<InboxTaskList />}
                    />
                    <Route
                        path="/today"
                        element={<TodayTaskList />}
                    />
                    <Route
                        path="/trash"
                        element={<TaskInTrashList />}
                    />
                    <Route
                        path="/folders"
                        element={<FolderTaskList />}>
                        <Route
                            path=":id"
                            element={<FolderTaskList />}
                        />
                    </Route>
                </Routes>
            </Space>
        </BrowserRouter>
    );
});

export default App;
