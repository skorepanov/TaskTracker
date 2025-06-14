import React from "react";
import { Empty } from "antd";
import { useTranslation } from "../../hooks/useTranslation";

const NoTasks: React.FC = () => {
    const t = useTranslation();

    return (
        <Empty
            image={Empty.PRESENTED_IMAGE_SIMPLE}
            description={t("noTasks")}
        />
    );
};

export default NoTasks;
