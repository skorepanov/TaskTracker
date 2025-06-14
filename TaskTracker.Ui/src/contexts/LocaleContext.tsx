import React, { ReactNode, createContext, useState, useContext } from "react";
import { LocaleType, DEFAULT_LOCALE } from "../locales/config";

interface LocaleContextType {
    locale: LocaleType;
    setLocale: (locale: LocaleType) => void;
}

const LocaleContext = createContext<LocaleContextType | undefined>(undefined);

interface ILocaleProviderProps {
    children: ReactNode;
}

export const LocaleProvider: React.FC<ILocaleProviderProps> = ({
    children,
}) => {
    const [locale, setLocale] = useState<LocaleType>(DEFAULT_LOCALE);

    return (
        <LocaleContext.Provider value={{ locale, setLocale }}>
            {children}
        </LocaleContext.Provider>
    );
};

export const useLocale = (): LocaleContextType => {
    const context = useContext(LocaleContext);

    if (!context) {
        throw new Error("useLocale must be used within a LocaleProvider");
    }

    return context;
};
