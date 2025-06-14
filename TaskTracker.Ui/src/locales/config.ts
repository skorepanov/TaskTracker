import { Locale } from "antd/es/locale";
import ruRU from "antd/es/locale/ru_RU";
import enUS from "antd/es/locale/en_US";

export type LocaleType = "ru" | "en";

interface ILocaleConfig {
    antd: Locale;
    name: string;
}

export const locales: Record<LocaleType, ILocaleConfig> = {
    ru: {
        antd: ruRU,
        name: "Русский",
    },
    en: {
        antd: enUS,
        name: "English",
    },
};

export const DEFAULT_LOCALE: LocaleType = "ru";
