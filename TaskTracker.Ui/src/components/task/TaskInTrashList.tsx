import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../../stores/RootStore";
import TaskInTrashListItem from "./TaskInTrashListItem";

const TaskInTrashList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const tasks = taskStore.getTasksMovedToTrash();

    const taskComponents = tasks.map(t => (
        <TaskInTrashListItem
            key={t.id}
            task={t}
        />
    ));

    return (
        <>
            <div style={{ paddingLeft: 10 }}>
                <strong>Корзина</strong>
            </div>
            <Divider />
            <div style={{ paddingLeft: 10 }}>{taskComponents}</div>
        </>
    );
});

export default TaskInTrashList;
