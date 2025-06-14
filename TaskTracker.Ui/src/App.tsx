import React, { useEffect } from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { ConfigProvider, Splitter } from "antd";
import dayjs from "dayjs";
import updateLocale from "dayjs/plugin/updateLocale";

import { useStore } from "./stores/RootStore";
import { LocaleProvider, useLocale } from "./contexts/LocaleContext";
import { locales } from "./locales/config";
import MainMenu from "./components/MainMenu";
import AllTaskList from "./components/task/AllTaskList";
import InboxTaskList from "./components/task/InboxTaskList";
import TodayTaskList from "./components/task/TodayTaskList";
import TaskInTrashList from "./components/task/TaskInTrashList";
import FolderTaskList from "./components/task/FolderTaskList";
import TagTaskList from "./components/task/TagTaskList";
import TaskUpdatePanel from "./components/task/TaskUpdatePanel";

const AppContent = observer(() => {
    const { locale } = useLocale();

    dayjs.extend(updateLocale);
    dayjs.updateLocale("ru-RU", {
        weekStart: 0,
    });

    return (
        <ConfigProvider locale={locales[locale].antd}>
            <BrowserRouter>
                <Splitter style={{ height: "100vh" }}>
                    <Splitter.Panel
                        defaultSize="270"
                        min="10%"
                        max="30%">
                        <MainMenu />
                    </Splitter.Panel>
                    <Splitter.Panel min="300">
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
                        </Routes>
                    </Splitter.Panel>
                    <Splitter.Panel min="300">
                        <TaskUpdatePanel />
                    </Splitter.Panel>
                </Splitter>
            </BrowserRouter>
        </ConfigProvider>
    );
});

const App: React.FC = observer(() => {
    const rootStore = useStore();

    useEffect(() => {
        const fetchInitialData = async () => {
            await rootStore.fetchInitialData();
        };

        fetchInitialData().catch(console.error);
    }, [rootStore]);

    return (
        <LocaleProvider>
            <AppContent />
        </LocaleProvider>
    );
});

export default App;
