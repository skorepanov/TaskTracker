import React from "react";
import { observer } from "mobx-react-lite";
import { Select, Switch } from "antd";
import { SupportedLocale } from "../types/settings";
import { useSettings } from "../contexts/SettingsContext";
import { useTranslation } from "../hooks/useTranslation";

const UserSettingsPanel: React.FC = observer(() => {
    const { settings, updateSettings } = useSettings();
    const t = useTranslation();

    const languageOptions = [
        {
            value: "ru_RU",
            label: "Русский",
        },
        {
            value: "en_US",
            label: "English",
        },
    ];

    const handleLanguageChange = (locale: SupportedLocale) => {
        updateSettings({ locale: locale });
    };

    const handleThemeChange = (checked: boolean) => {
        updateSettings({ theme: checked ? "dark" : "light" });
    };

    return (
        <div>
            <Select<SupportedLocale>
                options={languageOptions}
                value={settings.locale}
                onChange={handleLanguageChange}
                style={{ width: 120, marginLeft: 20, marginBottom: 15 }}
            />
            <div style={{ marginLeft: 20, marginBottom: 20 }}>
                <Switch
                    checked={settings.theme === "dark"}
                    onChange={handleThemeChange}
                    style={{ width: 30, marginRight: 10 }}
                />
                {t("darkTheme")}
            </div>
        </div>
    );
});

export default UserSettingsPanel;
