import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { ConfigProvider } from "antd";
import ru_RU from "antd/lib/locale/ru_RU";
import dayjs from "dayjs";
import "dayjs/locale/ru";
import updateLocale from "dayjs/plugin/updateLocale";
import App from "./App";
import "./styles/index.css";

dayjs.extend(updateLocale);
dayjs.updateLocale("ru-RU", {
    weekStart: 0,
});

const container = document.getElementById("root");
const root = createRoot(container!);

root.render(
    <StrictMode>
        <ConfigProvider locale={ru_RU}>
            <App />
        </ConfigProvider>
    </StrictMode>
);
