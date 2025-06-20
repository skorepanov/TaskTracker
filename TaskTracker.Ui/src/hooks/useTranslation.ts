import ru from "../locales/ru";
import en from "../locales/en";
import { useSettings } from "../contexts/SettingsContext";

type TranslationKey = keyof typeof ru;
type Translations = Record<TranslationKey, string>;

const translations: Record<"ru_RU" | "en_US", Translations> = {
    ru_RU: ru,
    en_US: en,
};

export const useTranslation = () => {
    const { settings } = useSettings();

    return (key: TranslationKey) => translations[settings.locale][key] || key;
};
