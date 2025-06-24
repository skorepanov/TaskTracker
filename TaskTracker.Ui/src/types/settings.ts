export type SupportedLocale = "ru_RU" | "en_US";
export type ColorTheme = "light" | "dark";

export interface IUserSettings {
    locale: SupportedLocale;
    theme: ColorTheme;
}

export const DEFAULT_SETTINGS: IUserSettings = {
    locale: "en_US",
    theme: "light",
};
