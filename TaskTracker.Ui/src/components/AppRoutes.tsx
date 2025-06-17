import React from "react";
import { observer } from "mobx-react-lite";
import { Navigate, Route, Routes } from "react-router-dom";
import AllTaskList from "./task/AllTaskList";
import InboxTaskList from "./task/InboxTaskList";
import TodayTaskList from "./task/TodayTaskList";
import TaskInTrashList from "./task/TaskInTrashList";
import FolderTaskList from "./task/FolderTaskList";
import TagTaskList from "./task/TagTaskList";

const AppRoutes: React.FC = observer(() => {
    return (
        <Routes>
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
            <Route
                path="/tags"
                element={<TagTaskList />}>
                <Route
                    path=":id"
                    element={<TagTaskList />}
                />
            </Route>
            <Route
                path="*"
                element={<Navigate to="/today" />}
            />
        </Routes>
    );
});

export default AppRoutes;
