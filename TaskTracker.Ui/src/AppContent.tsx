import React from "react";
import { BrowserRouter } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { ConfigProvider, Layout, Splitter } from "antd";
import dayjs from "dayjs";
import updateLocale from "dayjs/plugin/updateLocale";
import { useSettings } from "./contexts/SettingsContext";
import AppRoutes from "./components/AppRoutes";
import MainMenu from "./components/MainMenu";
import TaskUpdatePanel from "./components/task/TaskUpdatePanel";

const AppContent: React.FC = observer(() => {
    const { antdLocale, antdTheme } = useSettings();

    dayjs.extend(updateLocale);
    dayjs.updateLocale("ru-RU", {
        weekStart: 0,
    });

    return (
        <ConfigProvider
            locale={antdLocale}
            theme={antdTheme}>
            <Layout>
                <BrowserRouter>
                    <Splitter style={{ height: "100vh" }}>
                        <Splitter.Panel
                            defaultSize="270"
                            min="10%"
                            max="30%">
                            <MainMenu />
                        </Splitter.Panel>
                        <Splitter.Panel min="300">
                            <div
                                style={{
                                    display: "flex",
                                    flexDirection: "column",
                                    height: "100%",
                                }}>
                                <AppRoutes />
                            </div>
                        </Splitter.Panel>
                        <Splitter.Panel min="300">
                            <TaskUpdatePanel />
                        </Splitter.Panel>
                    </Splitter>
                </BrowserRouter>
            </Layout>
        </ConfigProvider>
    );
});

export default AppContent;
