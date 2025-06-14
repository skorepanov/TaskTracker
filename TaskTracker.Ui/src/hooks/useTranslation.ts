import ru from "../locales/ru";
import en from "../locales/en";
import { useLocale } from "../contexts/LocaleContext";

type TranslationKey = keyof typeof ru;
type Translations = Record<TranslationKey, string>;

const translations: Record<"ru" | "en", Translations> = {
    ru,
    en,
};

export const useTranslation = () => {
    const { locale } = useLocale();

    return (key: TranslationKey) => translations[locale][key] || key;
};
