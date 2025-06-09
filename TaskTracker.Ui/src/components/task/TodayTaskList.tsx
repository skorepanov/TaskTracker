import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../../stores/RootStore";
import TaskList from "./TaskList";
import TaskCreationPanel from "./TaskCreationPanel";

const TodayTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTasks = taskStore.getTodayIncompletedTasks();
    const completedTasks = taskStore.getTodayCompletedTasks();

    return (
        <>
            <div style={{ paddingLeft: 10, paddingTop: 10 }}>
                <strong>Задачи на сегодня</strong>
            </div>
            <TaskCreationPanel dueDateTime={new Date()} />
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
