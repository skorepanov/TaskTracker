import React from "react";
import { Empty } from "antd";

const NoTasks: React.FC = () => {
    return (
        <Empty
            image={Empty.PRESENTED_IMAGE_SIMPLE}
            description="Нет задач"
        />
    );
};

export default NoTasks;
