import React from "react";
import { BrowserRouter } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { ConfigProvider, Splitter } from "antd";
import dayjs from "dayjs";
import updateLocale from "dayjs/plugin/updateLocale";
import { useLocale } from "./contexts/LocaleContext";
import { locales } from "./locales/config";
import AppRoutes from "./components/AppRoutes";
import MainMenu from "./components/MainMenu";
import TaskUpdatePanel from "./components/task/TaskUpdatePanel";

const AppContent: React.FC = observer(() => {
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
                        <AppRoutes />
                    </Splitter.Panel>
                    <Splitter.Panel min="300">
                        <TaskUpdatePanel />
                    </Splitter.Panel>
                </Splitter>
            </BrowserRouter>
        </ConfigProvider>
    );
});

export default AppContent;
