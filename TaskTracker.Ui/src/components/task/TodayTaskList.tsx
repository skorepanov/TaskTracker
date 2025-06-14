import React from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../stores/RootStore";
import { useTranslation } from "../../hooks/useTranslation";
import TaskList from "./TaskList";
import TaskCreationPanel from "./TaskCreationPanel";

const TodayTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();
    const t = useTranslation();

    const incompletedTasks = taskStore.getTodayIncompletedTasks();
    const completedTasks = taskStore.getTodayCompletedTasks();

    return (
        <>
            <div className="task-list-header">
                <strong>{t("todayTasks")}</strong>
                <TaskCreationPanel dueDateTime={new Date()} />
            </div>
            <TaskList
                incompletedTasks={incompletedTasks}
                completedTasks={completedTasks}
                shouldShowFolder={true}
            />
        </>
    );
});

export default TodayTaskList;
