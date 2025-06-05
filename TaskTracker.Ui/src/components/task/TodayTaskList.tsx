import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../../stores/RootStore";
import TaskList from "./TaskList";
import TaskCreationModalButton from "./TaskCreationModalButton";

const TodayTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTasks = taskStore.getTodayIncompletedTasks();
    const completedTasks = taskStore.getTodayCompletedTasks();

    return (
        <>
            <div style={{ float: "left" }}>
                <strong>Задачи на сегодня</strong>
            </div>
            <div style={{ float: "right" }}>
                <TaskCreationModalButton dueDateTime={new Date()} />
            </div>
            <Divider />
            <TaskList
                incompletedTasks={incompletedTasks}
                completedTasks={completedTasks}
                shouldShowFolder={true}
            />
        </>
    );
});

export default TodayTaskList;
