import React from "react";
import { observer } from "mobx-react-lite";
import { Select } from "antd";
import { useLocale } from "../contexts/LocaleContext";
import { locales, LocaleType } from "../locales/config";

const LanguageSwitcher: React.FC = observer(() => {
    const { locale, setLocale } = useLocale();

    const languageOptions = Object.entries(locales).map(([key, { name }]) => ({
        key: key,
        value: key as LocaleType,
        label: name,
    }));

    return (
        <Select<LocaleType>
            options={languageOptions}
            value={locale}
            onChange={setLocale}
            style={{ width: 120, marginLeft: 20, marginBottom: 20 }}
        />
    );
});

export default LanguageSwitcher;
