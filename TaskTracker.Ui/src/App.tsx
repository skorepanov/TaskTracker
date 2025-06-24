import React, { useEffect, useState } from "react";
import { BrowserRouter } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { ConfigProvider, Layout, Spin, Splitter } from "antd";
import dayjs from "dayjs";
import updateLocale from "dayjs/plugin/updateLocale";

import { useStore } from "./stores/RootStore";
import { useSettings } from "./contexts/SettingsContext";
import { useTranslation } from "./hooks/useTranslation";
import MainMenu from "./components/MainMenu";
import AppRoutes from "./components/AppRoutes";
import TaskUpdatePanel from "./components/task/TaskUpdatePanel";

const App: React.FC = observer(() => {
    const rootStore = useStore();

    const { antdLocale, antdTheme } = useSettings();

    const t = useTranslation();

    const [loading, setLoading] = useState<boolean>(true);

    dayjs.extend(updateLocale);
    dayjs.updateLocale("en", {
        weekStart: 1,
    });

    useEffect(() => {
        const fetchInitialData = async () => {
            await rootStore.fetchInitialData();
            setLoading(false);
        };

        fetchInitialData().catch(console.error);
    }, [rootStore]);

    return (
        <ConfigProvider
            locale={antdLocale}
            theme={antdTheme}>
            <Spin
                spinning={loading}
                tip={t("loading")}
                fullscreen
            />
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

export default App;
