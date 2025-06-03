import React from "react";
import { observer } from "mobx-react-lite";

const NoTasks: React.FC = observer(() => {
    return <div style={{ textAlign: "center" }}>Нет задач</div>;
});

export default NoTasks;
