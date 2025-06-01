import React, { useEffect } from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Splitter } from "antd";
import { useStore } from "./stores/RootStore";
import MainMenu from "./components/MainMenu";
import AllTaskList from "./components/AllTaskList";
import InboxTaskList from "./components/InboxTaskList";
import TodayTaskList from "./components/TodayTaskList";
import TaskInTrashList from "./components/TaskInTrashList";
import FolderTaskList from "./components/FolderTaskList";

const App: React.FC = observer(() => {
    const rootStore = useStore();

    useEffect(() => {
        const fetchInitialData = async () => {
            await rootStore.fetchInitialData();
        };

        fetchInitialData().catch(console.error);
    }, [rootStore]);

    return (
        <BrowserRouter>
            <Splitter>
                <Splitter.Panel
                    defaultSize="270"
                    min="10%"
                    max="30%"
                    style={{ height: "100%" }}>
                    <MainMenu />
                </Splitter.Panel>
                <Splitter.Panel>
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
                    </Routes>
                </Splitter.Panel>
            </Splitter>
        </BrowserRouter>
    );
});

export default App;
