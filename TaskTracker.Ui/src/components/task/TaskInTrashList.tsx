import React from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../stores/RootStore";
import { useTranslation } from "../../hooks/useTranslation";
import TaskInTrashListItem from "./TaskInTrashListItem";

const TaskInTrashList: React.FC = observer(() => {
    const { taskStore } = useStore();
    const t = useTranslation();

    const tasks = taskStore.getTasksMovedToTrash();

    const taskComponents = tasks.map(t => (
        <TaskInTrashListItem
            key={t.id}
            task={t}
        />
    ));

    return (
        <>
            <div
                className="task-list-header"
                style={{ height: 40 }}>
                <strong>{t("tasksInTrash")}</strong>
            </div>
            <div
                className="scrollable-container"
                style={{ paddingLeft: 10 }}>
                {taskComponents}
            </div>
        </>
    );
});

export default TaskInTrashList;
