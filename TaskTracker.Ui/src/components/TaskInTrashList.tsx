import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../stores/RootStore";
import TaskMovedToTrashListView from "./TaskMovedToTrashListView";

const TaskInTrashList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const taskComponents = taskStore.tasksMovedToTrash.map(t => (
        <TaskMovedToTrashListView
            key={`trash-${t.id}`}
            task={t}
        />
    ));

    return (
        <>
            <strong>Корзина</strong>
            <Divider />
            {taskComponents}
        </>
    );
});

export default TaskInTrashList;
