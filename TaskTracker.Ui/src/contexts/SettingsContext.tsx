import React, { createContext, useContext, useState, useEffect } from "react";
import { ThemeConfig, theme } from "antd";
import { Locale } from "antd/es/locale";
import enUS from "antd/es/locale/en_US";
import ruRU from "antd/es/locale/ru_RU";
import { DEFAULT_SETTINGS, IUserSettings } from "../types/settings";

interface ISettingsContextType {
    settings: IUserSettings;
    updateSettings: (newSettings: Partial<IUserSettings>) => void;
    antdLocale: Locale;
    antdTheme: ThemeConfig;
}

const SettingsContext = createContext<ISettingsContextType | undefined>(
    undefined
);

export const SettingsProvider: React.FC<{ children: React.ReactNode }> = ({
    children,
}) => {
    const [settings, setSettings] = useState<IUserSettings>(() => {
        const savedSettings = localStorage.getItem("userSettings");

        if (savedSettings) {
            return JSON.parse(savedSettings);
        }

        const settings = DEFAULT_SETTINGS;

        const query = "(prefers-color-scheme: dark)";
        const isDarkTheme = window.matchMedia(query).matches;
        settings.theme = isDarkTheme ? "dark" : "light";

        return settings;
    });

    useEffect(() => {
        localStorage.setItem("userSettings", JSON.stringify(settings));
    }, [settings]);

    const updateSettings = (newSettings: Partial<IUserSettings>) => {
        setSettings(prev => ({ ...prev, ...newSettings }));
    };

    const locales = {
        ru_RU: ruRU,
        en_US: enUS,
    };

    const antdLocale = locales[settings.locale];

    const antdTheme: ThemeConfig =
        settings.theme === "dark"
            ? { algorithm: theme.darkAlgorithm }
            : {
                  algorithm: theme.defaultAlgorithm,
                  components: { Layout: { bodyBg: "#ffffff" } },
              };

    return (
        <SettingsContext.Provider
            value={{ settings, updateSettings, antdLocale, antdTheme }}>
            {children}
        </SettingsContext.Provider>
    );
};

export const useSettings = () => {
    const context = useContext(SettingsContext);

    if (!context) {
        throw new Error("useSettings must be used within a SettingsProvider");
    }

    return context;
};
